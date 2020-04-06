using System.Collections.Generic;
using System.Linq;

namespace MarkdownViewer.App.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// factors out common tokens in all the class namespaces to give you the shortest names for diplay purposes
        /// </summary>        
        public static Dictionary<string, string> SimplifyNames(this IEnumerable<string> fullNames)
        {
            var uniqueValues = fullNames.GroupBy(item => item).Select(grp => new { name = grp.Key, tokens = grp.Key.Split('.') });

            int index = 0;

            bool haveSameTokenAtIndex(int checkIndex)
            {
                if (uniqueValues.All(ns => ns.tokens.Length > checkIndex - 1))
                {
                    return uniqueValues.GroupBy(ns => ns.tokens[checkIndex]).Count() == 1;
                }

                return false;
            }

            while (haveSameTokenAtIndex(index)) { index++; }
        
            return uniqueValues.ToDictionary(item => item.name, item => string.Join(".", item.tokens.Skip(index)));
        }
    }
}
