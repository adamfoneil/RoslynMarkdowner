using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynDocumentor.Models;


namespace RoslynDocumentor {

	public sealed class SolutionAnalyzer {

		private readonly DocumentSyntaxAnalyzer m_syntaxAnalyzer = new DocumentSyntaxAnalyzer();
		private readonly DocumentSemanticAnalyzer m_semanticAnalyzer = new DocumentSemanticAnalyzer();

		public async Task<IEnumerable<ClassInfo>> Analyze( Solution solution ) {

			var result = new List<ClassInfo>();

			foreach( Document doc in solution.Projects.SelectMany( p => p.Documents ) ) {

				// Syntax Info
				SyntaxTree tree = await doc.GetSyntaxTreeAsync();
				List<ClassInfo> classInfos = m_syntaxAnalyzer.Analyze( tree );

				// Semantic Info
				SemanticModel model = await doc.GetSemanticModelAsync();
				m_semanticAnalyzer.Analyze( model, classInfos );

				result.AddRange( classInfos );

			}

			return result;
		}

	}

}