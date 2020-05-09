using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Humanizer;
using Microsoft.Build.Locator;
using RoslynDoc.Library.Extensions;
using RoslynDoc.Library.Models;
using RoslynMarkdowner.WPF.Annotations;
using RoslynMarkdowner.WPF.Models;
using RoslynMarkdowner.WPF.Services;

namespace RoslynMarkdowner.WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MsBuildService _msBuildService;
        private readonly SettingsService _settingsService;

        private const string NotAnalyzedText = "Not analyzed yet";
        private const string AllAssembliesText = "All Assemblies";

        private bool _processing;
        private bool _repositoryGridVisible;
        private string _analyzeTime;
        private SolutionInfo _currentSolution;
        private string _markdownText;
        private RepositoryInfo _selectedRepository;
        private string _selectedAssembly;
        private VisualStudioInstance _selectedMsBuildInstance;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Assemblies { get; } 
        public ObservableCollection<RepositoryInfo> Repositories { get; } 
        public ObservableCollection<RepositoryInfo> RepositoriesGrid { get; }
        public ObservableCollection<NamespaceNode> Namespaces { get; }
        public ObservableCollection<ComboBoxItem<VisualStudioInstance>> MsBuildInstances { get; }

        public MainWindowViewModel(
            MsBuildService msBuildService,
            SettingsService settingsService)
        {
            _msBuildService = msBuildService;
            _settingsService = settingsService;

            Assemblies = new ObservableCollection<string>();
            Repositories = new ObservableCollection<RepositoryInfo>();
            RepositoriesGrid = new ObservableCollection<RepositoryInfo>();
            Namespaces = new ObservableCollection<NamespaceNode>();
            MsBuildInstances = new ObservableCollection<ComboBoxItem<VisualStudioInstance>>();
        }

        public void Load()
        {
            LoadMsBuildInstances();
            LoadRepositories();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool Processing
        {
            get => _processing;
            set
            {
                _processing = value;
                OnPropertyChanged();
            }
        }

        public bool RepositoriesGridVisible
        {
            get => _repositoryGridVisible;
            set
            {
                _repositoryGridVisible = value;
                OnPropertyChanged();
            }
        }

        public string MarkdownText
        {
            get => _markdownText;
            set
            {
                _markdownText = value;
                OnPropertyChanged();
            }
        }

        public string AnalyzeTime
        {
            get => _analyzeTime;
            set
            {
                _analyzeTime = value;
                OnPropertyChanged();
            }
        }

        public string SelectedAssembly
        {
            get => _selectedAssembly;
            set
            {
                _selectedAssembly = value;
                OnPropertyChanged();

                var classes = _selectedAssembly == AllAssembliesText
                    ? _currentSolution.Classes
                    : _currentSolution.Classes.Where(ci => ci.AssemblyName.Equals(_selectedAssembly));

                LoadNamespaces(classes.ToList());
            }
        }

        public VisualStudioInstance SelectedMsBuildInstance
        {
            get => _selectedMsBuildInstance;
            set
            {
                _selectedMsBuildInstance = value;
                OnPropertyChanged();
            }
        }

        public RepositoryInfo SelectedRepository
        {
            get => _selectedRepository;
            set
            {
                _selectedRepository = value;
                OnPropertyChanged();
            }
        }

        public SolutionInfo CurrentSolution
        {
            get => _currentSolution;
            set
            {
                _currentSolution = value;
                OnPropertyChanged();

                Assemblies.Clear(); 
                Assemblies.Add(AllAssembliesText);

                _currentSolution.Classes
                    .Select(c => c.AssemblyName)
                    .GroupBy(s => s)
                    .Select(grp => grp.Key)
                    .ToList()
                    .ForEach(e => Assemblies.Add(e));

                SelectedAssembly = Assemblies.First();

                LoadNamespaces(_currentSolution.Classes);
            }
        }

        private void LoadMsBuildInstances()
        {
            MsBuildInstances.Clear();

            _msBuildService.GetInstances()
                .ToList()
                .ForEach(e =>
                    MsBuildInstances.Add(
                        new ComboBoxItem<VisualStudioInstance>()
                        {
                            Text = e.Key,
                            Value = e.Value
                        }));

            SelectedMsBuildInstance = MsBuildInstances?.FirstOrDefault()?.Value;

            OnPropertyChanged(nameof(MsBuildInstances));
        }

        private void LoadRepositories()
        {
            Repositories.Clear();
            RepositoriesGrid.Clear(); 

            _settingsService.Settings.Repositories.ForEach(e =>
            {
                Repositories.Add(e);
                RepositoriesGrid.Add(e);
            });

            SelectedRepository = null; //Repositories?.FirstOrDefault()?.Value;
        }

        public void TriggerRepositoriesGridVisibility()
        {
            RepositoriesGridVisible = !RepositoriesGridVisible;
        }

        public void UpdateRepositories()
        {
            var selectedRepository = SelectedRepository;
            SelectedRepository = null;

            Repositories.Clear();
            foreach (var repositoryInfo in RepositoriesGrid)
            {
                Repositories.Add(repositoryInfo);
            }

            SelectedRepository = selectedRepository;
        }

        public void UpdateCacheAge(string solutionCache)
        {
            AnalyzeTime = File.Exists(solutionCache) 
                ? GetFileAgeText(solutionCache) 
                : NotAnalyzedText;
        }

        private string GetFileAgeText(string fileName)
            => DateTime.UtcNow.Subtract(new FileInfo(fileName).LastWriteTimeUtc).Humanize() + " ago";

        private void LoadNamespaces(List<ClassInfo> classes)
        {
            Namespaces.Clear();

            var simplifiedNames = classes.Select(ci => ci.Namespace).SimplifyNames("(root)");
            var fileCounts = classes.GroupBy(ci => $"{ci.Namespace}.{ci.Name}").ToDictionary(grp => grp.Key, grp => grp.Count());

            foreach (var nsGrp in classes.GroupBy(ci => ci.Namespace).OrderBy(grp => grp.Key))
            {
                var namespaceNode = new NamespaceNode(simplifiedNames[nsGrp.Key]);
                Namespaces.Add(namespaceNode);

                foreach (var classInfo in nsGrp.OrderBy(item => item.Name))
                {
                    var fileName = $"{classInfo.Namespace}.{classInfo.Name}";
                    var partialLocation = (fileCounts[fileName] > 1) ? GetPartialLocation(classInfo) : default;

                    var classNode = new ClassNode(classInfo, partialLocation);
                    namespaceNode.Nodes.Add(classNode);
                }
            }
        }

        private SourceLocation GetPartialLocation(ClassInfo classInfo)
        {
            var properties = classInfo.Properties.OfType<IMemberInfo>().ToList();
            var methods = classInfo.Methods.OfType<IMemberInfo>();
            var allMembers = properties.Concat(methods).ToList();

            return !allMembers.Any() 
                ? default 
                : allMembers.First().Location;
        }
    }
}
