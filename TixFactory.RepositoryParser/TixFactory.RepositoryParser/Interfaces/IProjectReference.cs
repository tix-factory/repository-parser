namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A project reference.
	/// </summary>
	public interface IProjectReference : IProjectDependency
	{
		/// <summary>
		/// The absolute path to the dependent project file.
		/// </summary>
		string ProjectFilePath { get; }

		/// <summary>
		/// The raw project file reference path.
		/// </summary>
		string RawProjectFilePath { get; }
	}
}
