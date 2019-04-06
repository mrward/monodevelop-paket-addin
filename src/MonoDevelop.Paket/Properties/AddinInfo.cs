using Mono.Addins;

[assembly:Addin ("Paket",
	Namespace = "MonoDevelop",
	Version = "0.5",
	Category = "IDE extensions")]

[assembly:AddinName ("Paket")]
[assembly:AddinDescription ("Adds Paket support.")]

[assembly:AddinDependency ("Core", "8.0")]
[assembly:AddinDependency ("Ide", "8.0")]
[assembly:AddinDependency ("SourceEditor2", "8.0")]
[assembly:AddinDependency ("DesignerSupport", "8.0")]