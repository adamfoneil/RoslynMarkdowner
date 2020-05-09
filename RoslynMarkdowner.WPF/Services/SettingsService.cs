using JsonSettings.Library;
using RoslynMarkdowner.WPF.Models;
using System.IO;

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
