using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDocumentor.Models {

	public class PropertyInfo : IMemberInfo {

		public string Name { get; set; }
		public string Description { get; set; }
		public Location Location { get; set; }

		public bool IsStatic { get; set; }
		public bool CanWrite { get; set; }

		public string OriginalTypeName { get; set; }
		public string TypeName { get; set; }
		public Location TypeLocation { get; set; }

		public PropertyDeclarationSyntax Node { get; set; }

	}

}
