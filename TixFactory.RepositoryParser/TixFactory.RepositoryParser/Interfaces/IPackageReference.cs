namespace TixFactory.RepositoryParser
{
	public interface IPackageReference : IProjectDependency
	{
		string Version { get; }

		string RawVersion { get; }
	}
}
