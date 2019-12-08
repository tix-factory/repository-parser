using System.Collections.Generic;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// For parsing project dependency graphs.
	/// </summary>
	public interface IDependencyGrapher
	{
		/// <summary>
		/// Parses all projects into a collection of collections based on dependencies.
		/// </summary>
		/// <remarks>
		/// Each collection can be built in parallel, the returned list of collections must be built in order.
		/// </remarks>
		/// <returns>A tiered collection of <see cref="IProject"/> collections.</returns>
		IReadOnlyCollection<IReadOnlyCollection<IProject>> ParseDependencyTiers(IReadOnlyCollection<IProject> allProjects);
	}
}
