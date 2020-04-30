using JsonSettings.Library;
using System;
using System.Collections.Generic;
using WinForms.Library.Models;

namespace RoslynMarkdowner.WinForms.Models
{
    public class Settings : SettingsBase
    {
        public FormPosition Position { get; set; }

        public HashSet<string> RepositoryUrls { get; set; }
        public HashSet<string> LocalSolutions { get; set; }

        public override string Filename => BuildPath(Environment.SpecialFolder.LocalApplicationData, "RoslynMarkdowner", "Settings.json");
    }
}
