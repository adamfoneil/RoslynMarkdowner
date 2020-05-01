using JsonSettings;
using MarkdownViewer.App.Classes;
using MarkdownViewer.App.Extensions;
using MarkdownViewer.App.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoslynDoc.Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarkdownViewer.App.Pages
{
    public class MarkdownModel : PageModel
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly BlobStorage _blobStorage;

        public MarkdownModel(IWebHostEnvironment hosting, BlobStorage blobStorage, CSharpMarkdownHelper csmd)
        {
            _hosting = hosting;
            _blobStorage = blobStorage;
            CSMarkdown = csmd;
        }

        public CSharpMarkdownHelper CSMarkdown { get; }

        public IEnumerable<ClassInfo> Classes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Solution { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Namespace { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AssemblyName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassName { get; set; }

        public override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            context.HttpContext.Response.ContentType = "text/plain";
            return base.OnPageHandlerExecutionAsync(context, next);
        }

        public async Task OnGetAsync()
        {
            var metadata = (!string.IsNullOrEmpty(Solution)) ?
                await _blobStorage.GetAsync<SolutionInfo>(User.Email(), Solution) :
                GetSolutionMetadata();

            var criteria = new Criteria<ClassInfo>();
            criteria.AddIf(!string.IsNullOrEmpty(Namespace), (ci) => ci.Namespace.Equals(Namespace));
            criteria.AddIf(!string.IsNullOrEmpty(AssemblyName), (ci) => ci.AssemblyName.Equals(AssemblyName));
            criteria.AddIf(!string.IsNullOrEmpty(ClassName), (ci) => ci.Name.Equals(ClassName));

            Classes = metadata.Classes.Where(ci => criteria.Invoke(ci));

            CSMarkdown.OnlinePath = metadata.SourceFileBase();
        }

        /// <summary>
        /// this is for compatibility with original implementation, I think, and is no longer needed
        /// </summary>        
		private SolutionInfo GetSolutionMetadata()
        {
            string fileName = Path.Combine(_hosting.WebRootPath, "data", "SolutionMetadata.json");
            return JsonFile.Load<SolutionInfo>(fileName);
        }
    }
}