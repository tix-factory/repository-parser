using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TixFactory.RepositoryParser
{
	internal class Project : IProject
	{
		private const string _AssemblyNameTagName = "AssemblyName";
		private const string _TargetFrameworkTagName = "TargetFramework";
		private const string _TargetFrameworksTagName = "TargetFrameworks";

		private readonly Regex _PropertyRegex = new Regex(@"\$\((\w+)\)");

		/// <inheritdoc cref="IProject.Name"/>
		public string Name { get; }

		/// <inheritdoc cref="IProject.FilePath"/>
		public string FilePath { get; }

		/// <inheritdoc cref="IProject.ProjectContents"/>
		public XElement ProjectContents { get; }

		/// <inheritdoc cref="IProject.TargetFrameworks"/>
		public IReadOnlyCollection<string> TargetFrameworks { get; }

		/// <inheritdoc cref="IProject.ProjectDependencies"/>
		public IReadOnlyCollection<IProject> ProjectDependencies { get; }

		/// <inheritdoc cref="IProject.PackageReferences"/>
		public IReadOnlyCollection<IPackageReference> PackageReferences { get; }

		/// <inheritdoc cref="IProject.DllReferences"/>
		public IReadOnlyCollection<IDllReference> DllReferences { get; }

		/// <inheritdoc cref="IProject.ProjectReferences"/>
		public IReadOnlyCollection<IProjectReference> ProjectReferences { get; }

		public Project(string filePath, XElement projectContents)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException($"{nameof(filePath)} does not match valid file path.", filePath);
			}

			ProjectContents = projectContents ?? throw new ArgumentNullException(nameof(projectContents));
			Name = GetAssemblyName(filePath, projectContents);
			FilePath = filePath;
			TargetFrameworks = ParseTargetFrameworks(projectContents);
		}

		/// <inheritdoc cref="IProject.GetPropertyValue"/>
		public string GetPropertyValue(string propertyName, bool followDependentProperties)
		{
			var propertyTag = ProjectContents.Descendants().FirstOrDefault(e => propertyName.Equals(e.Name.LocalName, StringComparison.OrdinalIgnoreCase));
			if (propertyTag == null)
			{
				return null;
			}

			var value = propertyTag.Value;
			if (followDependentProperties && _PropertyRegex.IsMatch(value))
			{
				foreach (Match match in _PropertyRegex.Matches(value))
				{
					var dependentPropertyValue = GetPropertyValue(match.Groups[1].ToString(), followDependentProperties: true);
					if (dependentPropertyValue != null)
					{
						value = value.Replace(match.Value, dependentPropertyValue);
					}
				}
			}
			
			return value;
		}

		private string GetAssemblyName(string filePath, XElement projectElement)
		{
			var assemblyNameElement = projectElement.Descendants().FirstOrDefault(e => e.Name.LocalName == _AssemblyNameTagName);
			var assemblyName = assemblyNameElement?.Value.Trim();

			if (string.IsNullOrWhiteSpace(assemblyName))
			{
				assemblyName = Path.GetFileNameWithoutExtension(filePath);
			}

			return assemblyName;
		}

		private IReadOnlyCollection<string> ParseTargetFrameworks(XElement projectElement)
		{
			// https://docs.microsoft.com/en-us/dotnet/core/tools/csproj
			// TagetFramework takes precedence over TargetFrameworks
			var targetFrameworkTag = projectElement.Descendants().FirstOrDefault(e => e.Name.LocalName == _TargetFrameworkTagName);
			var targetFramework = targetFrameworkTag?.Value.Trim();
			var targetFrameworks = new HashSet<string>();

			if (string.IsNullOrWhiteSpace(targetFramework))
			{
				var targetFrameworksTag = projectElement.Descendants().FirstOrDefault(e => e.Name.LocalName == _TargetFrameworksTagName);
				if (!string.IsNullOrWhiteSpace(targetFrameworksTag?.Value))
				{
					foreach (var framework in targetFrameworksTag.Value.Split(';'))
					{
						var trimmedFramework = framework.Trim();
						if (!string.IsNullOrWhiteSpace(trimmedFramework))
						{
							targetFrameworks.Add(trimmedFramework);
						}
					}
				}
			}
			else
			{
				targetFrameworks.Add(targetFramework);
			}

			return targetFrameworks;
		}
	}
}
