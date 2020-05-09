namespace RoslynMarkdowner.WPF.Models
{
    public class RepositoryInfo
    {
        public string PublicUrl { get; set; }
        public string LocalSolution { get; set; }
        public string BranchName { get; set; }

        public override string ToString() =>
            $"{PublicUrl} - {BranchName}";
    }
}
