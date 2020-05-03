using Microsoft.CodeAnalysis;
using RoslynDoc.Library.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static RoslynDoc.Library.Models.MethodInfo;
using Location = Microsoft.CodeAnalysis.Location;

namespace RoslynDoc.Library
{
	public sealed class DocumentSemanticAnalyzer
	{
		public void Analyze(SemanticModel model, List<ClassInfo> data)
		{
			foreach (var info in data)
				AnalyzeClass(model, info);
		}

		private static void AnalyzeClass(SemanticModel model, ClassInfo info)
		{
			ISymbol symbol = model.GetDeclaredSymbol(info.Node);
			info.IsStatic = symbol.IsStatic;
			info.Location = ToModelLocation(symbol.Locations, false);
			info.AssemblyName = symbol.ContainingAssembly.Name;

			foreach (var methodInfo in info.Methods)
				AnalyzeMethod(model, methodInfo);

			foreach (var propertyInfo in info.Properties)
				AnalyzeProperty(model, propertyInfo);

			info.Node = null;
		}

		private static void AnalyzeProperty(SemanticModel model, PropertyInfo info)
		{
			IPropertySymbol symbol = (IPropertySymbol)model.GetDeclaredSymbol(info.Node);
			info.Node = null;

			info.Location = ToModelLocation(symbol.Locations, false);
			info.IsStatic = symbol.IsStatic;
			info.CanWrite = symbol.IsReadOnly;
			info.TypeName = symbol.Type.Name;
			info.TypeLocation = ToModelLocation(symbol.Type.Locations);
			SetArrayTypeLocation(info, symbol.Type);
		}

		private static void SetArrayTypeLocation(IMemberInfo info, ITypeSymbol typeSymbol)
		{
			if (info.TypeLocation == null)
			{
				var elementType = (typeSymbol as IArrayTypeSymbol)?.ElementType;
				if (elementType != null) info.TypeLocation = ToModelLocation(elementType.Locations);
				if (string.IsNullOrEmpty(info.TypeName) && info.TypeLocation != null) info.TypeName = elementType.Name + "[]";
			}
		}

		private static void AnalyzeMethod(SemanticModel model, MethodInfo info)
		{
			IMethodSymbol symbol = (IMethodSymbol)model.GetDeclaredSymbol(info.Node);
			info.Node = null;

			info.Location = ToModelLocation(symbol.Locations, false);
			info.IsStatic = symbol.IsStatic;
			info.TypeName = symbol.ReturnType.Name;
			info.TypeLocation = ToModelLocation(symbol.ReturnType.Locations);

			SetArrayTypeLocation(info, symbol.ReturnType);

			foreach (var parameterInfo in info.Parameters)
				AnalyzeParameter(model, parameterInfo);

			if(symbol.IsExtensionMethod) 
				info.Parameters.First().IsExtension = true;
		}

		private static void AnalyzeParameter(SemanticModel model, Parameter info)
		{
			IParameterSymbol symbol = (IParameterSymbol)model.GetDeclaredSymbol(info.Node);
			info.Node = null;

			info.Name = symbol.Name;
			info.TypeName = symbol.Type.Name;
			info.TypeLocation = ToModelLocation(symbol.Type.Locations);
			info.IsGeneric = symbol.Type.Kind == SymbolKind.TypeParameter;
			info.IsParams = symbol.IsParams;
			info.IsOptional = symbol.IsOptional;

			if (symbol.HasExplicitDefaultValue)
				info.DefaultValue = symbol?.ExplicitDefaultValue?.ToString() ?? "<unknown>";
		}

		private static Models.SourceLocation ToModelLocation(ImmutableArray<Location> locations, bool isInSourceOnly = true)
		{
			var location = locations.FirstOrDefault();

			if (location == null)
				return null;

			if (isInSourceOnly && !location.IsInSource)
				return null;

			FileLinePositionSpan lineSpan = location.GetLineSpan();

			return new Models.SourceLocation
			{
				LineNumber = lineSpan.StartLinePosition.Line + 1,
				Filename = lineSpan.Path
			};
		}
	}
}