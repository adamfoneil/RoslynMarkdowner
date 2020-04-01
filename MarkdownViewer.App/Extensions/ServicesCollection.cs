using MarkdownViewer.App.Services;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarkdownViewer.App.Extensions
{
    public static partial class ServicesCollection
    {
        public static void AddBlobStorage(this IServiceCollection services, IConfiguration config)
        {
            var creds = new StorageCredentials(config["StorageAccount:Name"], config["StorageAccount:Key"]);
            var container = config["StorageAccount:Container"];
            services.AddScoped((sp) => new BlobStorage(creds, container));
        }
    }
}
