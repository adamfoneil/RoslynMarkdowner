using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynMarkdowner.WPF.Models
{
    public class ComboBoxItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
    }
}
