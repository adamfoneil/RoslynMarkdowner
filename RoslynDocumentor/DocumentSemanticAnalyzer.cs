using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynDocumentor.Models;
using static RoslynDocumentor.Models.MethodInfo;
using Location = Microsoft.CodeAnalysis.Location;


namespace RoslynDocumentor {

	public sealed class DocumentSemanticAnalyzer {

		public void Analyze( SemanticModel model, List<ClassInfo> data ) {

			foreach( var info in data ) 
				AnalyzeClass( model, info );

		}

		private static void AnalyzeClass( SemanticModel model, ClassInfo info ) {

			ISymbol symbol = model.GetDeclaredSymbol( info.Node );
			info.IsStatic = symbol.IsStatic;
			info.Location = ToModelLocation( symbol.Locations, false );

			foreach( var methodInfo in info.Methods ) 
				AnalyzeMethod( model, methodInfo );
			
			foreach( var propertyInfo in info.Properties ) 
				AnalyzeProperty( model, propertyInfo );

			info.Node = null;
		}

		private static void AnalyzeProperty( SemanticModel model, PropertyInfo info ) {

			IPropertySymbol symbol = (IPropertySymbol)model.GetDeclaredSymbol( info.Node );
			info.Node = null;

			info.Location = ToModelLocation( symbol.Locations, false );
			info.IsStatic = symbol.IsStatic;
			info.CanWrite = symbol.IsReadOnly;
			info.TypeName = symbol.Type.Name;
			info.TypeLocation = ToModelLocation( symbol.Type.Locations );
		}

		private static void AnalyzeMethod( SemanticModel model, MethodInfo info ) {

			IMethodSymbol symbol = (IMethodSymbol)model.GetDeclaredSymbol( info.Node );
			info.Node = null;

			info.Location = ToModelLocation( symbol.Locations, false );
			info.IsStatic = symbol.IsStatic;
			info.TypeName = symbol.ReturnType.Name;
			info.TypeLocation = ToModelLocation( symbol.ReturnType.Locations );

			foreach( var parameterInfo in info.Parameters )
				AnalyzeParameter( model, parameterInfo );

		}

		private static void AnalyzeParameter( SemanticModel model, Parameter info ) {

			IParameterSymbol symbol = (IParameterSymbol)model.GetDeclaredSymbol( info.Node );
			info.Node = null;

			info.Name = symbol.Name;
			info.TypeName = symbol.Type.Name;
			info.TypeLocation = ToModelLocation( symbol.Type.Locations );
			info.IsGeneric = symbol.Type.Kind == SymbolKind.TypeParameter;

			if( symbol.HasExplicitDefaultValue )
				info.DefaultValue = symbol.ExplicitDefaultValue.ToString();

		}

		private static Models.Location ToModelLocation( ImmutableArray<Location> locations, bool isInSourceOnly = true ) {

			var location = locations.FirstOrDefault();

			if( location == null )
				return null;

			if( isInSourceOnly && !location.IsInSource )
				return null;

			FileLinePositionSpan lineSpan = location.GetLineSpan();

			return new Models.Location {
				LineNumber = lineSpan.StartLinePosition.Line + 1,
				SourceFile = lineSpan.Path
			};

		}

	}

}