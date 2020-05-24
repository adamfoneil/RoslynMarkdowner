using RoslynDoc.Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SolutionInfo = RoslynDoc.Library.Models.SolutionInfo;

namespace RoslynDoc.Library.Services
{
    /// <summary>
    /// rebuilds .md files in a repo from .rzm files.
    /// rzm is ordinary markdown except that it uses links like [link text](#identifier) which are C# identifiers that are resolved GitHub to line number links.
    /// "rzm" being short for "Roslyn Markdowner"    
    /// </summary>
    public class WikiBuilder
    {
        public WikiBuilder()
        {            
            Errors = new List<string>();
        }
        
        public List<string> Errors { get; }

        public static bool HasWikiRepository(string repoPath, out string wikiRepo)
        {
            wikiRepo = $"{repoPath}.wiki";
            return Directory.Exists(wikiRepo);
        }

        public async Task BuildAsync(string repoPath, SolutionInfo solutionInfo, CSharpMarkdownHelper markdownHelper, bool push = true)
        {
            if (BuildInner(repoPath, solutionInfo, markdownHelper) && push) await PushAsync(repoPath);

            if (HasWikiRepository(repoPath, out string wikiRepo))
            {
                if (BuildInner(wikiRepo, solutionInfo, markdownHelper) && push) await PushAsync(wikiRepo);
            }
        }

        /// <summary>
        /// compiles .md from .rzm source files. If any changes, they are committed, and returns true.        
        /// returns false if any errors (unrecognized identifiers) or no changes
        /// </summary>        
        private bool BuildInner(string repoPath, SolutionInfo solutionInfo, CSharpMarkdownHelper markdownHelper)
        {
            Errors.Clear();

            var rzmFiles = GetRzmFiles(repoPath);

            foreach (var file in rzmFiles)
            {
                string content = File.ReadAllText(file.Key);
                var identifiers = readIdentifiers(content, file.Key);

                var sb = new StringBuilder(content);
                foreach (var id in identifiers)
                {
                    if (findMember(id.Name, out SourceLocation location))
                    {                        
                        sb.Replace(id.Token, $"({markdownHelper.GetOnlineUrl(location)})");
                    }
                    else
                    {
                        Errors.Add($"Identifier at index {id.Match.Index} not found: {id.Name}");
                    }
                }

                File.WriteAllText(file.Value, sb.ToString());
            }

            return !Errors.Any();

            // tested via https://regexr.com/557un
            IEnumerable<IdentifierLink> readIdentifiers(string content, string fileName)
            {                
                var links = Regex.Matches(content, @"\[([^\]]+)\]\(#([^\)]+)\)").OfType<Match>();
                return links.Select(m =>
                {
                    var token = Regex.Match(m.Value, @"\(#.+\)").Value;
                    return new IdentifierLink()
                    {
                        Match = m,
                        Token = token,
                        Name = token.Substring(2, token.Length - 3)
                    };
                });
            }

            bool findMember(string name, out SourceLocation location)
            {
                var locations = new Dictionary<string, SourceLocation>();

                addFirst(locations, solutionInfo.Classes.ToLookup(ci => ci.Name, ci => ci.Location));
                addFirst(locations, solutionInfo.Classes.ToLookup(ci => $"{ci.Namespace}.{ci.Name}", ci => ci.Location));
                addFirst(locations, solutionInfo.Classes.SelectMany(ci => ci.Properties, (ci, p) => new { Class = ci.Name, Member = p }).ToLookup(p => $"{p.Class}.{p.Member.Name}", p => p.Member.Location));
                addFirst(locations, solutionInfo.Classes.SelectMany(ci => ci.Methods, (ci, m) => new { Class = ci.Name, Member = m }).ToLookup(m => $"{m.Class}.{m.Member.Name}", m => m.Member.Location));

                if (locations.ContainsKey(name))
                {
                    location = locations[name];
                    return true;
                }

                location = null;
                return false;
            }

            // because of partial classes, class names aren't necessarily unique, 
            // so I simply take the first source location associated with an identifier            
            void addFirst(Dictionary<string, SourceLocation> locations, ILookup<string, SourceLocation> lookups)
            {
                foreach (var grp in lookups) locations.Add(grp.Key, grp.First());
            }
        }        

        /// <summary>
        /// pushes .md changes to its remote
        /// </summary>        
        private static async Task PushAsync(string repoPath)
        {

        }

        private static Dictionary<string, string> GetRzmFiles(string path)
        {
            var rzmFiles = Directory.GetFiles(path, "*.rzm", SearchOption.AllDirectories);
            var filePairs = rzmFiles.Select(fileName =>
            new
            {
                Source = fileName,
                Target = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".md")
            });

            return filePairs.ToDictionary(item => item.Source, item => item.Target);
        }

        /// <summary>
        /// describes a markdown link representing a C# identifier, takes the form
        /// [link text](#identifier)
        /// </summary>
        private class IdentifierLink
        { 
            public Match Match { get; set; }

            /// <summary>
            /// the part of the link in parentheses
            /// </summary>
            public string Token { get; set; }

            /// <summary>
            /// part of the link after the # sign within parentheses
            /// </summary>
            public string Name { get; set; }
        }
    }
}
