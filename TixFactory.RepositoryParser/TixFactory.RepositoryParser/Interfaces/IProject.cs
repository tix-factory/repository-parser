using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TixFactory.RepositoryParser
{
	/// <summary>
	/// A parsed project.
	/// </summary>
	public interface IProject
	{
		/// <summary>
		/// The project assembly name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The project file path.
		/// </summary>
		string FilePath { get; }

		/// <summary>
		/// The project contents.
		/// </summary>
		XElement ProjectContents { get; }

		/// <summary>
		/// Target frameworks.
		/// </summary>
		IReadOnlyCollection<string> TargetFrameworks { get; }

		/// <summary>
		/// Direct dependencies of the <see cref="IProject"/>.
		/// </summary>
		/// <remarks>
		/// This collection only contains <see cref="IProject"/>s that also exist in the same repository.
		/// These are projects that are referenced via Dll, Package, or Project references.
		/// </remarks>
		IReadOnlyCollection<IProject> ProjectDependencies { get; }

		/// <summary>
		/// Direct package references in the <see cref="IProject"/>.
		/// </summary>
		IReadOnlyCollection<IPackageReference> PackageReferences { get; }

		/// <summary>
		/// Direct DLL references in the <see cref="IProject"/>.
		/// </summary>
		IReadOnlyCollection<IDllReference> DllReferences { get; }

		/// <summary>
		/// Direct project references in the <see cref="IProject"/>.
		/// </summary>
		/// <remarks>
		/// Dependencies that are added via project reference.
		/// </remarks>
		IReadOnlyCollection<IProjectReference> ProjectReferences { get; }

		/// <summary>
		/// Gets a property value by property name from the <see cref="ProjectContents"/>.
		/// </summary>
		/// <remarks>
		/// if <paramref name="raw"/> is <c>true</c> the property value is not evaluated.
		/// </remarks>
		/// <param name="propertyName">The property name to get the value of.</param>
		/// <param name="raw">Whether or not the property value should be evaluated.</param>
		/// <returns>The property value (or <c>null</c> if the property does not exist).</returns>
		/// <exception cref="ArgumentException">
		/// - <paramref name="propertyName"/> is null or whitespace.
		/// </exception>
		string GetPropertyValue(string propertyName, bool raw);
	}
}
