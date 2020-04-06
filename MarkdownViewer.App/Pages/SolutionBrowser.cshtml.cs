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
using System;

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
        public SelectList AssemblySelect { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SolutionName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AssemblyName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassNamespace { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassName { get; set; }

        public Func<ClassInfo, bool> ClassFilter { get; set; }

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

            SolutionSelect = new SelectList(solutions, "Value", "Text", SolutionName);

            if (!string.IsNullOrEmpty(SolutionName))
            {
                SolutionInfo = await _blobStorage.GetAsync<SolutionInfo>(User.Email(), SolutionName);

                var assemblyItems = SolutionInfo.Classes.Select(c => c.AssemblyName).GroupBy(s => s).Select(grp => new SelectListItem(grp.Key, grp.Key));
                AssemblySelect = new SelectList(assemblyItems, "Value", "Text", AssemblyName);

                ClassFilter = (c) => true;
                if (!string.IsNullOrEmpty(AssemblyName)) ClassFilter = (c) => c.AssemblyName.Equals(AssemblyName);                    
                ShortNamespaces = SolutionInfo.Classes.Where(c => ClassFilter.Invoke(c)).Select(c => c.Namespace).SimplifyNames("(base)");

                if (!string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(ClassNamespace))
                {
                    ClassInfo = SolutionInfo.ClassDictionary[ClassNamespace + "." + ClassName];
                }
            }
        }
    }
}