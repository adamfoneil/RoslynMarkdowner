using System.Collections.Generic;

namespace RoslynDoc.Library.Models
{
    public class SolutionInfo
    {
        public string RepoUrl { get; set; }
        public string BranchName { get; set; }

        public string SourceFileUrl(string path)
        {
            return $"{SourceFileBase()}{path}";
        }

        public string SourceFileBase()
        {
            return $"{RepoUrl}/blob/{BranchName}/";
        }

        public List<ClassInfo> Classes { get; set; }
    }
}
