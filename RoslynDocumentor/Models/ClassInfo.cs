﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace RoslynDocumentor.Models
{
	public class ClassInfo
	{
		public string Namespace { get; set; }
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