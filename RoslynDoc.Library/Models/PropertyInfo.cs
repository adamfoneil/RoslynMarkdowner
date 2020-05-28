using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDoc.Library.Services;
using System.Collections.Generic;

namespace RoslynDoc.Library.Models
{
	public sealed class PropertyInfo : IMemberInfo
	{
        public PropertyInfo()
        {
			References = new List<SourceLocation>();
        }

        public string Name { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public SourceLocation Location { get; set; }
		public bool IsPublic { get; set; }

		public bool IsStatic { get; set; }
		public bool CanWrite { get; set; }

		public string OriginalTypeName { get; set; }
		public string TypeName { get; set; }
		public SourceLocation TypeLocation { get; set; }
		public ICollection<SourceLocation> References { get; set; }

		public PropertyDeclarationSyntax Node { get; set; }		

		public string GetMarkdown(CSharpMarkdownHelper helper)
		{
			return $"- {helper.TypeUrlOrName(this)} [{this.Name}]({helper.GetOnlineUrl(this.Location)})";
		}
	}
}