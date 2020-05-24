using RoslynDoc.Library.Models;
using System.IO;
using System.Threading.Tasks;

namespace RoslynDoc.Library.Services
{
    /// <summary>
    /// rebuilds .md files in a repo from .rzm files.
    /// rzm is ordinary markdown except that it uses links like [link text](#identifier) which are C# identifiers that are resolved GitHub to line number links.
    /// "rzm" being short for "Roslyn Markdowner"    
    /// </summary>
    public class WikiBuilder
    {
        public WikiBuilder(string repositoryPath)
        {
            RepositoryPath = repositoryPath;            
        }

        public string RepositoryPath { get; }
        public Models.SolutionInfo SolutionInfo { get; }

        public static bool HasWikiRepository(string repoPath, out string wikiRepo)
        {
            wikiRepo = Path.Combine(repoPath, ".wiki");
            return Directory.Exists(wikiRepo);
        }

        public async Task BuildAsync(SolutionInfo solutionInfo, bool push = true)
        {
            if (BuildInner(solutionInfo, RepositoryPath) && push) await PushInnerAsync(RepositoryPath);

            if (HasWikiRepository(RepositoryPath, out string wikiRepo))
            {
                if (BuildInner(solutionInfo, wikiRepo) && push) await PushInnerAsync(wikiRepo);
            }
        }

        /// <summary>
        /// compiles .md from .rzm source files. If any changes, they are committed, and returns true.        
        /// returns false if any errors (unrecognized identifiers) or no changes
        /// </summary>        
        private bool BuildInner(SolutionInfo solutionInfo, string path)
        {
            var rzmFiles = Directory.GetFiles(path, "*.rzm", SearchOption.AllDirectories);
            foreach (var file in rzmFiles)
            {

            }

            return false;
        }

        /// <summary>
        /// pushes .md changes to its remote
        /// </summary>        
        public async Task PushAsync()
        {
            await PushInnerAsync(RepositoryPath);
        }

        private static async Task PushInnerAsync(string repoPath)
        {

        }
    }
}
