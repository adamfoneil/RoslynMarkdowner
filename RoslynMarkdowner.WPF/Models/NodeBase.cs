using RoslynMarkdowner.WPF.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RoslynMarkdowner.WPF.Models
{
    public abstract class NodeBase : INotifyPropertyChanged
    {
        private bool _checked;

        public string Name { get; }

        protected virtual void OnCheckChanged() { }

        protected NodeBase(string name)
        {
            Name = name;
        }

        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                OnPropertyChanged();
                OnCheckChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public abstract class NodeBase<TSubNode> : NodeBase, INotifyPropertyChanged
    {
        public ObservableCollection<TSubNode> Nodes { get; }

        protected NodeBase(string name)
            : base(name)
        {
            Nodes = new ObservableCollection<TSubNode>();
        }
    }
}
