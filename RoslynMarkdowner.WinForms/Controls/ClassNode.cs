using RoslynDoc.Library.Models;
using RoslynDoc.Library.Services;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RoslynMarkdowner.WinForms.Controls
{
    public class ClassNode : TreeNode
    {
        public ClassNode(ClassInfo classInfo, SourceLocation partialLocation) : base(NodeText(classInfo, partialLocation))
        {
            ClassInfo = classInfo;
            PartialLocation = partialLocation;
            Nodes.AddRange(classInfo.Properties.Select(p => new MemberNode(p, MemberType.Property)).ToArray());
            Nodes.AddRange(classInfo.Methods.Select(m => new MemberNode(m, MemberType.Method)).ToArray());
        }

        public ClassInfo ClassInfo { get; }
        public SourceLocation PartialLocation { get; }

        public string GetLinkHref(CSharpMarkdownHelper helper)
        {
            var location = (PartialLocation == null) ? ClassInfo.Location : PartialLocation;
            bool lineNumbers = (PartialLocation == null);
            return helper.GetOnlineUrl(location, lineNumbers);
        }

        public string GetLinkText() => (PartialLocation == null) ?
            Path.GetFileName(ClassInfo.Location.Filename) :
            Path.GetFileName(PartialLocation.Filename);
        
        public static string NodeText(ClassInfo classInfo, SourceLocation partialLocation)
        {
            string result = classInfo.Name;

            if (partialLocation != null)
            {
                result += $" - {Path.GetFileName(partialLocation.Filename)}";
            }

            return result;
        }
    }
}
