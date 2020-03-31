namespace RoslynDoc.Library.Models
{
	public class SourceLocation
	{
		/// <summary>
		/// Relative source file path within solution
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		/// 1-based line number of location
		/// </summary>
		public int LineNumber { get; set; }
	}
}