using Microsoft.CodeAnalysis;
using RoslynDoc.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoslynDoc.Library
{
	public sealed class SolutionAnalyzer
	{
		private readonly DocumentSyntaxAnalyzer _syntaxAnalyzer = new DocumentSyntaxAnalyzer();
		private readonly DocumentSemanticAnalyzer _semanticAnalyzer = new DocumentSemanticAnalyzer();

		public async Task<IEnumerable<ClassInfo>> Analyze(Solution solution)
		{
			var result = new List<ClassInfo>();

			foreach (Document doc in solution.Projects.SelectMany(p => p.Documents))
			{
				// Syntax Info
				SyntaxTree tree = await doc.GetSyntaxTreeAsync();
				List<ClassInfo> classInfos = _syntaxAnalyzer.Analyze(tree);

				// Semantic Info
				SemanticModel model = await doc.GetSemanticModelAsync();
				_semanticAnalyzer.Analyze(model, classInfos);

				result.AddRange(classInfos);
			}

			return result;
		}
	}
}