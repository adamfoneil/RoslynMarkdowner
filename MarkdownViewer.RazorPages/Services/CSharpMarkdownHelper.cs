using RoslynDoc.Library.Models;
using System.Linq;

namespace MarkdownViewer.App.Services
{
	public class CSharpMarkdownHelper
    {
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
	}
}
