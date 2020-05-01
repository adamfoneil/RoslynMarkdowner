using RoslynDoc.Library.Models;
using System.Linq;
using System.Windows.Forms;

namespace RoslynMarkdowner.WinForms.Controls
{
    public class ClassNode : TreeNode
    {
        public ClassNode(ClassInfo classInfo, string partialFile) : base(NodeText(classInfo, partialFile))
        {
            ClassInfo = classInfo;
            Nodes.AddRange(classInfo.Properties.Select(p => new MemberNode(p, MemberType.Property)).ToArray());
            Nodes.AddRange(classInfo.Methods.Select(m => new MemberNode(m, MemberType.Method)).ToArray());
        }

        public ClassInfo ClassInfo { get; }

        public static string NodeText(ClassInfo classInfo, string partialFile)
        {
            string result = classInfo.Name;

            if (!string.IsNullOrEmpty(partialFile))
            {
                result += $" - {partialFile}";
            }

            return result;
        }
    }
}
