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
			return location.SourceFile.Replace(LocalPath, OnlinePath).Replace("\\", "/") + "#L" + location.LineNumber;
		}

		public string GetMethodSignature(MethodInfo method)
		{
			return "(" + string.Join(", ", method.Parameters.Select(p => ArgText(p))) + ")";
		}

		private string ArgText(MethodInfo.Parameter p)
		{
			return (p.TypeLocation != null) ? $"[{p.OriginalTypeName}]({GetOnlineUrl(p.TypeLocation)}) {p.Name}" : $"{p.OriginalTypeName} {p.Name}";
		}
	}
}