using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarkdownViewer.App.Extensions;
using MarkdownViewer.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDoc.Library;
using RoslynDoc.Library.Models;

namespace MarkdownViewer.App.Pages
{
    public enum SourceType
    {
        LocalFile,
        LocalZip,
        DownloadZip
    }

    [Authorize]
    public class AnalyzeModel : PageModel
    {
        private readonly BlobStorage _blobStorage;

        public AnalyzeModel(BlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public bool AllowLocal { get; set; }

        [BindProperty]
        public SourceType SourceType { get; set; }

        [BindProperty]
        public int VSInstance { get; set; }

        [BindProperty]
        public string LocalFile { get; set; }

        [BindProperty]
        public string LocalZip { get; set; }

        [BindProperty]
        public string DownloadZip { get; set; }

        [BindProperty]
        public string GitHubRepoUrl { get; set; }

        [BindProperty]
        public string BranchName { get; set; }

        public SelectList VisualStudioInstanceSelect { get; set; }

        public List<string> Errors { get; set; }

        public IEnumerable<CloudBlockBlob> MySolutions { get; set; }

        public async Task OnGetAsync()
        {
            await InitializeAsync();
        }

        private SelectList GetVisualStudioInstances()
        {
            var instances = MSBuildLocator.QueryVisualStudioInstances();
            var items = instances.Select((instance, index) => new SelectListItem() { Value = index.ToString(), Text = instance.Name });
            return new SelectList(items, "Value", "Text");
        }

        public async Task OnPostAsync()
        {            
            string solutionPath =
                (SourceType == SourceType.LocalFile) ? LocalFile :
                (SourceType == SourceType.LocalZip) ? ExtractSolution(LocalZip) :
                (SourceType == SourceType.DownloadZip) ? DownloadSolution(DownloadZip) :
                throw new Exception($"Unknown source type {SourceType}");

            var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = instances[VSInstance];            

            MSBuildLocator.RegisterInstance(instance);
            using (var ws = MSBuildWorkspace.Create())
            {
                Errors = new List<string>();
                ws.WorkspaceFailed += (o, e) => Errors.Add(e.Diagnostic.Message);

                var solution = await ws.OpenSolutionAsync(solutionPath);
                var engine = new SolutionAnalyzer();
                var classes = (await engine.Analyze(solution, solutionPath)).ToList();
                var output = new SolutionInfo()
                {
                    Classes = classes,
                    RepoUrl = GitHubRepoUrl,
                    BranchName = BranchName,
                    Timestamp = DateTime.UtcNow
                };

                await _blobStorage.SaveAsync(User.Email(), output, Path.GetFileNameWithoutExtension(solutionPath) + ".json");
            }
            MSBuildLocator.Unregister();

            await InitializeAsync();
        }        

        private string DownloadSolution(string downloadZip)
        {
            throw new NotImplementedException();
        }

        private string ExtractSolution(string localZip)
        {
            throw new NotImplementedException();
        }

        private Task<string> GetSolutionAsync()
        {
            throw new NotImplementedException();
        }

        public static string SourceTypeName(SourceType sourceType)
        {
            return Enum.GetName(typeof(SourceType), sourceType);
        }

        private async Task InitializeAsync()
        {
            AllowLocal = HttpContext.IsLocal();
            VisualStudioInstanceSelect = GetVisualStudioInstances();
            MySolutions = await _blobStorage.ListContentAsync(User.Email());
        }
    }
}