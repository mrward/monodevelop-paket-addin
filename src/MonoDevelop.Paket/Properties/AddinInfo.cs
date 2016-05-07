using Mono.Addins;

[assembly:Addin ("Paket",
	Namespace = "MonoDevelop",
	Version = "0.1",
	Category = "IDE extensions")]

[assembly:AddinName ("Paket")]
[assembly:AddinDescription ("Adds Paket support.")]

[assembly:AddinDependency ("Core", "6.0")]
[assembly:AddinDependency ("Ide", "6.0")]
[assembly:AddinDependency ("SourceEditor2", "6.0")]
[assembly:AddinDependency ("DesignerSupport", "6.0")]