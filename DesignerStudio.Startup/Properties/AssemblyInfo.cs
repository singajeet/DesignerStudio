#region Using directives
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: 
#endregion
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
AssemblyTitle ("DesignerStudio.Startup")]
[assembly: AssemblyConfiguration ("")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyCulture ("")]
// This sets the default COM visibility of types in the assembly to invisible.
// If you need to expose a type to COM, use [ComVisible(true)] on that type.
[assembly: ComVisible (false)]
// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all the values or you can use the default the Revision and 
// Build Numbers by using the '*' as shown below:
[assembly: AssemblyDescription ("DesignerStudio IDE based on SharpDevelop")]
[assembly: AssemblyCompany ("Armin Inc")]
[assembly: AssemblyProduct ("DesignerStudio 2017")]
[assembly: AssemblyCopyright ("2017")]
[assembly: AssemblyVersion ("1.0.0.0")]
[assembly: AssemblyFileVersion ("1.0.0.0")]
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
internal static class RevisionClass
{
	public const string Major = "1";

	public const string Minor = "0";

	public const string Build = "0";

	public const string Revision = "0";

	public const string VersionName = "Beta";

	// "" is not valid for no version name, you have to use null if you don't want a version name (eg "Beta 1")
	public const string FullVersion = Major + "." + Minor + "." + Build + "." + Revision + "." + VersionName;
}

