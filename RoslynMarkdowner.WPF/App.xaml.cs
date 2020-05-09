using Microsoft.Extensions.DependencyInjection;
using RoslynMarkdowner.WPF.Services;
using RoslynMarkdowner.WPF.ViewModels;
using System;
using System.Windows;

namespace RoslynMarkdowner.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ServiceProvider = BuildServiceProvider();
            ServiceProvider.GetRequiredService<MainWindow>().Show();
        }

        private IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<MsBuildService>()
                .AddSingleton<SettingsService>()
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}
