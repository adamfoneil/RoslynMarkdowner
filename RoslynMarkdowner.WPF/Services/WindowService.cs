using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace RoslynMarkdowner.WPF.Services
{
    public class WindowService
    {
        private readonly IServiceProvider _services;

        public WindowService(IServiceProvider services)
        {
            _services = services;
        }

        public bool ShowDialog<TWindow>()
            where TWindow : Window
            => _services.GetService<TWindow>().ShowDialog() == true;
    }
}
