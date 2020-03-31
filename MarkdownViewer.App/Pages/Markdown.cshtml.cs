using JsonSettings;
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

        public MarkdownModel(IWebHostEnvironment hosting)
        {
            _hosting = hosting;
        }

        public IEnumerable<ClassInfo> Classes { get; set; }

		/// <summary>
		/// Base folder in local file system (reported by ClassInfo.Location.SourceFile)
		/// </summary>
		public string LocalPath { get; set; }

		/// <summary>
		/// GitHub base path
		/// </summary>
		public string OnlinePath { get; set; }

		public string GetOnlineUrl(Location location)
		{
			return OnlinePath + location.SourceFile.Replace("\\", "/") + "#L" + location.LineNumber;
		}

		public string GetMethodSignature(MethodInfo method)
		{
			return "(" + string.Join(", ", method.Parameters.Select(p => ArgText(p))) + ")";
		}

		public string GetGenericArguments(MethodInfo method)
		{
			return (method.HasGenericArguments()) ? $"<{method.GetGenericArguments()}>" : string.Empty;
		}

		private string ArgText(MethodInfo.Parameter p)
		{
			string extension = (p.IsExtension) ? "this " : string.Empty;
			string paramArray = (p.IsParams) ? "params " : string.Empty;
			string result = (p.TypeLocation != null && !p.IsGeneric) ?
				$"{extension}{paramArray}[{p.OriginalTypeName}]({GetOnlineUrl(p.TypeLocation)}) {p.Name}" :
				$"{extension}{paramArray}{p.OriginalTypeName} {p.Name}";

			/* indicate optionality, doesn't work -- markdown is incorrect and the <unknown> value isn't right for expressing optionality

			if ((p.DefaultValue?.Equals("<unknown>") ?? false) && !p.IsParams)
			{
				result = "[[" + result + "]]";
			}*/

			return result;
		}

		public override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            context.HttpContext.Response.ContentType = "text/plain";
            return base.OnPageHandlerExecutionAsync(context, next);            
        }

        public void OnGet(string @namespace = null)
        {
			var metadata = GetSolutionMetadata();
			
			Classes = (!string.IsNullOrEmpty(@namespace)) ? metadata.Classes.Where(ci => ci.Namespace.Equals(@namespace)) : metadata.Classes;
			//OnlinePath = "https://github.com/adamosoftware/Dapper.CX/blob/master/";
			metadata.RepoUrl = "https://github.com/adamosoftware/Dapper.CX";
			OnlinePath = metadata.SourceFileBase();
		}

		private SolutionInfo GetSolutionMetadata()
        {
            string fileName = Path.Combine(_hosting.WebRootPath, "data", "SolutionMetadata.json");
            return JsonFile.Load<SolutionInfo>(fileName);
        }
    }
}