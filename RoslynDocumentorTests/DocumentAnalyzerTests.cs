using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynDocumentor;
using Xunit;

namespace RoslynDocumentorTests {

	public sealed class DocumentAnalyzerTests {

		private readonly DocumentSyntaxAnalyzer m_sut = new DocumentSyntaxAnalyzer();

		[Fact]
		public void EmptyStaticClass_Test() {

			SyntaxTree tree = CSharpSyntaxTree.ParseText( CodeSamples.EmptyStaticClass );
			var result = m_sut.Analyze( tree );

			Assert.Single( result );
			var classInfo = result[0];
			Assert.Equal( "StaticClass", classInfo.Name );
			Assert.Null( classInfo.Description );
			Assert.Empty( classInfo.Methods );
			Assert.Empty( classInfo.Properties );

		}

		[Fact]
		public void Sample1_Test() {

			SyntaxTree tree = CSharpSyntaxTree.ParseText( CodeSamples.Sample1 );
			var result = m_sut.Analyze( tree );

			// class
			Assert.Single( result );
			var classInfo = result[0];
			Assert.Equal( "Nesting", classInfo.Name );
			Assert.Equal( " <summary>\r\n\t/// My Solution\r\n\t/// </summary>\r\n", classInfo.Description );

			// methods
			Assert.Equal( 4, classInfo.Methods.Count );
			var method1 = classInfo.Methods.First();
			var method2 = classInfo.Methods.Skip( 1 ).First();
			var method3 = classInfo.Methods.Skip( 2 ).First();
			var method4 = classInfo.Methods.Skip( 3 ).First();

			Assert.Equal( "solution1", method1.Name );
			Assert.NotNull( method1.Description );

			Assert.Equal( "solution2", method2.Name );
			Assert.Null( method2.Description );

			Assert.Equal( "solution3", method3.Name );
			Assert.Null( method3.Description );

			Assert.Equal( "Test", method4.Name );
			Assert.Null( method4.Description );

			// properties

			Assert.Equal( 2, classInfo.Properties.Count );
			var property1 = classInfo.Properties.First();
			var property2 = classInfo.Properties.Skip( 1 ).First();

			Assert.Equal( "AutoProperty", property1.Name );
			Assert.Null( property1.Description );

			Assert.Equal( "ReadOnlyProperty", property2.Name );
			Assert.NotNull( property2.Description );

		}

	}

}
