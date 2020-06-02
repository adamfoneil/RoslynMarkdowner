using HeyRed.MarkdownSharp;
using JsonSettings;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynDoc.Library;
using RoslynDoc.Library.Models;
using RoslynDoc.Library.Services;
using RoslynMarkdowner.WPF.Models;
using RoslynMarkdowner.WPF.Services;
using RoslynMarkdowner.WPF.ViewModels;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LibGit2Sharp;
using RoslynMarkdowner.WPF.Windows;

namespace RoslynMarkdowner.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly SettingsService _settingsService;
        private readonly WindowService _windowService;
        private readonly WikiBuilder _wikiBuilder;

        public MainWindow(
            MainWindowViewModel viewModel,
            SettingsService settingsService,
            WindowService windowService,
            WikiBuilder wikiBuilder)
        {
            _viewModel = viewModel;
            _settingsService = settingsService;
            _windowService = windowService;
            _wikiBuilder = wikiBuilder;

            InitializeComponent();

            DataContext = viewModel;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _settingsService.Settings.Repositories = _viewModel.Repositories.ToList();
            _settingsService.Save();
        }

        // Workaround to hide the Overflow button
        private void RightPanelToolbar_OnLoaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;

            if (toolBar?.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            if (toolBar?.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void OnRepositoriesManageClick(object sender, RoutedEventArgs e)
        {
            _viewModel.TriggerRepositoriesGridVisibility();
        }

        private void OnRepositoryViewOnlineClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentSolution == null ||
                    string.IsNullOrEmpty(_viewModel.CurrentSolution.RepoUrl))
                {
                    return;
                }

                Process.Start(new ProcessStartInfo(_viewModel.CurrentSolution.RepoUrl)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private async void AnalyzeButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.Processing = true;

                var repository = _viewModel.SelectedRepository;

                if (!MSBuildLocator.IsRegistered)
                {
                    MSBuildLocator.RegisterInstance(_viewModel.SelectedMsBuildInstance);
                }

                using var ws = MSBuildWorkspace.Create();
                var engine = new SolutionAnalyzer();

                // It's not used in WinForms version
                //Errors.Clear();
                //ws.WorkspaceFailed += (o, e2) => Errors.Add(e2.Diagnostic.Message);

                var solution = await ws.OpenSolutionAsync(repository.LocalSolution);
                var classes = await engine.Analyze(solution, repository.LocalSolution);

                var output = new SolutionInfo()
                {
                    Classes = classes.ToList(),
                    RepoUrl = repository.PublicUrl,
                    BranchName = repository.BranchName,
                    Timestamp = DateTime.UtcNow
                };

                var cacheFile = _settingsService.GetSolutionInfoFilename(repository.LocalSolution);
                await JsonFile.SaveAsync(cacheFile, output);

                _viewModel.UpdateCacheAge(cacheFile);
                _viewModel.CurrentSolution = await JsonFile.LoadAsync<SolutionInfo>(cacheFile);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                _viewModel.Processing = false;
            }
        }

        private async void Repositories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var repository = _viewModel.SelectedRepository;
                if (repository == null)
                {
                    _viewModel.AnalyzeTime = null;
                    return;
                }

                var solutionCache = _settingsService.GetSolutionInfoFilename(repository.LocalSolution);
                _viewModel.UpdateCacheAge(solutionCache);

                if (!File.Exists(solutionCache)) return;

                _viewModel.CurrentSolution = await JsonFile.LoadAsync<SolutionInfo>(solutionCache);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void RepositoriesGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _viewModel.UpdateRepositories();
        }

        private void CopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(_viewModel.MarkdownText);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void TreeViewItem_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateMarkdown();
        }

        private void UpdateMarkdown()
        {
            var sb = new StringBuilder();
            var md = new CSharpMarkdownHelper
            {
                OnlinePath = _viewModel.CurrentSolution.SourceFileBase()
            };

            void WriteMarkdown(NodeBase node)
            {
                switch (node)
                {
                    case ClassNode classNode:
                        {
                            if (classNode.Nodes.Any(nd => nd.Checked))
                            {
                                sb.AppendLine();
                                sb.Append($"# {classNode.ClassInfo.Namespace}.{classNode.ClassInfo.Name}");
                                sb.Append($" [{classNode.GetLinkText()}]({classNode.GetLinkHref(md)})\r\n");

                                WriteMembers(sb, md, classNode, MemberType.Property, "## Properties");
                                WriteMembers(sb, md, classNode, MemberType.Method, "## Methods");
                            }

                            foreach (var child in classNode.Nodes)
                            {
                                WriteMarkdown(child);
                            }

                            break;
                        }

                    case NamespaceNode namespaceNode:
                        {
                            foreach (var child in namespaceNode.Nodes)
                            {
                                WriteMarkdown(child);
                            }

                            break;
                        }
                }
            }

            foreach (var node in _viewModel.Namespaces)
            {
                WriteMarkdown(node);
            }

            var markdownText = sb.ToString();
            _viewModel.MarkdownText = markdownText;

            var html = new Markdown().Transform(markdownText);
            var content = !string.IsNullOrEmpty(html)
                ? html
                : "<div></div>";

            WebBrowser.NavigateToString(content);
        }

        private static void WriteMembers(StringBuilder sb, CSharpMarkdownHelper md, ClassNode classNode, MemberType type, string heading)
        {
            var propertyNodes = classNode.Nodes
                .Where(nd => nd.Checked && nd.Type == type)
                .ToList();

            if (!propertyNodes.Any()) return;

            sb.AppendLine(heading);
            propertyNodes.ForEach(m => sb.AppendLine(m.MemberInfo.GetMarkdown(md)));
        }

        private void PublishButton_OnClick(object sender, RoutedEventArgs e)
        {
            var repository = _viewModel.SelectedRepository?.LocalSolution;
            var solution = _viewModel.CurrentSolution;

            if (repository == null)
            {
                MessageBox.Show(this, "Please select a repository", "Repository is not selected", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (solution == null)
            {
                MessageBox.Show(this, "Please select a solution", "Solution is not selected", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var result = _windowService.ShowDialog<RemoteInfoWindow>();
            if (!result)
            {
                MessageBox.Show(this, "Please provide the credentials", "Credentials is not set", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var displayName = _settingsService.Settings.Remote.DisplayName;
            var username = _settingsService.Settings.Remote.UserName;
            var password = _settingsService.Settings.Remote.Password;

            _wikiBuilder.Build(
                repository,
                solution,
                new CSharpMarkdownHelper() {OnlinePath = solution.SourceFileBase()},
                new UsernamePasswordCredentials()
                {
                    Username = username,
                    Password = password
                },
                new Identity(displayName, username));
        }
    }
}
