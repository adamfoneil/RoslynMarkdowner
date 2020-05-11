using Microsoft.CodeAnalysis;
using RoslynDoc.Library.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.FindSymbols;
using static RoslynDoc.Library.Models.MethodInfo;
using Location = Microsoft.CodeAnalysis.Location;

namespace RoslynDoc.Library
{
	public sealed class DocumentSemanticAnalyzer
	{
		public async Task Analyze(Solution solution, SemanticModel model, List<ClassInfo> data)
		{
			foreach (var info in data)
				await AnalyzeClass(solution, model, info);
		}

		private static async Task AnalyzeClass(Solution solution, SemanticModel model, ClassInfo info)
		{
			ISymbol symbol = model.GetDeclaredSymbol(info.Node);
			info.IsStatic = symbol.IsStatic;
			info.Location = ToModelLocation(symbol.Locations, false);
			info.AssemblyName = symbol.ContainingAssembly.Name;

			foreach (var methodInfo in info.Methods)
				await AnalyzeMethod(solution, model, methodInfo);

			foreach (var propertyInfo in info.Properties)
                await AnalyzeProperty(solution, model, propertyInfo);

			info.Node = null;
		}

		private static async Task AnalyzeProperty(Solution solution, SemanticModel model, PropertyInfo info)
		{
			IPropertySymbol symbol = (IPropertySymbol)model.GetDeclaredSymbol(info.Node);
			info.Node = null;

			info.Location = ToModelLocation(symbol.Locations, false);
			info.IsStatic = symbol.IsStatic;
			info.CanWrite = symbol.IsReadOnly;
			info.TypeName = symbol.Type.Name;
			info.TypeLocation = ToModelLocation(symbol.Type.Locations);
			SetArrayTypeLocation(info, symbol.Type);

            await SetReferenceLocations(solution, info, symbol);
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

		private static async Task AnalyzeMethod(Solution solution, SemanticModel model, MethodInfo info)
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

            await SetReferenceLocations(solution, info, symbol);
        }

        private static async Task SetReferenceLocations(Solution solution, IMemberInfo info, ISymbol symbol )
        {
            var referenceLocations = (await SymbolFinder.FindReferencesAsync(symbol, solution))
                .SelectMany(x => x.Locations)
                .ToList();

            foreach (var referenceLocation in referenceLocations)
            {
                var reference = ToModelLocation(referenceLocation.Location, true);
                info.References.Add(reference);
            }
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

		private static SourceLocation ToModelLocation(ImmutableArray<Location> locations, bool isInSourceOnly = true) => 
            ToModelLocation( locations.FirstOrDefault(), isInSourceOnly );

        private static SourceLocation ToModelLocation(Location location, bool isInSourceOnly)
        {
            if (location == null)
                return null;

            if (isInSourceOnly && !location.IsInSource)
                return null;

            FileLinePositionSpan lineSpan = location.GetLineSpan();

            return new SourceLocation
            {
                LineNumber = lineSpan.StartLinePosition.Line + 1,
                Filename = lineSpan.Path
            };
        }
    }
}