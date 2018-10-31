using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDocumentor.Models;
using static RoslynDocumentor.Models.MethodInfo;


namespace RoslynDocumentor {

	public sealed class DocumentSyntaxAnalyzer {

		public List<ClassInfo> Analyze( SyntaxTree tree ) {

			var result = new List<ClassInfo>();

			var classNodes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Where( c => IsPublic( c.Modifiers ) ).ToList();
			foreach( var node in classNodes ) {
				result.Add( AnalyzeClass( node ) );
			}

			return result;

		}

		private static ClassInfo AnalyzeClass( ClassDeclarationSyntax node ) {

			var result = new ClassInfo();

			result.Name = GetName( node.Identifier );
			result.Description = GetSummary( node );

			result.Methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>()
				.Where( m => IsPublic( m.Modifiers ) )
				.Select( AnalyzeMethod )
				.ToList();

			result.Properties = node.DescendantNodes().OfType<PropertyDeclarationSyntax>()
				.Where( m => IsPublic( m.Modifiers ) )
				.Select( AnalyzeProperty )
				.ToList();

			result.Node = node;

			return result;
		}

		private static PropertyInfo AnalyzeProperty( PropertyDeclarationSyntax node ) {

			var result = new PropertyInfo();

			result.Name = GetName( node.Identifier );
			result.Description = GetSummary( node );
			result.OriginalTypeName = node.Type.ToString();
			result.Node = node;

			return result;
		}

		private static MethodInfo AnalyzeMethod( MethodDeclarationSyntax node ) {

			var result = new MethodInfo();

			result.Name = GetName( node.Identifier );
			result.Description = GetSummary( node );
			result.OriginalTypeName = node.ReturnType.ToString();
			result.Parameters = node.ParameterList.Parameters.Select( AnalyzeParameter ).ToList();
			result.Node = node;

			return result;

		}

		private static Parameter AnalyzeParameter( ParameterSyntax node ) {
			var result = new Parameter();
			result.OriginalTypeName = node.Type.ToString();
			result.Node = node;
			return result;
		}

		private static string GetSummary( CSharpSyntaxNode node ) {

			DocumentationCommentTriviaSyntax xmlTrivia = node.GetLeadingTrivia()
				.Select( i => i.GetStructure() )
				.OfType<DocumentationCommentTriviaSyntax>()
				.FirstOrDefault();


			//TODO: fix
			bool? isSummary = xmlTrivia?.ChildNodes()
				.OfType<XmlElementSyntax>()
				.Any( i => i.StartTag.Name.ToString().Equals( "summary" ) );

			return isSummary == true ? xmlTrivia.ToString() : null;

		}

		private static string GetName( SyntaxToken syntaxToken ) => syntaxToken.Text;

		private static bool IsPublic( SyntaxTokenList modifiers ) => modifiers.Any( m => m.Value.Equals( "public" ) );

	}

}