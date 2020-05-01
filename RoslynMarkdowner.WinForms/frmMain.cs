using JsonSettings;
using JsonSettings.Library;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDoc.Library;
using RoslynDoc.Library.Models;
using RoslynMarkdowner.WinForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.Library.Extensions.ComboBoxes;
using WinForms.Library.Models;

namespace RoslynMarkdowner.WinForms
{
    public partial class frmMain : Form
    {
        private Settings _settings;

        public frmMain()
        {
            InitializeComponent();
            dgvRepos.AutoGenerateColumns = false;
        }

        public List<string> Errors { get; set; } = new List<string>();

        private void frmMain_Load(object sender, EventArgs e)
        {
            _settings = SettingsBase.Load<Settings>();
            _settings.Position?.Apply(this);

            FillRepoList(_settings.Repositories);
         
            cbMSBuildInstance.Fill(GetMSBuildInstances());
            cbRepo.Fill(_settings.Repositories);
        }

        private void FillRepoList(List<Settings.RepoInfo> repositories)
        {
            var list = new BindingList<Settings.RepoInfo>();
            foreach (var item in repositories) list.Add(item);
            var bs = new BindingSource();
            bs.DataSource = list;
            dgvRepos.DataSource = bs;
        }

        private Dictionary<string, VisualStudioInstance> GetMSBuildInstances()
        {
            var instances = MSBuildLocator.QueryVisualStudioInstances();
            var keyPairs = instances.Select((instance, index) => new KeyValuePair<string, VisualStudioInstance>($"{instance.Name} - {instance.Version}", instance));
            return keyPairs.ToDictionary(kp => kp.Key, kp => kp.Value);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _settings.Position = FormPosition.FromForm(this);
            _settings.Repositories = ((dgvRepos.DataSource as BindingSource).DataSource as BindingList<Settings.RepoInfo>).ToList();
            _settings.Save();
        }


        private void llSelectLocalRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Solution Files|*.sln|All Files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //rbLocalRepo.Checked = true;
                //cbLocalSolution.Text = dlg.FileName;
            }
        }

        private async void btnAnalyzeSolution_Click(object sender, EventArgs e)
        {
            try
            {
                pbMain.Visible = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                pbMain.Visible = false;
            }
        }

        /// <summary>
        /// downloads a repository from the given URL as a zip file, extracts to a temp location, and returns the solution file. 
        /// </summary>
        private async Task<string> DownloadSolutionZipAsync(string url)
        {
            var uri = new Uri(url);

            string localZipFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Settings.Path,
                uri.GetComponents(UriComponents.Path, UriFormat.Unescaped));

            string path = Path.GetDirectoryName(localZipFile);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (File.Exists(localZipFile)) File.Delete(localZipFile);

            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(uri, localZipFile);
            }

            ZipFile.ExtractToDirectory(localZipFile, path);

            string result = FindSolutionFile(path);

            return result;
        }

        private string FindSolutionFile(string path)
        {
            string result = null;

            FileSystem.EnumFiles(path, "*.sln", fileFound: (fi) =>
            {
                result = fi.FullName;
                return EnumFileResult.Stop;
            });

            return result;
        }

        private void llManageRepos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
        }
    }
}
