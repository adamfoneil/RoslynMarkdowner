using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDocumentor.Models {

	public class ClassInfo {

		public string Name { get; set; }
		public bool IsStatic { get; set; }

		/// <summary>
		///     XML summary comments
		/// </summary>
		public string Description { get; set; }

		public Location Location { get; set; }
		public ICollection<MethodInfo> Methods { get; set; }
		public ICollection<PropertyInfo> Properties { get; set; }

		public ClassDeclarationSyntax Node { get; set; }

	}

}
