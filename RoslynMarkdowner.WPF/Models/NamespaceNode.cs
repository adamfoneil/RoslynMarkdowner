namespace RoslynMarkdowner.WPF.Models
{
    public class NamespaceNode : NodeBase<ClassNode>
    {
        public NamespaceNode(string name)
            : base(name) { }

        protected override void OnCheckChanged()
        {
            foreach (var classNode in Nodes)
            {
                classNode.Checked = Checked;
            }
        }
    }
}
