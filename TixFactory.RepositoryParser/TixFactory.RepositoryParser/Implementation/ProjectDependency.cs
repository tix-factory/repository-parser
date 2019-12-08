using System;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IProjectDependency"/>
	public abstract class ProjectDependency : IProjectDependency
	{
		/// <inheritdoc cref="IProjectDependency.Name"/>
		public string Name { get; }

		/// <summary>
		/// Initializes a new <see cref="ProjectDependency"/>.
		/// </summary>
		/// <param name="name">The dependency name.</param>
		/// <exception cref="ArgumentException">
		/// - <paramref name="name"/> is <c>null</c> or whitespace.
		/// </exception>
		protected ProjectDependency(string name)
		{
			Name = name;
		}
	}
}
