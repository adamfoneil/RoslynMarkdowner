using JsonSettings.Library;
using System;
using System.Collections.Generic;
using WinForms.Library.Models;

namespace RoslynMarkdowner.WinForms.Models
{
    public class Settings : SettingsBase
    {
        public FormPosition Position { get; set; }        

        public List<RepoInfo> Repositories { get; set; }

        public class RepoInfo
        {
            public string PublicUrl { get; set; }
            public string LocalSolution { get; set; }

            public override string ToString()
            {
                return PublicUrl;
            }
        }

        protected override void Initialize()
        {
            if (Repositories == null) Repositories = new List<RepoInfo>();
        }

        public override string Filename => BuildPath(Environment.SpecialFolder.LocalApplicationData, Path, "Settings.json");

        public const string Path = "RoslynMarkdowner";
    }
}
