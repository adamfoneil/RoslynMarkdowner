using JsonSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MarkdownViewer.App.Pages
{
    public class MarkdownModel : PageModel
    {
        private readonly IWebHostEnvironment _hosting;

        public MarkdownModel(IWebHostEnvironment hosting)
        {
            _hosting = hosting;
        }

        public IEnumerable<ClassInfo> Classes { get; set; }

        public override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            context.HttpContext.Response.ContentType = "text/plain";
            return base.OnPageHandlerExecutionAsync(context, next);            
        }

        public async Task OnGetAsync()
        {

        }

        private IEnumerable<ClassInfo> GetSolutionMetadata()
        {
            string fileName = Path.Combine(_hosting.WebRootPath, "App_Data", "SolutionMetadata.json");
            return JsonFile.Load<IEnumerable<ClassInfo>>(fileName);
        }

    }
}