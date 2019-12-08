namespace TixFactory.RepositoryParser
{
	public interface IProjectReference : IProjectDependency
	{
		IProject Project { get; }

		string ProjectFilePath { get; }
	}
}
