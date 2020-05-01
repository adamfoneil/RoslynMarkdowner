using RoslynDoc.Library.Models;
using System.Windows.Forms;

namespace RoslynMarkdowner.WinForms.Controls
{
    public enum MemberType
    {
        Property,
        Method
    }

    public class MemberNode : TreeNode
    {
        public MemberNode(IMemberInfo memberInfo, MemberType type) : base(memberInfo.Name)
        {
            MemberInfo = memberInfo;
            Type = type;
        }

        public IMemberInfo MemberInfo { get; }
        public MemberType Type { get; }
    }
}
