using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynDoc.Library.Models
{
    public class SolutionInfo
    {
        public DateTime Timestamp { get; set; }
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

        [JsonIgnore]
        public Dictionary<string, ClassInfo> ClassDictionary => Classes.ToDictionary(item => $"{item.Namespace}.{item.Name}");
    }
}
