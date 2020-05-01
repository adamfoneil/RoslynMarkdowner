using Microsoft.Azure.Storage.Blob;

namespace MarkdownViewer.App.Extensions
{
    public static class BlobExtensions
    {
        public static string DisplayName(this CloudBlockBlob blob, string userName) => blob.Name.Substring(userName.Length + 1);

    }
}
