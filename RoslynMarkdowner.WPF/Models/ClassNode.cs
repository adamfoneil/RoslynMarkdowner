using RoslynDoc.Library.Models;
using RoslynDoc.Library.Services;
using System.IO;
using System.Linq;

namespace RoslynMarkdowner.WPF.Models
{
    public class ClassNode : NodeBase<MemberNode>
    {
        public ClassInfo ClassInfo { get; }
        public SourceLocation PartialLocation { get; }

        public ClassNode(ClassInfo classInfo, SourceLocation partialLocation)
            : base(GetName(classInfo, partialLocation))
        {
            ClassInfo = classInfo;
            PartialLocation = partialLocation;

            classInfo.Properties
                .ToList()
                .ForEach(p => Nodes.Add(new MemberNode(p, MemberType.Property)));

            classInfo.Methods
                .ToList()
                .ForEach(m => Nodes.Add(new MemberNode(m, MemberType.Method)));
        }

        protected override void OnCheckChanged()
        {
            foreach (var memberNode in Nodes)
            {
                memberNode.Checked = Checked;
            }
        }

        public string GetLinkHref(CSharpMarkdownHelper helper)
        {
            var location = PartialLocation ?? ClassInfo.Location;
            var lineNumbers = PartialLocation == null;

            return helper.GetOnlineUrl(location, lineNumbers);
        }

        public string GetLinkText() => (PartialLocation == null) ?
            Path.GetFileName(ClassInfo.Location.Filename) :
            Path.GetFileName(PartialLocation.Filename);

        public static string GetName(ClassInfo classInfo, SourceLocation partialLocation)
        {
            var result = classInfo.Name;

            if (partialLocation != null)
            {
                result += $" - {Path.GetFileName(partialLocation.Filename)}";
            }

            return result;
        }
    }
}
