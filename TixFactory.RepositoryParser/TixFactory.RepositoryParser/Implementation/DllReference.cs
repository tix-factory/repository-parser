namespace TixFactory.RepositoryParser
{
	/// <inheritdoc cref="IDllReference"/>
	internal class DllReference : ProjectDependency, IDllReference
	{
		/// <inheritdoc cref="IDllReference.HintPath"/>
		public string HintPath { get; }

		public DllReference(string name, string hintPath)
		{
			Name = name;
			HintPath = hintPath;
		}
	}
}
