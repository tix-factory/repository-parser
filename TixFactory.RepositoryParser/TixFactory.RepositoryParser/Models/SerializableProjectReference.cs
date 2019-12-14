using System;
using System.Runtime.Serialization;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A serializable project project reference.
	/// </summary>
	/// <seealso cref="IProjectReference"/>
	[DataContract]
	public class SerializableProjectReference : SerializableReference
	{
		/// <summary>
		/// The <see cref="IProjectReference.ProjectFilePath"/>.
		/// </summary>
		[DataMember(Name = "projectFilePath")]
		public string ProjectFilePath { get; }

		/// <summary>
		/// The <see cref="IProjectReference.RawProjectFilePath"/>.
		/// </summary>
		[DataMember(Name = "rawProjectFilePath")]
		public string RawProjectFilePath { get; }

		/// <summary>
		/// Initializes a new <see cref="SerializableProjectReference"/>.
		/// </summary>
		/// <param name="projectReference">The <see cref="IProjectReference"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// - <paramref name="projectReference"/>
		/// </exception>
		public SerializableProjectReference(IProjectReference projectReference)
			: base(projectReference)
		{
			ProjectFilePath = projectReference.ProjectFilePath;
			RawProjectFilePath = projectReference.RawProjectFilePath;
		}
	}
}
