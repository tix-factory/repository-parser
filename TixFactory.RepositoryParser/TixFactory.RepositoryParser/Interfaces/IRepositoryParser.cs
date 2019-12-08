using System.Collections.Generic;

namespace TixFactory.RepositoryParser
{
	public interface IRepositoryParser
	{
		IReadOnlyCollection<IProject> Load(string repositoryDirectory);
	}
}
