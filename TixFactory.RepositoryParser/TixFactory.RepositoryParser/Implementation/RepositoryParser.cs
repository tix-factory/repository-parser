using System.Collections.Generic;
using System.IO;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IRepositoryParser"/>
	public class RepositoryParser : IRepositoryParser
	{
		/// <inheritdoc cref="IRepositoryParser.Load"/>
		public IReadOnlyCollection<IProject> Load(string repositoryDirectory)
		{
			if (!Directory.Exists(repositoryDirectory))
			{
				throw new DirectoryNotFoundException($"'{nameof(repositoryDirectory)}' does not map to valid directory.\n\tValue: {repositoryDirectory}");
			}

			var projects = new List<Project>();

			foreach (var projectFilePath in Directory.EnumerateFiles(repositoryDirectory, "*.csproj", SearchOption.AllDirectories))
			{
				var project = new Project(projectFilePath);
				projects.Add(project);
			}

			foreach (var project in projects)
			{
				project.LoadRepository(projects);
			}

			return projects;
		}
	}
}
