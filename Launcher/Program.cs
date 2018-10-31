using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDocumentor;

namespace Launcher {

	internal class Program {

		//TODO:
		private const string SolutionPath = @"D:\Work\Upwork\2_RoslynAnalyzer\Test\CodeToAnalyze.sln";

		//private const string SolutionPath = @"D:\Work\Upwork\1_QueryTree\QueryTree.sln";


		private static async Task Main( string[] args ) {

			// Attempt to set the version of MSBuild.
			var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
			var instance = visualStudioInstances.Length == 1
				// If there is only one instance of MSBuild on this machine, set that as the one to use.
				? visualStudioInstances[0]
				// Handle selecting the version of MSBuild you want to use.
				: SelectVisualStudioInstance( visualStudioInstances );

			Console.WriteLine( $"Using MSBuild at '{instance.MSBuildPath}' to load projects." );

			// NOTE: Be sure to register an instance with the MSBuildLocator 
			//       before calling MSBuildWorkspace.Create()
			//       otherwise, MSBuildWorkspace won't MEF compose.
			MSBuildLocator.RegisterInstance( instance );

			using( var workspace = MSBuildWorkspace.Create() ) {
				// Print message for WorkspaceFailed event to help diagnosing project load failures.
				workspace.WorkspaceFailed += ( o, e ) => Console.WriteLine( e.Diagnostic.Message );

				Console.WriteLine( $"Loading solution '{SolutionPath}'" );

				// Attach progress reporter so we print projects as they are loaded.
				var solution = await workspace.OpenSolutionAsync( SolutionPath, new ConsoleProgressReporter() );
				Console.WriteLine( $"Finished loading solution '{SolutionPath}'" );

				//TODO:
				var engine = new SolutionAnalyzer();
				var result = engine.Analyze( solution ).Result;

			}

			Console.WriteLine( "done" );
			Console.ReadLine();


		}

		private static VisualStudioInstance SelectVisualStudioInstance( VisualStudioInstance[] visualStudioInstances ) {
			Console.WriteLine( "Multiple installs of MSBuild detected please select one:" );
			for( int i = 0; i < visualStudioInstances.Length; i++ ) {
				Console.WriteLine( $"Instance {i + 1}" );
				Console.WriteLine( $"    Name: {visualStudioInstances[i].Name}" );
				Console.WriteLine( $"    Version: {visualStudioInstances[i].Version}" );
				Console.WriteLine( $"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}" );
			}

			while( true ) {
				var userResponse = Console.ReadLine();
				if( int.TryParse( userResponse, out int instanceNumber ) &&
					instanceNumber > 0 &&
					instanceNumber <= visualStudioInstances.Length ) {
					return visualStudioInstances[instanceNumber - 1];
				}
				Console.WriteLine( "Input not accepted, try again." );
			}
		}

		private class ConsoleProgressReporter : IProgress<ProjectLoadProgress> {
			public void Report( ProjectLoadProgress loadProgress ) {
				var projectDisplay = Path.GetFileName( loadProgress.FilePath );
				if( loadProgress.TargetFramework != null ) {
					projectDisplay += $" ({loadProgress.TargetFramework})";
				}

				Console.WriteLine( $"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}" );
			}
		}

	}
}
