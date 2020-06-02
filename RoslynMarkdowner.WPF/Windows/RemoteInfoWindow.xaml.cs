using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RoslynMarkdowner.WPF.ViewModels;

namespace RoslynMarkdowner.WPF.Windows
{
    /// <summary>
    /// Interaction logic for RemoteInfoWindow.xaml
    /// </summary>
    public partial class RemoteInfoWindow : Window
    {
        private readonly RemoteInfoWindowViewModel _viewModel;

        public RemoteInfoWindow(RemoteInfoWindowViewModel viewModel)
        {
            _viewModel = viewModel;

            InitializeComponent();

            DataContext = _viewModel;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
           _viewModel.Save();

           DialogResult = true;
           Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void RemoteInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }
    }
}
