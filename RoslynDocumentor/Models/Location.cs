namespace RoslynDocumentor.Models {

	public class Location {

		/// <summary>
		///     Relative source file path within solution
		/// </summary>
		public string SourceFile { get; set; }

		/// <summary>
		///     1-based line number of location
		/// </summary>
		public int LineNumber { get; set; }

	}

}
