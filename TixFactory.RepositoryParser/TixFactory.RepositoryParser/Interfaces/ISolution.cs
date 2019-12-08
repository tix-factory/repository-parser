using System.Collections.Generic;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A parsed solution file.
	/// </summary>
	public interface ISolution
	{
		/// <summary>
		/// The name of the solution.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The absolute solution file path.
		/// </summary>
		string FilePath { get; }

		/// <summary>
		/// The absolute file paths from the solution.
		/// </summary>
		IReadOnlyCollection<string> ProjectFilePaths { get; }

		/// <summary>
		/// The raw project file paths parsed from the solution.
		/// </summary>
		IReadOnlyCollection<string> RawProjectFilePaths { get; }

		/// <summary>
		/// <see cref="IProject"/>s in the repository that map to <see cref="ProjectFilePaths"/>.
		/// </summary>
		IReadOnlyCollection<IProject> Projects { get; }
	}
}
