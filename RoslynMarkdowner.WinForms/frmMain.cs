using Humanizer;
using JsonSettings;
using JsonSettings.Library;
using Markdig;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDoc.Library;
using RoslynDoc.Library.Extensions;
using RoslynDoc.Library.Models;
using RoslynDoc.Library.Services;
using RoslynMarkdowner.WinForms.Controls;
using RoslynMarkdowner.WinForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.Library.Extensions.ComboBoxes;
using WinForms.Library.Extensions.ToolStrip;
using WinForms.Library.Models;

namespace RoslynMarkdowner.WinForms
{
    public partial class frmMain : Form
    {
        private Settings _settings;
        private SolutionInfo _currentSolution;
        private Settings.RepoInfo _currentRepo;
        private TreeNode _currentNode;

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
            cbMSBuildInstance.SelectedIndex = _settings.VSInstance;

            cbRepo.Fill(_settings.Repositories);

            splitContainer3.Panel1Collapsed = true;
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

            bs.CurrentItemChanged += delegate (object sender, EventArgs e)
            {                
                cbRepo.SelectedItem = (dgvRepos.DataSource as BindingSource).Current as Settings.RepoInfo;
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

        private async void btnAnalyzeSolution_Click(object sender, EventArgs e)
        {
            try
            {
                pbMain.Visible = true;

                var repo = cbRepo.GetItem<Settings.RepoInfo>();
                var instance = cbMSBuildInstance.SelectedItem as ListItem<VisualStudioInstance>;

                if (!MSBuildLocator.IsRegistered) MSBuildLocator.RegisterInstance(instance.Value);
                
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

        private void llManageRepos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer3.Panel1Collapsed = !splitContainer3.Panel1Collapsed;
        }

        private async void cbRepo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var repo = cbRepo.GetItem<Settings.RepoInfo>();
                _currentRepo = repo;

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
                    var partialLocation = (fileCounts[fileName] > 1) ? GetPartialLocation(classInfo) : default;
                    var classNode = new ClassNode(classInfo, partialLocation);
                    nsNode.Nodes.Add(classNode);
                }
            }
        }

        private SourceLocation GetPartialLocation(ClassInfo classInfo)
        {
            var properties = classInfo.Properties.OfType<IMemberInfo>();
            var methods = classInfo.Methods.OfType<IMemberInfo>();
            var allMembers = properties.Concat(methods);

            if (!allMembers.Any()) return default;

            return allMembers.First().Location;
        }

        private void AssemblySelected(object sender, EventArgs e)
        {
            var classes = (cbAssembly.SelectedIndex == 0) ?
                _currentSolution.Classes :
                _currentSolution.Classes.Where(ci => ci.AssemblyName.Equals(cbAssembly.SelectedItem as string));

            LoadNamespaces(classes);
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

        private void cbMSBuildInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.VSInstance = cbMSBuildInstance.SelectedIndex;
        }

        private void tvObjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                void checkChildren(TreeNode node)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        child.Checked = node.Checked;
                        checkChildren(child);
                    }
                }

                checkChildren(e.Node);

                UpdateMarkdown();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void UpdateMarkdown()
        {
            StringBuilder sb = new StringBuilder();
            var md = new CSharpMarkdownHelper();
            md.OnlinePath = _currentSolution.SourceFileBase();

            void writeMarkdown(TreeNode node)
            {
                var classNode = node as ClassNode;
                if (classNode != null && classNode.Nodes.OfType<MemberNode>().Any(nd => nd.Checked))
                {
                    sb.AppendLine();
                    sb.Append($"# {classNode.ClassInfo.Namespace}.{classNode.ClassInfo.Name}");
                    sb.Append($" [{classNode.GetLinkText()}]({classNode.GetLinkHref(md)})\r\n");
                    
                    WriteMembers(sb, md, classNode, MemberType.Property, "## Properties");
                    WriteMembers(sb, md, classNode, MemberType.Method, "## Methods");
                }

                foreach (TreeNode child in node.Nodes) writeMarkdown(child);
            }

            foreach (TreeNode node in tvObjects.Nodes) writeMarkdown(node);

            tbMarkdown.Text = sb.ToString();
            var html = Markdown.ToHtml(tbMarkdown.Text);
            webBrowser1.DocumentText = html;
        }

        private static void WriteMembers(StringBuilder sb, CSharpMarkdownHelper md, ClassNode classNode, MemberType type, string heading)
        {
            var propertyNodes = classNode.Nodes.OfType<MemberNode>().Where(nd => nd.Checked && nd.Type == type);
            if (propertyNodes.Any())
            {
                sb.AppendLine(heading);
                foreach (var memberNode in propertyNodes) sb.AppendLine(memberNode.MemberInfo.GetMarkdown(md));
            }
        }

        private void btnViewOnline_Click(object sender, EventArgs e)
        {
            try
            {
                FileSystem.OpenDocument(_currentSolution.RepoUrl);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void tvObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) _currentNode = e.Node;
        }

        private void openSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClassNode findClass(TreeNode node)
                {
                    if (node == null) return null;
                    ClassNode result = node as ClassNode;
                    if (result != null) return result;
                    return findClass(node.Parent);
                };

                if (GetVisualStudioExe(out string exePath))
                {                    
                    var classNode = findClass(_currentNode);

                    string args = " /edit ";
                    if (classNode != null)
                    {
                        string sourceFile = Path.Combine(Path.GetDirectoryName(_currentRepo.LocalSolution), classNode.ClassInfo.Location.Filename);
                        args += sourceFile;
                    }

                    Process.Start(exePath, args);
                }
                else
                {
                    MessageBox.Show("Visual Studio path not set.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private bool GetVisualStudioExe(out string exePath)
        {
            if (string.IsNullOrEmpty(_settings.VSExePath))
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "devenv.exe";
                dlg.Filter = "Executable files|*.exe|All Files|*.*";
                if (dlg.ShowDialog() == DialogResult.OK && Path.GetFileName(dlg.FileName).ToLower().Equals("devenv.exe"))
                {
                    _settings.VSExePath = dlg.FileName;
                }
                else
                {
                    exePath = null;
                    return false;
                }
            }

            exePath = _settings.VSExePath;
            return true;
        }
    }
}
