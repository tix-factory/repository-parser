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
		private readonly Microsoft.Build.Evaluation.Project _MsProject;
		private readonly ISet<IProject> _ProjectDependencies;
		private readonly ISet<ProjectReference> _ProjectReferences;

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
		public IReadOnlyCollection<IProjectReference> ProjectReferences => _ProjectReferences.ToArray();

		public Project(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException($"{nameof(filePath)} does not match valid file path.", filePath);
			}

			FilePath = filePath;

			var projectFileContents = File.ReadAllText(filePath);
			ProjectContents = XElement.Parse(projectFileContents, LoadOptions.PreserveWhitespace);

			var msProject = new Microsoft.Build.Evaluation.Project(filePath);
			_MsProject = msProject;

			Name = GetAssemblyName(filePath);

			_ProjectDependencies = new HashSet<IProject>();
			_ProjectReferences = new HashSet<ProjectReference>();
		}

		/// <inheritdoc cref="IProject.GetPropertyValue"/>
		public string GetPropertyValue(string propertyName, bool followDependentProperties)
		{
			return null;
		}

		internal void LoadRepository(IReadOnlyCollection<IProject> allProjects)
		{
			foreach (var project in allProjects)
			{
				if (PackageReferences.Any(p => p.Name == project.Name))
				{
					_ProjectDependencies.Add(project);
				}
				else if (DllReferences.Any(p => p.Name == project.Name))
				{
					_ProjectDependencies.Add(project);
				}
				else
				{
					// TODO: Project references
				}
			}
		}

		private string GetAssemblyName(string filePath)
		{
			var assemblyName = GetPropertyValue(_AssemblyNameTagName, followDependentProperties: true);

			if (string.IsNullOrWhiteSpace(assemblyName))
			{
				assemblyName = Path.GetFileNameWithoutExtension(filePath);
			}

			return assemblyName;
		}
	}
}
