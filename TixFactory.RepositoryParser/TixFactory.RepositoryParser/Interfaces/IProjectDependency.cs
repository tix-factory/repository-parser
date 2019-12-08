namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A base interface for project dependencies/references.
	/// </summary>
	public interface IProjectDependency
	{
		/// <summary>
		/// The dependency name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The <see cref="IProject"/> the reference maps to.
		/// </summary>
		IProject Project { get; }
	}
}
