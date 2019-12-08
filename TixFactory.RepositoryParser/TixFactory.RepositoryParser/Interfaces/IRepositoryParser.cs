using System.Collections.Generic;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// For parsing repository projects and solutions.
	/// </summary>
	public interface IRepositoryParser
	{
		/// <summary>
		/// Parses all projects in a directory.
		/// </summary>
		/// <param name="repositoryDirectory">The repository directory to parse the project files from.</param>
		/// <returns>The collection of all <see cref="IProject"/>s.</returns>
		IReadOnlyCollection<IProject> ParseProjects(string repositoryDirectory);

		/// <summary>
		/// Parses all the solution files in a directory.
		/// </summary>
		/// <param name="repositoryDirectory">The repository directory to parse the solution files from.</param>
		/// <param name="allProjects">All the projects from the repository directory (see <see cref="ParseProjects"/>).</param>
		/// <returns>The collection of all <see cref="ISolution"/>s.</returns>
		IReadOnlyCollection<ISolution> ParseSolutions(string repositoryDirectory, IReadOnlyCollection<IProject> allProjects);
	}
}
