namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A package reference.
	/// </summary>
	public interface IPackageReference : IProjectDependency
	{
		/// <summary>
		/// The evaluated version.
		/// </summary>
		string Version { get; }

		/// <summary>
		/// The unevaluated version value for the package reference.
		/// </summary>
		string RawVersion { get; }
	}
}
