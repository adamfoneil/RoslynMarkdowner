using RoslynDocumentor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownViewer.ViewModels
{
	public class MarkdownIndexView
	{
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
			return (p.TypeLocation != null && !p.IsGeneric) ? $"{extension}{paramArray}[{p.OriginalTypeName}]({GetOnlineUrl(p.TypeLocation)}) {p.Name}" : $"{extension}{paramArray}{p.OriginalTypeName} {p.Name}";
		}
	}
}