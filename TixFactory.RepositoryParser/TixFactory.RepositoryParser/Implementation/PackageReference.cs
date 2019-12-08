namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IPackageReference"/>
	internal class PackageReference : ProjectDependency, IPackageReference
	{
		/// <inheritdoc cref="IPackageReference.RawVersion"/>
		public string Version { get; }

		/// <inheritdoc cref="IPackageReference.RawVersion"/>
		public string RawVersion { get; }

		public PackageReference(string name, string version, string rawVersion)
		{
			Name = name;
			Version = version;
			RawVersion = rawVersion;
		}
	}
}
