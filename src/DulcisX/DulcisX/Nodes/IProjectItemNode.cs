namespace DulcisX.Nodes
{
    public interface IProjectItemNode : INamedNode
    {
        ProjectNode GetParentProject();
        string GetDefaultNamespace();
    }
}
