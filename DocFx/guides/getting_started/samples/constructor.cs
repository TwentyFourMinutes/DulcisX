public MyFirstExtensionPackage()
{
    this.Solution.OpenNodeEvents.OnSaved.Hook(NodeTypes.Document, OnDocumentSaved);
}

private void OnDocumentSaved(IPhysicalNode node)
{

}