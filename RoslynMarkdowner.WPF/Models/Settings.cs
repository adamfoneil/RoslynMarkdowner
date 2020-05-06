using System;
using System.Collections.Generic;
using System.Text;
using JsonSettings.Library;
using Newtonsoft.Json;

namespace RoslynMarkdowner.WPF.Models
{
    public class Settings : SettingsBase
    {
        //public FormPosition Position { get; set; }
        public int VsInstance { get; set; }
        public List<RepositoryInfo> Repositories { get; set; }
        public string VsExePath { get; set; }

        protected override void Initialize()
        {
            Repositories ??= new List<RepositoryInfo>();
        }

        public override string Filename =>
            BuildPath(Environment.SpecialFolder.LocalApplicationData, Path, "Settings.json");

        [JsonIgnore]
        public string Folder =>
            System.IO.Path.GetDirectoryName(Filename);

        public const string Path = "RoslynMarkdowner";
    }
}
