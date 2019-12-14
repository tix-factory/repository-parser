using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A serializable representation of an <see cref="IProject"/>.
	/// </summary>
	[DataContract]
	public class SerializableProject
	{
		/// <summary>
		/// The <see cref="IProject.Name"/>.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; }

		/// <summary>
		/// The <see cref="IProject.FilePath"/>.
		/// </summary>
		[DataMember(Name = "filePath")]
		public string FilePath { get; }

		/// <summary>
		/// The <see cref="IProject.Type"/>.
		/// </summary>
		[DataMember(Name = "type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ProjectType Type { get; }

		/// <summary>
		/// The <see cref="IProject.TargetFrameworks"/>.
		/// </summary>
		[DataMember(Name = "targetFrameworks")]
		public IReadOnlyCollection<string> TargetFrameworks { get; }

		/// <summary>
		/// The <see cref="IProject.PackageReferences"/> as <see cref="SerializablePackageReference"/>s.
		/// </summary>
		[DataMember(Name = "packageReferences")]
		public IReadOnlyCollection<SerializablePackageReference> PackageReferences { get; }

		/// <summary>
		/// The <see cref="IProject.DllReferences"/> as <see cref="SerializbleDllReference"/>s.
		/// </summary>
		[DataMember(Name = "dllReferences")]
		public IReadOnlyCollection<SerializbleDllReference> DllReferences { get; }

		/// <summary>
		/// The <see cref="IProject.ProjectReferences"/> as <see cref="SerializableProjectReference"/>s.
		/// </summary>
		[DataMember(Name = "projectReferences")]
		public IReadOnlyCollection<SerializableProjectReference> ProjectReferences { get; }

		/// <summary>
		/// Assembly names of directly referenced known <see cref="IProject"/>s.
		/// </summary>
		[DataMember(Name = "projectDependencies")]
		public IReadOnlyCollection<string> ProjectDependencies { get; }

		/// <summary>
		/// Initializes a new <see cref="SerializableProject"/>.
		/// </summary>
		/// <param name="project">The <see cref="IProject"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// - <paramref name="project"/>
		/// </exception>
		public SerializableProject(IProject project)
		{
			if (project == null)
			{
				throw new ArgumentNullException(nameof(project));
			}

			Name = project.Name;
			FilePath = project.FilePath;
			Type = project.Type;
			TargetFrameworks = project.TargetFrameworks;
			PackageReferences = project.PackageReferences.Select(r => new SerializablePackageReference(r)).ToArray();
			DllReferences = project.DllReferences.Select(r => new SerializbleDllReference(r)).ToArray();
			ProjectReferences = project.ProjectReferences.Select(r => new SerializableProjectReference(r)).ToArray();
			ProjectDependencies = project.ProjectDependencies.Select(p => p.Name).ToArray();
		}
	}
}
