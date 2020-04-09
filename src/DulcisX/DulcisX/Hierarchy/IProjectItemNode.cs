namespace DulcisX.Hierarchy
{
    public interface IProjectItemNode : INamedNode
    {
        ProjectNode GetParentProject();
        string GetDefaultNamespace();
    }
}
