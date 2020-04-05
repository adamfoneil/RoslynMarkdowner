using MarkdownViewer.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MarkdownViewer.App.Pages
{
    [Authorize]
    public class ClassesModel : PageModel
    {
        private readonly BlobStorage _blobStorage;

        public ClassesModel(BlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task OnGetAsync()
        {

        }
    }
}