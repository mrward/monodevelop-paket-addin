using Mono.Addins;

[assembly:Addin ("Paket",
	Namespace = "MonoDevelop",
	Version = "0.5",
	Category = "IDE extensions")]

[assembly:AddinName ("Paket")]
[assembly:AddinDescription ("Adds Paket support.")]

[assembly:AddinDependency ("Core", "7.0")]
[assembly:AddinDependency ("Ide", "7.0")]
[assembly:AddinDependency ("SourceEditor2", "7.0")]
[assembly:AddinDependency ("DesignerSupport", "7.0")]