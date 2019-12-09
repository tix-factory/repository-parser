using System;
using System.IO;
using TixFactory.RepositoryParser;

namespace TixFactory.MsBuildProjectGenerator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.Error.WriteLine($"Exactly two query parameters expected: inputDirectory, outputFilePath\n\tExample: TixFactory.MsBuildProjectGenerator.exe E:/Git/tix-factory/nuget ./build.proj");
				Environment.Exit(1);
				return;
			}

			var inputDirectory = args[0];
			if (!Directory.Exists(inputDirectory))
			{
				throw new DirectoryNotFoundException($"'{nameof(inputDirectory)}' does not exist.");
			}

			var outputFilePath = args[1];
			if (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
			{
				throw new DirectoryNotFoundException($"'{nameof(outputFilePath)}' must be in a directory that exists.");
			}

			var repositoryParser = new RepositoryParser.RepositoryParser();
			var dependencyGrapher = new DependencyGrapher();
			var projectBuilder = new ProjectBuilder(dependencyGrapher);

			var projects = repositoryParser.ParseProjects(inputDirectory);
			var solutions = repositoryParser.ParseSolutions(inputDirectory, projects);

			var buildProject = projectBuilder.BuildBuildProject(projects);
			var rawBuildProject = buildProject.ToString();
			buildProject.Save(outputFilePath);
		}
	}
}
