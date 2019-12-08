namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IProjectDependency"/>
	public abstract class ProjectDependency : IProjectDependency
	{
		/// <inheritdoc cref="IProjectDependency.Name"/>
		public string Name { get; set; }

		/// <inheritdoc cref="IProjectDependency.Project"/>
		public IProject Project { get; set; }
	}
}
