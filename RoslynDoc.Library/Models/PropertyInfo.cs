using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDoc.Library.Models
{
	public class PropertyInfo : IMemberInfo
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public SourceLocation Location { get; set; }

		public bool IsStatic { get; set; }
		public bool CanWrite { get; set; }

		public string OriginalTypeName { get; set; }
		public string TypeName { get; set; }
		public SourceLocation TypeLocation { get; set; }

		public PropertyDeclarationSyntax Node { get; set; }
	}
}