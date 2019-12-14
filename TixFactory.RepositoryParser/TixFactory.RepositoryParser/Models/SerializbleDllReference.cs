using System;
using System.Runtime.Serialization;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A serializable DLL reference.
	/// </summary>
	/// <seealso cref="IDllReference"/>
	[DataContract]
	public class SerializbleDllReference : SerializableReference
	{
		/// <summary>
		/// The <see cref="IDllReference.HintPath"/>.
		/// </summary>
		[DataMember(Name = "hintPath")]
		public string HintPath { get; set; }

		/// <summary>
		/// Initializes a new <see cref="SerializbleDllReference"/>.
		/// </summary>
		/// <param name="dllReference">The <see cref="IDllReference"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// - <paramref name="dllReference"/>
		/// </exception>
		public SerializbleDllReference(IDllReference dllReference)
			: base(dllReference)
		{
			HintPath = dllReference.HintPath;
		}
	}
}
