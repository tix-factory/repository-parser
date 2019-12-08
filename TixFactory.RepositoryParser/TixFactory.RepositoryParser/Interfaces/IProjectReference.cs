namespace TixFactory.RepositoryParser
{
	public interface IProjectReference : IProjectDependency
	{
		IProject Project { get; }
	}
}
