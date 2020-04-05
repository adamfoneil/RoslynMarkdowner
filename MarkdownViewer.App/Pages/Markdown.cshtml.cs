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

        public MarkdownModel(IWebHostEnvironment hosting, BlobStorage blobStorage)
        {
            _hosting = hosting;
			_blobStorage = blobStorage;
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

		public string GetOnlineUrl(SourceLocation location)
		{
			return OnlinePath + location.Filename.Replace("\\", "/") + "#L" + location.LineNumber;
		}

		public string TypeUrlOrName(IMemberInfo member)
		{
			return (member.TypeLocation != null) ?
				$"[{member.TypeName}]({GetOnlineUrl(member.TypeLocation)})" :
				member.TypeName;
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
			string optionalStart = (p.IsOptional) ? "[ " : string.Empty;
			string optionalEnd = (p.IsOptional) ? " ]" : string.Empty;

			string result = (p.TypeLocation != null && !p.IsGeneric) ?
				$"{optionalStart}{extension}{paramArray}[{p.OriginalTypeName}]({GetOnlineUrl(p.TypeLocation)}) {p.Name}{optionalEnd}" :
				$"{optionalStart}{extension}{paramArray}{p.OriginalTypeName} {p.Name}{optionalEnd}";

			return result;
		}

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

			OnlinePath = metadata.SourceFileBase();
		}

		private SolutionInfo GetSolutionMetadata()
        {
            string fileName = Path.Combine(_hosting.WebRootPath, "data", "SolutionMetadata.json");
            return JsonFile.Load<SolutionInfo>(fileName);
        }
    }
}