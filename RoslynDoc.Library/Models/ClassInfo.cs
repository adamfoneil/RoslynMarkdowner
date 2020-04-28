using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynDoc.Library.Models
{
	public class ClassInfo
	{
		public string Namespace { get; set; }
		public string Name { get; set; }
		public bool IsStatic { get; set; }
		public string AssemblyName { get; set; }

		/// <summary>
		///     XML summary comments
		/// </summary>
		public string Description { get; set; }

		public SourceLocation Location { get; set; }
		public ICollection<MethodInfo> Methods { get; set; }
		public ICollection<PropertyInfo> Properties { get; set; }

		public ClassDeclarationSyntax Node { get; set; }

		internal static ClassInfo FromPartials(IEnumerable<ClassInfo> grp)
		{
			var first = grp.First();

			return new ClassInfo()
			{
				Namespace = first.Namespace,
				Name = first.Name,
				IsStatic = first.IsStatic,
				AssemblyName = first.AssemblyName,
				Description = first.Description,
				Location = first.Location,
				Methods = grp.SelectMany(c => c.Methods).ToList(),
				Properties = grp.SelectMany(c => c.Properties).ToList()
			};
		}
	}
}