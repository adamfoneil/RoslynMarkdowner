using JsonSettings;
using MarkdownViewer.App.Extensions;
using MarkdownViewer.App.Services;
using Microsoft.AspNetCore.Hosting;
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

		public override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            context.HttpContext.Response.ContentType = "text/plain";
            return base.OnPageHandlerExecutionAsync(context, next);            
        }

        public async Task OnGetAsync(string solution = null, string @namespace = null)
        {
			var metadata = (!string.IsNullOrEmpty(solution)) ?
				await _blobStorage.GetAsync<SolutionInfo>(User.Email(), solution) : 
				GetSolutionMetadata();
			
			Classes = (!string.IsNullOrEmpty(@namespace)) ? metadata.Classes.Where(ci => ci.Namespace.Equals(@namespace)) : metadata.Classes;

			CSMarkdown.OnlinePath = metadata.SourceFileBase();
		}

		private SolutionInfo GetSolutionMetadata()
        {
            string fileName = Path.Combine(_hosting.WebRootPath, "data", "SolutionMetadata.json");
            return JsonFile.Load<SolutionInfo>(fileName);
        }
    }
}