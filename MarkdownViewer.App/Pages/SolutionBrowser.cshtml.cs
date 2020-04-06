using MarkdownViewer.App.Extensions;
using MarkdownViewer.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoslynDoc.Library.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MarkdownViewer.App.Pages
{
    [Authorize]
    public class SolutionBrowserModel : PageModel
    {
        private readonly BlobStorage _blobStorage;

        public SolutionBrowserModel(BlobStorage blobStorage, CSharpMarkdownHelper csMarkdownHelper)
        {
            _blobStorage = blobStorage;
            CSMarkdown = csMarkdownHelper;
        }

        public SelectList SolutionSelect { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SolutionName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassNamespace { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassName { get; set; }

        public CSharpMarkdownHelper CSMarkdown { get; }
        public SolutionInfo SolutionInfo { get; private set; }
        public ClassInfo ClassInfo { get; private set; }
        public Dictionary<string, string> ShortNamespaces { get; set; }

        public async Task OnGetAsync()
        {
            var solutions = (await _blobStorage.ListContentAsync(User.Email())).Select(blob =>
            {
                string name = blob.DisplayName(User.Email());
                return new SelectListItem(name, name);
            });

            SolutionSelect = new SelectList(solutions, "Value", "Text");

            if (!string.IsNullOrEmpty(SolutionName))
            {
                SolutionInfo = await _blobStorage.GetAsync<SolutionInfo>(User.Email(), SolutionName);
                ShortNamespaces = SolutionInfo.Classes.Select(c => c.Namespace).SimplifyNames();

                if (!string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(ClassNamespace))
                {
                    ClassInfo = SolutionInfo.ClassDictionary[ClassNamespace + "." + ClassName];
                }
            }
        }
    }
}