namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// The <see cref="IProject"/> type.
	/// </summary>
	public enum ProjectType
	{
		/// <summary>
		/// Library assembly.
		/// </summary>
		Assembly = 0,

		/// <summary>
		/// Test assembly.
		/// </summary>
		Tests = 1,

		/// <summary>
		/// Console application.
		/// </summary>
		ConsoleApplication = 2,

		/// <summary>
		/// Web application.
		/// </summary>
		WebApplication = 3,
	}
}
