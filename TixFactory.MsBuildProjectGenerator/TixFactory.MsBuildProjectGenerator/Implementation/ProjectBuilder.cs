using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TixFactory.RepositoryParser;

namespace TixFactory.MsBuildProjectGenerator
{
	/// <inheritdoc cref="IProjectBuilder"/>
	public class ProjectBuilder : IProjectBuilder
	{
		private const string _BuildTagName = "MSBuild";
		private const string _BuildTagTargets = "Restore;Build";
		private const string _PublishTagTargets = "Publish";
		private const string _TestTagTargets = "Test";
		private const string _BuildTargetsAttributeName = "Targets";

		private readonly IDependencyGrapher _DependencyGrapher;
		private readonly XElement _BuildProjectTemplate;

		/// <summary>
		/// Initializes a new <see cref="ProjectBuilder"/>.
		/// </summary>
		/// <param name="dependencyGrapher">An <see cref="IDependencyGrapher"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// - <paramref name="dependencyGrapher"/>
		/// </exception>
		public ProjectBuilder(IDependencyGrapher dependencyGrapher)
		{
			_DependencyGrapher = dependencyGrapher ?? throw new ArgumentNullException(nameof(dependencyGrapher));
			_BuildProjectTemplate = BuildProjectTemplate();
		}

		/// <inheritdoc cref="IProjectBuilder.BuildBuildProject"/>
		public XElement BuildBuildProject(IReadOnlyCollection<IProject> allProjects)
		{
			var tiers = _DependencyGrapher.ParseDependencyTiers(allProjects);
			var testProjects = new HashSet<IProject>();
			var publishProjects = new HashSet<IProject>();

			var rootProject = new XElement(_BuildProjectTemplate);
			var buildTemplate = GetBuildTag(rootProject, _BuildTagTargets);
			var publishTemplate = GetBuildTag(rootProject, _PublishTagTargets);
			var testTemplate = GetBuildTag(rootProject, _TestTagTargets);

			foreach (var tier in tiers)
			{
				var buildProjects = new HashSet<IProject>();
				foreach (var project in tier)
				{
					switch (project.Type)
					{
						case ProjectType.Tests:
							testProjects.Add(project);
							break;
						case ProjectType.ConsoleApplication:
						case ProjectType.WebApplication:
							publishProjects.Add(project);
							break;
						case ProjectType.Assembly:
						default:
							buildProjects.Add(project);
							break;
					}
				}

				if (buildProjects.Any())
				{
					var buildTag = new XElement(buildTemplate);
					buildTag.SetAttributeValue("Projects", string.Join(';', buildProjects.Select(p => p.FilePath)));
					buildTemplate.Parent?.Add(buildTag);
				}
			}

			buildTemplate.Remove();

			if (publishProjects.Any())
			{
				publishTemplate.SetAttributeValue("Projects", string.Join(';', publishProjects.Select(p => p.FilePath)));
			}
			else
			{
				publishTemplate.Remove();
			}

			if (testProjects.Any())
			{
				testTemplate.SetAttributeValue("Projects", string.Join(';', testProjects.Select(p => p.FilePath)));
			}
			else
			{
				testTemplate.Remove();
			}

			return rootProject;
		}
		
		private XElement GetBuildTag(XElement rootProject, string targetValue)
		{
			return rootProject.Descendants().First(e => e.Name.LocalName == _BuildTagName && e.Attributes().First(a => a.Name.LocalName == _BuildTargetsAttributeName).Value == targetValue);
		}

		private XElement BuildProjectTemplate()
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream($"{typeof(ProjectBuilder).Namespace}.Templates.build.proj"))
			using (var reader = new StreamReader(stream))
			{
				return XElement.Parse(reader.ReadToEnd());
			}
		}
	}
}
