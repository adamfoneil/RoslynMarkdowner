using Humanizer;
using JsonSettings;
using JsonSettings.Library;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDoc.Library;
using RoslynDoc.Library.Models;
using RoslynDoc.Library.Extensions;
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
using WinForms.Library.Extensions.ToolStrip;
using WinForms.Library.Models;
using RoslynMarkdowner.WinForms.Controls;
using RoslynDoc.Library.Services;
using System.Text;
using Markdig;

namespace RoslynMarkdowner.WinForms
{
    public partial class frmMain : Form
    {
        private Settings _settings;
        private SolutionInfo _currentSolution;

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
            bs.ListChanged += delegate (object sender, ListChangedEventArgs e)
            {
                if (e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    var source = sender as BindingSource;
                    var data = source.DataSource as BindingList<Settings.RepoInfo>;
                    cbRepo.Fill(data);
                }
            };

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

                var repo = cbRepo.GetItem<Settings.RepoInfo>();
                var instance = cbMSBuildInstance.SelectedItem as ListItem<VisualStudioInstance>;

                MSBuildLocator.RegisterInstance(instance.Value);

                using (var ws = MSBuildWorkspace.Create())
                {
                    Errors.Clear();
                    ws.WorkspaceFailed += (o, e2) => Errors.Add(e2.Diagnostic.Message);

                    var solution = await ws.OpenSolutionAsync(repo.LocalSolution);
                    var engine = new SolutionAnalyzer();
                    var classes = (await engine.Analyze(solution, repo.LocalSolution)).ToList();
                    var output = new SolutionInfo()
                    {
                        Classes = classes,
                        RepoUrl = repo.PublicUrl,
                        BranchName = repo.BranchName,
                        Timestamp = DateTime.UtcNow
                    };

                    string cacheFile = _settings.GetSolutionInfoFilename(repo.LocalSolution);
                    await JsonFile.SaveAsync(cacheFile, output);
                    UpdateCacheAge(cacheFile);
                    await LoadSolutionInfo(cacheFile);
                }
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
            splitContainer3.Panel1Collapsed = !splitContainer3.Panel1Collapsed;
        }

        private async void cbRepo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var repo = cbRepo.GetItem<Settings.RepoInfo>();
                lblCachedInfo.Visible = repo != null;

                if (repo != null)
                {
                    string solutionCache = _settings.GetSolutionInfoFilename(repo.LocalSolution);
                    UpdateCacheAge(solutionCache);

                    if (File.Exists(solutionCache)) await LoadSolutionInfo(solutionCache);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void UpdateCacheAge(string solutionCache)
        {
            lblCachedInfo.Text = File.Exists(solutionCache) ? GetFileAgeText(solutionCache) : "Not analyzed yet";
        }

        private string GetFileAgeText(string fileName)
        {
            var fi = new FileInfo(fileName);
            return DateTime.UtcNow.Subtract(fi.LastWriteTimeUtc).Humanize() + " ago";
        }

        private async Task LoadSolutionInfo(string fileName)
        {            
            _currentSolution = await JsonFile.LoadAsync<SolutionInfo>(fileName);
            var assemblies = _currentSolution.Classes.Select(c => c.AssemblyName).GroupBy(s => s).Select(grp => grp.Key).ToList();
            assemblies.Insert(0, "All Assemblies");

            cbAssembly.SelectedIndexChanged -= AssemblySelected;
            cbAssembly.Fill(assemblies);
            cbAssembly.SelectedIndexChanged += AssemblySelected;

            LoadNamespaces(_currentSolution.Classes);
        }

        private void LoadNamespaces(IEnumerable<ClassInfo> classes)
        {
            tvObjects.Nodes.Clear();

            var simplifiedNames = classes.Select(ci => ci.Namespace).SimplifyNames("(root)");
            Dictionary<string, int> fileCounts = classes.GroupBy(ci => $"{ci.Namespace}.{ci.Name}").ToDictionary(grp => grp.Key, grp => grp.Count());

            foreach (var nsGrp in classes.GroupBy(ci => ci.Namespace).OrderBy(grp => grp.Key))
            {
                var nsNode = new NamespaceNode(simplifiedNames[nsGrp.Key]);
                tvObjects.Nodes.Add(nsNode);

                foreach (var classInfo in nsGrp.OrderBy(item => item.Name))
                {
                    string fileName = $"{classInfo.Namespace}.{classInfo.Name}";
                    string partialFile = (fileCounts[fileName] > 1) ? GetPartialFilename(classInfo) : default;
                    var classNode = new ClassNode(classInfo, partialFile);
                    nsNode.Nodes.Add(classNode);
                }
            }
        }

        private string GetPartialFilename(ClassInfo classInfo)
        {
            var properties = classInfo.Properties.OfType<IMemberInfo>();
            var methods = classInfo.Methods.OfType<IMemberInfo>();
            var allMembers = properties.Concat(methods);

            if (!allMembers.Any()) return default;

            string fileName = allMembers.First().Location.Filename;

            return Path.GetFileName(fileName);
        }

        private void AssemblySelected(object sender, EventArgs e)
        {
            var classes = (cbAssembly.SelectedIndex == 0) ?
                _currentSolution.Classes :
                _currentSolution.Classes.Where(ci => ci.AssemblyName.Equals(cbAssembly.SelectedItem as string));

            LoadNamespaces(classes);
        }

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var node = e.Node as ClassNode ?? e.Node.Parent as ClassNode;
                if (node != null)
                {
                    var md = new CSharpMarkdownHelper();
                    md.OnlinePath = _currentSolution.SourceFileBase();

                    StringBuilder sb = new StringBuilder();

                    if (node.ClassInfo.Properties.Any())
                    {
                        sb.AppendLine("# Properties");
                        foreach (var p in node.ClassInfo.Properties) sb.AppendLine(p.GetMarkdown(md));
                    }
                    
                    if (node.ClassInfo.Methods.Any())
                    {
                        sb.AppendLine("# Methods");
                        foreach (var m in node.ClassInfo.Methods) sb.AppendLine(m.GetMarkdown(md));
                    }

                    tbMarkdown.Text = sb.ToString();
                    var html = Markdown.ToHtml(tbMarkdown.Text);
                    webBrowser1.DocumentText = html;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(tbMarkdown.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            
        }
    }
}
