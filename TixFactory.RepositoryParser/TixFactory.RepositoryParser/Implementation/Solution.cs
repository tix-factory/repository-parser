using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="ISolution"/>
	internal class Solution : ISolution
	{
		private readonly ISet<IProject> _Projects;

		/// <inheritdoc cref="ISolution.Name"/>
		public string Name { get; }

		/// <inheritdoc cref="ISolution.FilePath"/>
		public string FilePath { get; }

		/// <inheritdoc cref="ISolution.ProjectFilePaths"/>
		public IReadOnlyCollection<string> ProjectFilePaths { get; }

		/// <inheritdoc cref="ISolution.RawProjectFilePaths"/>
		public IReadOnlyCollection<string> RawProjectFilePaths { get; }

		/// <inheritdoc cref="ISolution.Projects"/>
		public IReadOnlyCollection<IProject> Projects => _Projects.ToArray();

		public Solution(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException($"{nameof(filePath)} does not match valid file path.", filePath);
			}

			SolutionFile msSolution;
			try
			{
				msSolution = SolutionFile.Parse(filePath);
			}
			catch(Exception e)
			{
				throw new ArgumentException($"'{nameof(filePath)}' could not be parsed as solution.", nameof(filePath), e);
			}

			var projectFilePaths = new HashSet<string>();
			var rawProjectFilePaths = new HashSet<string>();

			foreach (var project in msSolution.ProjectsInOrder)
			{
				projectFilePaths.Add(project.AbsolutePath.Replace('\\', '/'));
				rawProjectFilePaths.Add(project.RelativePath);
			}

			Name = Path.GetFileNameWithoutExtension(filePath);
			FilePath = filePath.Replace('\\', '/');
			ProjectFilePaths = projectFilePaths;
			RawProjectFilePaths = rawProjectFilePaths;

			_Projects = new HashSet<IProject>();
		}

		internal void Load(IReadOnlyCollection<IProject> allProjects)
		{
			foreach (var project in allProjects)
			{
				if (ProjectFilePaths.Contains(project.FilePath))
				{
					_Projects.Add(project);
				}
			}
		}
	}
}
