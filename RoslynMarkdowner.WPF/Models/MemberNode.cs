using System;
using System.Collections.Generic;
using System.Text;
using RoslynDoc.Library.Models;

namespace RoslynMarkdowner.WPF.Models
{
    public enum MemberType
    {
        Property,
        Method
    }

    public class MemberNode : NodeBase
    {
        public IMemberInfo MemberInfo { get; }
        public MemberType Type { get; }

        public MemberNode(IMemberInfo memberInfo, MemberType type)
            : base(memberInfo.Name)
        {
            MemberInfo = memberInfo;
            Type = type;
        }
    }
}
