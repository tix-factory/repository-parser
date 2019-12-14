using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IProject"/>
	internal class Project : IProject
	{
		private const string _AssemblyNameTagName = "AssemblyName";
		private const string _TargetFrameworkTagName = "TargetFramework";
		private const string _TargetFrameworksTagName = "TargetFrameworks";
		private const string _DllReferenceTagName = "Reference";
		private const string _ProjectReferenceTagName = "ProjectReference";
		private const string _PackageReferenceTagName = "PackageReference";
		private const string _VersionMetadataName = "Version";
		private const string _HintPathMetadataName = "HintPath";
		private const string _OutputTypeTagName = "OutputType";
		private const string _ConsoleApplicationOutputType = "Exe";
		private const string _IsPackablePropertyName = "IsPackable";
		private const string _SdkAttributeName = "Sdk";
		private const string _TestsIdentifierPackageName = "Microsoft.NET.Test.Sdk";
		private const string _WebSdk = "Microsoft.NET.Sdk.Web";

		private readonly Microsoft.Build.Evaluation.Project _MsProject;
		private readonly ISet<IProject> _ProjectDependencies;
		private readonly ISet<ProjectReference> _ProjectReferences;
		private readonly ISet<DllReference> _DllReferences;
		private readonly ISet<PackageReference> _PackageReferences;

		/// <inheritdoc cref="IProject.Name"/>
		public string Name { get; }

		/// <inheritdoc cref="IProject.FilePath"/>
		public string FilePath { get; }

		/// <inheritdoc cref="IProject.ProjectContents"/>
		public XElement ProjectContents { get; }

		/// <inheritdoc cref="IProject.Type"/>
		public ProjectType Type { get; }

		/// <inheritdoc cref="IProject.TargetFrameworks"/>
		public IReadOnlyCollection<string> TargetFrameworks { get; }

		/// <inheritdoc cref="IProject.ProjectDependencies"/>
		public IReadOnlyCollection<IProject> ProjectDependencies => _ProjectDependencies.ToArray();

		/// <inheritdoc cref="IProject.PackageReferences"/>
		public IReadOnlyCollection<IPackageReference> PackageReferences => _PackageReferences.ToArray();

		/// <inheritdoc cref="IProject.DllReferences"/>
		public IReadOnlyCollection<IDllReference> DllReferences => _DllReferences.ToArray();

		/// <inheritdoc cref="IProject.ProjectReferences"/>
		public IReadOnlyCollection<IProjectReference> ProjectReferences => _ProjectReferences.ToArray();

		public Project(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException($"{nameof(filePath)} does not match valid file path.", filePath);
			}

			Microsoft.Build.Evaluation.Project msProject;
			try
			{
				msProject = new Microsoft.Build.Evaluation.Project(filePath, null, null, ProjectCollection.GlobalProjectCollection, ProjectLoadSettings.IgnoreMissingImports);
				_MsProject = msProject;
			}
			catch (Exception e)
			{
				throw new ArgumentException($"'{nameof(filePath)}' could not be parsed as project.", nameof(filePath), e);
			}
			
			FilePath = msProject.FullPath.Replace('\\', '/');
			ProjectContents = XElement.Parse(msProject.Xml.RawXml, LoadOptions.PreserveWhitespace);
			Name = GetPropertyValue(_AssemblyNameTagName, raw: false);
			TargetFrameworks = ParseTargetFrameworks();

			_ProjectDependencies = new HashSet<IProject>();
			_ProjectReferences = ParseProjectReferences();
			_DllReferences = ParseDllReferences();
			_PackageReferences = ParsePackageReferences();
			Type = ParseProjectType();

			if (Name == null)
			{
				var nullNameLog = $"Failed to parse assembly name.\n\tProject: {FilePath}\n\tProperties:";
				foreach (var prop in msProject.AllEvaluatedProperties)
				{
					nullNameLog += $"\n\t\t{prop.Name}: {prop.EvaluatedValue}";
				}

				Console.WriteLine(nullNameLog);
			}
		}

		/// <inheritdoc cref="IProject.GetPropertyValue"/>
		public string GetPropertyValue(string propertyName, bool raw)
		{
			var property = _MsProject.GetProperty(propertyName);
			if (property == null)
			{
				return null;
			}

			if (raw)
			{
				return property.UnevaluatedValue;
			}

			return property.EvaluatedValue;
		}

		internal void LoadRepository(IReadOnlyCollection<IProject> allProjects)
		{
			foreach (var project in allProjects)
			{
				var packageReference = _PackageReferences.FirstOrDefault(p => p.Name == project.Name);
				if (packageReference != null)
				{
					packageReference.Project = project;
					_ProjectDependencies.Add(project);

					continue;
				}

				var dllReference = _DllReferences.FirstOrDefault(p => p.Name == project.Name);
				if (dllReference != null)
				{
					dllReference.Project = project;
					_ProjectDependencies.Add(project);

					continue;
				}

				var projectReference = _ProjectReferences.FirstOrDefault(p => p.ProjectFilePath == project.FilePath);
				if (projectReference != null)
				{
					projectReference.Project = project;
					projectReference.Name = project.Name;
					_ProjectDependencies.Add(project);

					continue;
				}
			}
		}

		private ISet<PackageReference> ParsePackageReferences()
		{
			var packageReferences = new HashSet<PackageReference>();

			foreach (var item in _MsProject.GetItems(_PackageReferenceTagName))
			{
				var version = item.GetMetadata(_VersionMetadataName);
				var packageReference = new PackageReference(item.EvaluatedInclude, version?.EvaluatedValue, version?.UnevaluatedValue);
				packageReferences.Add(packageReference);
			}

			return packageReferences;
		}

		private ISet<ProjectReference> ParseProjectReferences()
		{
			var projectReferences = new HashSet<ProjectReference>();

			foreach (var item in _MsProject.GetItems(_ProjectReferenceTagName))
			{
				var projectFilePath = Path.GetFullPath(item.EvaluatedInclude, _MsProject.DirectoryPath).Replace('\\', '/');
				projectReferences.Add(new ProjectReference(projectFilePath, item.EvaluatedInclude));
			}

			return projectReferences;
		}

		private ISet<DllReference> ParseDllReferences()
		{
			var dllReferences = new HashSet<DllReference>();

			foreach (var item in _MsProject.GetItems(_DllReferenceTagName))
			{
				string name;
				var hintPath = item.GetMetadata(_HintPathMetadataName);

				if (string.IsNullOrWhiteSpace(hintPath?.EvaluatedValue))
				{
					name = item.EvaluatedInclude;
				}
				else
				{
					name = Path.GetFileNameWithoutExtension(hintPath.EvaluatedValue);
				}

				var dllReference = new DllReference(name, hintPath?.EvaluatedValue);
				dllReferences.Add(dllReference);
			}

			return dllReferences;
		}

		private IReadOnlyCollection<string> ParseTargetFrameworks()
		{
			var targetFrameworks = new HashSet<string>();

			var targetFramework = GetPropertyValue(_TargetFrameworkTagName, raw: false);
			if (string.IsNullOrWhiteSpace(targetFramework))
			{
				var targetFrameworksCsv = GetPropertyValue(_TargetFrameworksTagName, raw: false);
				foreach (var framework in targetFrameworksCsv.Split(';'))
				{
					if (!string.IsNullOrWhiteSpace(framework))
					{
						targetFrameworks.Add(framework.Trim());
					}
				}
			}
			else
			{
				targetFrameworks.Add(targetFramework);
			}

			return targetFrameworks;
		}

		private ProjectType ParseProjectType()
		{
			var sdkAttribute = ProjectContents?.Attributes().FirstOrDefault(a => _SdkAttributeName.Equals(a.Name.LocalName, StringComparison.OrdinalIgnoreCase));
			if (_WebSdk.Equals(sdkAttribute?.Value, StringComparison.OrdinalIgnoreCase))
			{
				return ProjectType.WebApplication;
			}

			var outputType = GetPropertyValue(_OutputTypeTagName, raw: false);
			if (_ConsoleApplicationOutputType.Equals(outputType?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return ProjectType.ConsoleApplication;
			}

			if (PackageReferences.Any(r => r.Name == _TestsIdentifierPackageName))
			{
				var isPackable = GetPropertyValue(_IsPackablePropertyName, raw: false);
				var notPackableValue = "false";
				if (notPackableValue.Equals(isPackable, StringComparison.OrdinalIgnoreCase))
				{
					return ProjectType.Tests;
				}
			}

			return ProjectType.Assembly;
		}
	}
}
