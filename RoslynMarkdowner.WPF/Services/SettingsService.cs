using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsonSettings.Library;
using RoslynMarkdowner.WPF.Models;

namespace RoslynMarkdowner.WPF.Services
{
    public class SettingsService
    {
        public Settings Settings { get; }

        public SettingsService()
        {
            Settings = SettingsBase.Load<Settings>();
        }

        public void Save()
        {
            Settings.Save();
        }

        public string GetSolutionInfoFilename(string solutionFile) =>
            Path.Combine(Settings.Folder, Path.GetFileNameWithoutExtension(solutionFile) + ".json");
    }
}
