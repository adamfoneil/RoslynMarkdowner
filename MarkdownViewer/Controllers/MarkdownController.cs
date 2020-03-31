using JsonSettings;
using MarkdownViewer.ViewModels;
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

			var vm = new MarkdownIndexView();
			vm.Classes = (!string.IsNullOrEmpty(@namespace)) ? metadata.Where(ci => ci.Namespace.Equals(@namespace)) : metadata;
			//vm.OnlinePath = "https://github.com/adamosoftware/Postulate/blob/master/Postulate.Base/";
			vm.OnlinePath = "https://github.com/adamosoftware/Dapper.CX/blob/master/Dapper.CX.Base/";
			return View(vm);
        }

		private IEnumerable<ClassInfo> GetSolutionMetadata()
		{
			string fileName = Server.MapPath("~/App_Data/SolutionMetadata.json");
			return JsonFile.Load<IEnumerable<ClassInfo>>(fileName);
		}
    }
}