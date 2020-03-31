using MarkdownViewer.App.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarkdownViewer.App.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {            
        }

        public bool AllowLocal { get; set; }

        public void OnGet()
        {
            AllowLocal = HttpContext.IsLocal();
        }
    }
}
