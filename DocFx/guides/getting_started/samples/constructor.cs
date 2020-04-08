public MyFirstExtensionPackage()
{
	this.OnInitializeAsync += MyFirstExtensionPackage_OnInitializeAsync;
}

private async System.Threading.Tasks.Task MyFirstExtensionPackage_OnInitializeAsync(System.Threading.CancellationToken ct, System.IProgress<ServiceProgressData> progress)
{
	await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

	this.Solution.OpenNodeEvents.OnSaved.Hook(NodeTypes.Document, OnDocumentSaved);
}

private void OnDocumentSaved(IPhysicalNode node)
{
	
}