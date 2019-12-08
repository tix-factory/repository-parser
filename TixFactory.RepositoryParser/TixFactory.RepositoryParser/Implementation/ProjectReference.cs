using System;
using System.IO;

namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IProjectReference"/>
	internal class ProjectReference : ProjectDependency, IProjectReference
	{
		/// <inheritdoc cref="IProjectReference.ProjectFilePath"/>
		public string ProjectFilePath { get; }

		/// <inheritdoc cref="IProjectReference.RawProjectFilePath"/>
		public string RawProjectFilePath { get; }

		/// <summary>
		/// Initializes a new <see cref="ProjectReference"/>.
		/// </summary>
		/// <param name="projectFilePath">The full project file path.</param>
		/// <param name="rawProjectFilePath">The raw project file path from the reference.</param>
		/// <exception cref="ArgumentException">
		/// - <paramref name="rawProjectFilePath"/> is <c>null</c> or whitespace.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		/// </exception>
		public ProjectReference(string projectFilePath, string rawProjectFilePath)
		{
			if (string.IsNullOrWhiteSpace(rawProjectFilePath))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(rawProjectFilePath));
			}

			if (!File.Exists(projectFilePath))
			{
				throw new FileNotFoundException($"'{nameof(projectFilePath)}' is not a valid file path.", projectFilePath);
			}

			Name = Path.GetFileNameWithoutExtension(projectFilePath);
			ProjectFilePath = projectFilePath;
			RawProjectFilePath = rawProjectFilePath;
		}
	}
}
