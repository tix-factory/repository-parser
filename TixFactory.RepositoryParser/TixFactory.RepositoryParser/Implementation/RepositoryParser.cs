using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IRepositoryParser"/>
	public class RepositoryParser : IRepositoryParser
	{
		/// <inheritdoc cref="IRepositoryParser.ParseProjects"/>
		public IReadOnlyCollection<IProject> ParseProjects(string repositoryDirectory)
		{
			if (!Directory.Exists(repositoryDirectory))
			{
				throw new DirectoryNotFoundException($"'{nameof(repositoryDirectory)}' does not map to valid directory.\n\tValue: {repositoryDirectory}");
			}

			var projects = new Dictionary<string, Project>();

			foreach (var filePath in Directory.EnumerateFiles(repositoryDirectory, "*.csproj", SearchOption.AllDirectories).Select(p => p.Replace('\\', '/')))
			{
				var project = new Project(filePath);
				projects.Add(project.FilePath, project);
			}

			foreach (var project in projects.Values)
			{
				project.LoadRepository(projects.Values);
			}

			return projects.Values;
		}

		/// <inheritdoc cref="IRepositoryParser.ParseSolutions"/>
		public IReadOnlyCollection<ISolution> ParseSolutions(string repositoryDirectory, IReadOnlyCollection<IProject> allProjects)
		{
			if (!Directory.Exists(repositoryDirectory))
			{
				throw new DirectoryNotFoundException($"'{nameof(repositoryDirectory)}' does not map to valid directory.\n\tValue: {repositoryDirectory}");
			}

			if (allProjects == null)
			{
				throw new ArgumentNullException(nameof(allProjects));
			}

			var solutions = new List<Solution>();

			foreach (var filePath in Directory.EnumerateFiles(repositoryDirectory, "*.sln", SearchOption.AllDirectories).Select(p => p.Replace('\\', '/')))
			{
				var solution = new Solution(filePath);
				solution.Load(allProjects);
				solutions.Add(solution);
			}

			return solutions;
		}
	}
}
