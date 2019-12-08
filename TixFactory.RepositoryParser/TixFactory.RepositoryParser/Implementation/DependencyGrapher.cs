using System.Collections.Generic;
using System.Linq;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IDependencyGrapher"/>
	public class DependencyGrapher : IDependencyGrapher
	{
		/// <inheritdoc cref="IDependencyGrapher.ParseDependencyTiers"/>
		public IReadOnlyCollection<IReadOnlyCollection<IProject>> ParseDependencyTiers(IReadOnlyCollection<IProject> allProjects)
		{
			var dependencyTiers = new HashSet<IReadOnlyCollection<IProject>>();
			var remainingProjects = allProjects.ToList();
			
			while (remainingProjects.Any())
			{
				var tier = new HashSet<IProject>();

				foreach (var project in remainingProjects)
				{
					var canBuild = true;
					foreach (var dependentProject in project.ProjectDependencies)
					{
						if (remainingProjects.Contains(dependentProject))
						{
							canBuild = false;
							break;
						}
					}

					if (canBuild)
					{
						tier.Add(project);
					}
				}

				foreach (var project in tier)
				{
					remainingProjects.Remove(project);
				}

				dependencyTiers.Add(tier);
			}

			return dependencyTiers;
		}
	}
}
