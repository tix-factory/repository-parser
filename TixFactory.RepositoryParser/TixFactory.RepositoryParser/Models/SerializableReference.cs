using System;
using System.Runtime.Serialization;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A serializable project reference.
	/// </summary>
	/// <remarks>
	/// A reference on a project, not a project project reference.
	/// </remarks>
	[DataContract]
	public abstract class SerializableReference
	{
		/// <summary>
		/// The referenced assembly name.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Initializes a new <see cref="SerializableReference"/>.
		/// </summary>
		/// <param name="projectDependency">The <see cref="IProjectDependency"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// - <paramref name="projectDependency"/>
		/// </exception>
		protected SerializableReference(IProjectDependency projectDependency)
		{
			if (projectDependency == null)
			{
				throw new ArgumentNullException(nameof(projectDependency));
			}

			Name = projectDependency.Name;
		}
	}
}
