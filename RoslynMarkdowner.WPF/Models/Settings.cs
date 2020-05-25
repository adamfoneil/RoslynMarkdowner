using JsonSettings;
using JsonSettings.Library;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RoslynMarkdowner.WPF.Models
{
    public class Settings : SettingsBase
    {
        //public FormPosition Position { get; set; }
        public int VsInstance { get; set; }
        public List<RepositoryInfo> Repositories { get; set; }
        public string VsExePath { get; set; }        

        public RemoteInfo Remote { get; set; }

        protected override void Initialize()
        {
            Repositories ??= new List<RepositoryInfo>();
        }

        public override string Filename =>
            BuildPath(Environment.SpecialFolder.LocalApplicationData, Path, "Settings.json");

        [Newtonsoft.Json.JsonIgnore]
        public string Folder =>
            System.IO.Path.GetDirectoryName(Filename);

        public const string Path = "RoslynMarkdowner";

        public class RemoteInfo
        {
            [JsonProtect(DataProtectionScope.CurrentUser)]
            public string DisplayName { get; set; }

            [JsonProtect(DataProtectionScope.CurrentUser)]
            public string UserName { get; set; }

            [JsonProtect(DataProtectionScope.CurrentUser)]
            public string Password { get; set; }
        }
    }
}
