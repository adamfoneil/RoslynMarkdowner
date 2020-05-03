using RoslynDoc.Library.Services;

namespace RoslynDoc.Library.Models
{
	public interface IMemberInfo
	{
		string Name { get; }

		/// <summary>
		/// Name of a method or property's return type (BCL types should not have namespace, but solution-defined types should have namespace)
		/// </summary>
		string TypeName { get; set; }
		string OriginalTypeName { get; set; }

		/// <summary>
		/// If the TypeName is defined in this solution, this is its location (otherwise null)
		/// </summary>
		SourceLocation TypeLocation { get; set; }

		bool IsStatic { get; set; }
		string Description { get; set; }
		string Category { get; set; }
		SourceLocation Location { get; set; }

		string GetMarkdown(CSharpMarkdownHelper helper);
	}
}