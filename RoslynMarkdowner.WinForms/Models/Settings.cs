using JsonSettings.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WinForms.Library.Models;

namespace RoslynMarkdowner.WinForms.Models
{
    public class Settings : SettingsBase
    {
        public FormPosition Position { get; set; }

        public int VSInstance { get; set; }

        public List<RepoInfo> Repositories { get; set; }

        public string VSExePath { get; set; }

        public class RepoInfo
        {
            public string PublicUrl { get; set; }
            public string LocalSolution { get; set; }
            public string BranchName { get; set; }

            public override string ToString()
            {
                return $"{PublicUrl} - {BranchName}";
            }
        }

        protected override void Initialize()
        {
            if (Repositories == null) Repositories = new List<RepoInfo>();
        }

        public override string Filename =>
            BuildPath(Environment.SpecialFolder.LocalApplicationData, Path, "Settings.json");

        [JsonIgnore]
        public string Folder =>
            System.IO.Path.GetDirectoryName(Filename);

        public string GetSolutionInfoFilename(string solutionFile) =>
            System.IO.Path.Combine(Folder, System.IO.Path.GetFileNameWithoutExtension(solutionFile) + ".json");

        public const string Path = "RoslynMarkdowner";
    }
}
