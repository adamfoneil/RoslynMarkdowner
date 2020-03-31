using MarkdownViewer.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDoc.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkdownViewer.App.Pages
{
    public enum SourceType
    {
        LocalFile,
        LocalZip,
        DownloadZip
    }

    public class IndexModel : PageModel
    {
        public IndexModel()
        {            
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

        public SelectList VisualStudioInstanceSelect { get; set; }

        public List<string> Errors { get; set; }

        public void OnGet()
        {
            Initialize();
        }

        private SelectList GetVisualStudioInstances()
        {
            var instances = MSBuildLocator.QueryVisualStudioInstances();
            var items = instances.Select((instance, index) => new SelectListItem() { Value = index.ToString(), Text = instance.Name });
            return new SelectList(items, "Value", "Text");
        }

        public async Task OnPostAsync()
        {
            Initialize();

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
                var result = (await engine.Analyze(solution)).ToList();

            }
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

        private void Initialize()
        {
            AllowLocal = HttpContext.IsLocal();
            VisualStudioInstanceSelect = GetVisualStudioInstances();
        }
    }
}
