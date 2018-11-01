using System;
using System.IO;
using System.Linq;

namespace RoslynDocumentor.Utils 
{
	public static class PathUtils 
	{
		//https://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path/32113484#32113484
		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <param name="trimLeadingRelativeFolder">For GitHub link compatibility, you need to remove the first folder name in the relative path</param>
		/// <returns>The relative path from the start directory to the end path.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="fromPath"/> or <paramref name="toPath"/> is <c>null</c>.</exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static string GetRelativePath(string fromPath, string toPath, bool trimLeadingRelativeFolder = false) 
		{
			if( string.IsNullOrEmpty(fromPath))
			{
				throw new ArgumentNullException("fromPath");
			}

			if(string.IsNullOrEmpty(toPath))
			{
				throw new ArgumentNullException("toPath");
			}

			Uri fromUri = new Uri(AppendDirectorySeparatorChar(fromPath));
			Uri toUri = new Uri(AppendDirectorySeparatorChar(toPath));

			if(fromUri.Scheme != toUri.Scheme) 
			{
				return toPath;
			}

			Uri relativeUri = fromUri.MakeRelativeUri(toUri);
			string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			if(string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase)) 
			{
				relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}

			if (trimLeadingRelativeFolder)
			{
				var folders = relativePath.Split('\\');
				relativePath = string.Join("/", folders.Skip(1));
			}

			return relativePath;
		}

		private static string AppendDirectorySeparatorChar(string path) 
		{
			// Append a slash only if the path is a directory and does not have a slash.
			if(!Path.HasExtension(path) && !path.EndsWith(Path.DirectorySeparatorChar.ToString())) 
			{
				return path + Path.DirectorySeparatorChar;
			}

			return path;
		}

	}
}
