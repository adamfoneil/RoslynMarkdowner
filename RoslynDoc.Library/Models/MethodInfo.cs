﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDoc.Library.Services;
using System.Collections.Generic;
using System.Linq;

namespace RoslynDoc.Library.Models
{
	public sealed class MethodInfo : IMemberInfo
	{
        public MethodInfo()
        {
			References = new List<SourceLocation>();
        }

        public string Name { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public SourceLocation Location { get; set; }
		public bool IsStatic { get; set; }
		public bool IsPublic { get; set; }

		public string OriginalTypeName { get; set; }
		public string TypeName { get; set; }
		public SourceLocation TypeLocation { get; set; }
		public ICollection<SourceLocation> References { get; set; }

		public ICollection<Parameter> Parameters { get; set; }

		public bool HasGenericArguments()
		{
			return Parameters?.Any(p => p.IsGeneric) ?? false;
		}

		/// <summary>
		/// Returns concatenated list of generic type arguments that should be displayed with a method name
		/// </summary>		
		public string GetGenericArguments()
		{
			return string.Join(", ", Parameters.Where(p => p.IsGeneric).Select(p => p.OriginalTypeName));
		}

		public string GetMarkdown(CSharpMarkdownHelper helper)
		{
			return $"- {helper.TypeUrlOrName(this)} [{Name}]({helper.GetOnlineUrl(Location)}){helper.GetGenericArguments(this)}\r\n {helper.GetMethodSignature(this)}";
		}

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

			public bool IsOptional { get; set; }

			public string OriginalTypeName { get; set; }
			public string TypeName { get; set; }
			public SourceLocation TypeLocation { get; set; }

			public ParameterSyntax Node { get; set; }
		}

		public MethodDeclarationSyntax Node { get; set; }        
    }
}