using JsonSettings;
using RoslynDocumentor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkdownViewer.Controllers
{
    public class MarkdownController : Controller
    {
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			Response.ContentType = "text/plain";
		}

		// GET: Markdown
		public ActionResult Index(string @namespace = null)
        {
			var metadata = GetSolutionMetadata();
			var display = (!string.IsNullOrEmpty(@namespace)) ? metadata.Where(ci => ci.Namespace.Equals(@namespace)) : metadata;
            return View(display);
        }

		private IEnumerable<ClassInfo> GetSolutionMetadata()
		{
			string fileName = Server.MapPath("~/App_Data/SolutionMetadata.json");
			return JsonFile.Load<IEnumerable<ClassInfo>>(fileName);
		}
    }
}