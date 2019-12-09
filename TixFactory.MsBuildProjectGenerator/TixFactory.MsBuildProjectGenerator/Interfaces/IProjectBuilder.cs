using System.Collections.Generic;
using System.Xml.Linq;
using TixFactory.RepositoryParser;

namespace TixFactory.MsBuildProjectGenerator
{
	public interface IProjectBuilder
	{
		XElement BuildBuildProject(IReadOnlyCollection<IProject> allProjects);
	}
}
