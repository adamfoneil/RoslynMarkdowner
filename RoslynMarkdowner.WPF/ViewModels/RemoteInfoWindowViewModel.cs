using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using RoslynMarkdowner.WPF.Annotations;
using RoslynMarkdowner.WPF.Models;
using RoslynMarkdowner.WPF.Services;

namespace RoslynMarkdowner.WPF.ViewModels
{
    public class RemoteInfoWindowViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly SettingsService _settingsService;

        private readonly Dictionary<string, ICollection<string>>
            _errors = new Dictionary<string, ICollection<string>>();

        private string _userName;
        private string _displayName;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public RemoteInfoWindowViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public bool HasErrors
            => _errors.Count > 0;

        public bool IsValid
            => !HasErrors;

        public IEnumerable GetErrors(string propertyName)
            => _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;

        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(value);
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(value);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(value);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                _errors[propertyName] = new List<string>() {"Field is Required"};
            }
            else
            {
                _errors.Remove(propertyName);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasErrors)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }

        public void Load()
        {
            DisplayName = _settingsService.Settings.Remote?.DisplayName;
            UserName = _settingsService.Settings.Remote?.UserName;
            Password = _settingsService.Settings.Remote?.Password;
        }

        public void Save()
        {
            _settingsService.Settings.Remote ??= new Settings.RemoteInfo();

            _settingsService.Settings.Remote.DisplayName = DisplayName;
            _settingsService.Settings.Remote.UserName = UserName;
            _settingsService.Settings.Remote.Password = Password;

            _settingsService.Save();
        }
    }
}
