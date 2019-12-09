using System;
using System.IO;
using TixFactory.RepositoryParser;

namespace TixFactory.MsBuildProjectGenerator
{
	/// <summary>
	/// The application entry class.
	/// </summary>
	public class Program
	{
		private readonly IRepositoryParser _RepositoryParser;
		private readonly IProjectBuilder _ProjectBuilder;

		/// <summary>
		/// Initializes a new <see cref="Program"/>.
		/// </summary>
		public Program()
		{
			var repositoryParser = new RepositoryParser.RepositoryParser();
			var dependencyGrapher = new DependencyGrapher();
			var projectBuilder = new ProjectBuilder(dependencyGrapher);

			_RepositoryParser = repositoryParser;
			_ProjectBuilder = projectBuilder;
		}

		/// <summary>
		/// The application entry method.
		/// </summary>
		/// <param name="args">The application start arguments.</param>
		public static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.Error.WriteLine($"Exactly two query parameters expected: inputDirectory, outputFilePath\n\tExample: TixFactory.MsBuildProjectGenerator.exe E:/Git/tix-factory/nuget ./build.proj");
				Environment.Exit(1);
				return;
			}
			
			var program = new Program();
			program.Run(args[0], args[1]);
		}

		/// <summary>
		/// Runs the application.
		/// </summary>
		/// <param name="inputDirectory">The input directory to parse.</param>
		/// <param name="outputFilePath">The output file path for the parallel build project.</param>
		/// <exception cref="DirectoryNotFoundException">
		/// - <paramref name="inputDirectory"/> does not exist.
		/// - <paramref name="outputFilePath"/> is in directory that does not exist.
		/// </exception>
		public void Run(string inputDirectory, string outputFilePath)
		{
			if (!Directory.Exists(inputDirectory))
			{
				throw new DirectoryNotFoundException($"'{nameof(inputDirectory)}' does not exist.");
			}

			if (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
			{
				throw new DirectoryNotFoundException($"'{nameof(outputFilePath)}' must be in a directory that exists.");
			}

			var projects = _RepositoryParser.ParseProjects(inputDirectory);

			var buildProject = _ProjectBuilder.BuildBuildProject(projects);
			buildProject.Save(outputFilePath);
		}
	}
}
