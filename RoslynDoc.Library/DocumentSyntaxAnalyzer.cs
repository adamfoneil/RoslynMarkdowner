using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDoc.Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using static RoslynDoc.Library.Models.MethodInfo;

namespace RoslynDoc.Library
{
	public sealed class DocumentSyntaxAnalyzer
	{
		public List<ClassInfo> Analyze(SyntaxTree tree)
		{
			var result = new List<ClassInfo>();

			var classNodes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Where(c => IsPublic(c.Modifiers)).ToList();
			foreach (var node in classNodes)
			{
				result.Add(AnalyzeClass(node));
			}

			return result;
		}

		private static ClassInfo AnalyzeClass(ClassDeclarationSyntax node)
		{
			var result = new ClassInfo();

			result.Name = GetName(node.Identifier);
			result.Namespace = GetNamespace(node);
			result.Description = GetSummary(node);

			result.Methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>()
				.Where(m => IsPublic(m.Modifiers))
				.Select(AnalyzeMethod)
				.ToList();

			result.Properties = node.DescendantNodes().OfType<PropertyDeclarationSyntax>()
				.Where(m => IsPublic(m.Modifiers))
				.Select(AnalyzeProperty)
				.ToList();

			result.Node = node;

			return result;
		}

		private static string GetNamespace(ClassDeclarationSyntax node)
		{
			// see https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
			// used https://stackoverflow.com/a/23249021/2023653

			SyntaxNode parent = node.Parent;
			string result = null;

			while (true)
			{
				if (parent == null) break;
				if (parent.GetType() == typeof(NamespaceDeclarationSyntax))
				{
					result = (parent as NamespaceDeclarationSyntax).Name.GetText().ToString().Trim();
				}

				parent = parent.Parent;
			}

			return result;
		}

		private static PropertyInfo AnalyzeProperty(PropertyDeclarationSyntax node)
		{
			var result = new PropertyInfo();

			result.Name = GetName(node.Identifier);
			result.Description = GetSummary(node);
			result.Category = GetCategory(node);
			result.OriginalTypeName = node.Type.ToString();
			result.Node = node;

			return result;
		}

		private static MethodInfo AnalyzeMethod(MethodDeclarationSyntax node)
		{
			var result = new MethodInfo();

			result.Name = GetName(node.Identifier);
			result.Description = GetSummary(node);
			result.Category = GetCategory(node);
			result.OriginalTypeName = node.ReturnType.ToString();
			result.Parameters = node.ParameterList.Parameters.Select(AnalyzeParameter).ToList();
			result.Node = node;

			return result;
		}

		// help from https://stackoverflow.com/a/27675593/2023653
		private static string GetCategory(MemberDeclarationSyntax node)
		{			
			var attributes = node.AttributeLists.SelectMany(als => als.Attributes).ToArray();

			if (attributes.Any())
			{
				// find any [Category] attribute and return the argument from it,
				// for example [Category("helpers")] should return "helpers"
			}

			return null;
		}

		private static Parameter AnalyzeParameter(ParameterSyntax node)
		{
			var result = new Parameter();
			result.OriginalTypeName = node.Type.ToString();
			result.Node = node;
			return result;
		}

		private static string GetSummary(CSharpSyntaxNode node)
		{
			DocumentationCommentTriviaSyntax xmlTrivia = node.GetLeadingTrivia()
				.Select(i => i.GetStructure())
				.OfType<DocumentationCommentTriviaSyntax>()
				.FirstOrDefault();

			List<XmlElementSyntax> xmlComments = xmlTrivia?.ChildNodes()
				.OfType<XmlElementSyntax>()
				.ToList();

			if(xmlComments == null || !xmlComments.Any())
				return null;

			XmlElementSyntax elementSyntax = xmlComments.SkipWhile(x => !x.StartTag.Name.ToString().Equals("summary")).FirstOrDefault();
			if(elementSyntax == null)
				return null;

			var description = elementSyntax.Content.ToFullString();
			description = Regex.Replace( description, @"\t*///", string.Empty ).TrimStart( '\r', '\n', ' ' ).TrimEnd( '\r', '\n', ' ' );

			return description;

		}

		private static string GetName(SyntaxToken syntaxToken) => syntaxToken.Text;

		private static bool IsPublic(SyntaxTokenList modifiers) => modifiers.Any(m => m.Value.Equals("public"));
	}
}