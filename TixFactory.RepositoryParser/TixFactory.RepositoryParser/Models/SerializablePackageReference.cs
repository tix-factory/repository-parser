using System;
using System.Runtime.Serialization;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A serializable packager reference.
	/// </summary>
	/// <seealso cref="IPackageReference"/>
	[DataContract]
	public class SerializablePackageReference : SerializableReference
	{
		/// <summary>
		/// The <see cref="IPackageReference.Version"/>.
		/// </summary>
		[DataMember(Name = "version")]
		public string Version { get; }

		/// <summary>
		/// The <see cref="IPackageReference.RawVersion"/>.
		/// </summary>
		[DataMember(Name = "rawVersion")]
		public string RawVersion { get; }

		/// <summary>
		/// Initializes a new <see cref="SerializablePackageReference"/>.
		/// </summary>
		/// <param name="packageReference">The <see cref="IPackageReference"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// - <paramref name="packageReference"/>
		/// </exception>
		public SerializablePackageReference(IPackageReference packageReference)
			: base(packageReference)
		{
			Version = packageReference.Version;
			RawVersion = packageReference.RawVersion;
		}
	}
}
