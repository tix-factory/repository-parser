namespace TixFactory.RepositoryParser
{
	public interface IDllReference : IProjectDependency
	{
		string HintPath { get; }
	}
}
