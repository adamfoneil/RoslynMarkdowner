using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDocumentor.Models {

	public class MethodInfo : IMemberInfo {

		public string Name { get; set; }
		public string Description { get; set; }
		public Location Location { get; set; }
		public bool IsStatic { get; set; }

		public string OriginalTypeName { get; set; }
		public string TypeName { get; set; }
		public Location TypeLocation { get; set; }

		public ICollection<Parameter> Parameters { get; set; }

		public class Parameter {

			public string Name { get; set; }
			public string DefaultValue { get; set; }
			public bool IsGeneric { get; set; }

			public string OriginalTypeName { get; set; }
			public string TypeName { get; set; }
			public Location TypeLocation { get; set; }

			public ParameterSyntax Node { get; set; }

		}

		public MethodDeclarationSyntax Node { get; set; }

	}

}
