using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace RoslynDocumentor.Models
{
	public class MethodInfo : IMemberInfo
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Location Location { get; set; }
		public bool IsStatic { get; set; }

		public string OriginalTypeName { get; set; }
		public string TypeName { get; set; }
		public Location TypeLocation { get; set; }

		public ICollection<Parameter> Parameters { get; set; }

		public class Parameter
		{
			public string Name { get; set; }
			public string DefaultValue { get; set; }
			public bool IsGeneric { get; set; }

			/// <summary>
			/// If true, means that the argument is an extension of TypeName/OriginalType
			/// </summary>
			public bool IsExtension { get; set; }

			/// <summary>
			/// If true, means that argument accepts variable number of values
			/// </summary>
			public bool IsParams { get; set; }

			public string OriginalTypeName { get; set; }
			public string TypeName { get; set; }
			public Location TypeLocation { get; set; }

			public ParameterSyntax Node { get; set; }
		}

		public MethodDeclarationSyntax Node { get; set; }
	}
}