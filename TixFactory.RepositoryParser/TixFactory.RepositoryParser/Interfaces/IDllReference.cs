namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A DLL reference.
	/// </summary>
	public interface IDllReference : IProjectDependency
	{
		/// <summary>
		/// The parsed DLL hint path from the <see cref="IProject"/>.
		/// </summary>
		string HintPath { get; }
	}
}
