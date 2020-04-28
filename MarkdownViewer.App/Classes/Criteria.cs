using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownViewer.App.Classes
{
    public class Criteria<T>
    {
        private readonly List<Func<T, bool>> _filters;

        public Criteria()
        {
            _filters = new List<Func<T, bool>>();
        }

        public void Add(Func<T, bool> filter)
        {
            _filters.Add(filter);
        }

        public void AddIf(bool condition, Func<T, bool> filter)
        {
            if (condition) Add(filter);
        }

        public bool Invoke(T @object) => _filters.All(f => f.Invoke(@object));
    }
}
