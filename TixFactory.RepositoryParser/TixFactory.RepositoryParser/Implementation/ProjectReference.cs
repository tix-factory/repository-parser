using System.IO;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IProjectReference"/>
	internal class ProjectReference : ProjectDependency, IProjectReference
	{
		/// <inheritdoc cref="IProjectReference.Project"/>
		public IProject Project { get; internal set; }

		/// <inheritdoc cref="IProjectReference.ProjectFilePath"/>
		public string ProjectFilePath { get; }

		/// <summary>
		/// Initializes a new <see cref="ProjectReference"/>.
		/// </summary>
		/// <param name="projectFilePath">The referenced project file path.</param>
		/// <exception cref="FileNotFoundException">
		/// - <paramref name="projectFilePath"/> does not map to valid project file.
		/// </exception>
		public ProjectReference(string projectFilePath)
			: base(Path.GetFileNameWithoutExtension(projectFilePath))
		{
			if (!File.Exists(projectFilePath))
			{
				throw new FileNotFoundException($"'{nameof(projectFilePath)}' does not map to valid project file path.", projectFilePath);
			}

			ProjectFilePath = projectFilePath;
		}
	}
}
