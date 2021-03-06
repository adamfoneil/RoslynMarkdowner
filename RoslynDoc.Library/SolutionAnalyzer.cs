﻿using Microsoft.CodeAnalysis;
using RoslynDoc.Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoslynDoc.Library
{
	public sealed class SolutionAnalyzer
	{
		private readonly DocumentSyntaxAnalyzer _syntaxAnalyzer = new DocumentSyntaxAnalyzer();
		private readonly DocumentSemanticAnalyzer _semanticAnalyzer = new DocumentSemanticAnalyzer();

		public async Task<IEnumerable<ClassInfo>> Analyze(Solution solution, string solutionPath)
		{
			var result = new List<ClassInfo>();

			foreach (Document doc in solution.Projects.SelectMany(p => p.Documents))
			{
				// Syntax Info
				SyntaxTree tree = await doc.GetSyntaxTreeAsync();
				List<ClassInfo> classInfos = _syntaxAnalyzer.Analyze(tree);

				// Semantic Info
				SemanticModel model = await doc.GetSemanticModelAsync();
				await _semanticAnalyzer.Analyze(solution, model, classInfos);

				result.AddRange(classInfos);
			}

			SetRelativePaths(solutionPath, result);

			return result;
		}

		private static void SetRelativePaths(string solutionPath, IEnumerable<ClassInfo> classInfos)
		{
			var basePath = Path.GetDirectoryName(solutionPath);

			// turns out we need to remove the last folder name for GitHub link compatibility
			//var folders = basePath.Split('\\');
			//basePath = string.Join("\\", folders.Take(folders.Length - 1));

			foreach (var classInfo in classInfos)
			{
				SetRelativePath(classInfo.Location);
				foreach (var methodInfo in classInfo.Methods)
				{
					SetRelativePath(methodInfo.Location);
					SetRelativePath(methodInfo.TypeLocation);
					SetReferenceRelativePaths(methodInfo);

					foreach (var p in methodInfo.Parameters.Where(p => p.TypeLocation != null))
					{
						SetRelativePath(p.TypeLocation);
					}
				}

				foreach (var propertyInfo in classInfo.Properties)
				{
					SetRelativePath(propertyInfo.Location);
					SetRelativePath(propertyInfo.TypeLocation);
					SetReferenceRelativePaths(propertyInfo);
				}				
			}

			void SetRelativePath(SourceLocation location)
			{
				if (location == null || string.IsNullOrWhiteSpace(location.Filename))
					return;
				
				location.Filename = location.Filename.Substring(basePath.Length + 1);
			}

			void SetReferenceRelativePaths(IMemberInfo member)
			{
				foreach (var reference in member.References) SetRelativePath(reference);
			}
		}
	}
}