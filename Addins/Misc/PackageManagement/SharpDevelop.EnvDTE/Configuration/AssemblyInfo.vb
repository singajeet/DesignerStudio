'Imports System.Reflection
'Imports System.Runtime.CompilerServices
'Imports System.Runtime.InteropServices

' Information about this assembly is defined by the following
' attributes.
'
' change them to the information which is associated with the assembly
' you compile.

'<assembly: AssemblyTitle("SharpDevelop.EnvDTE")>
'<assembly: AssemblyDescription("SharpDevelop EnvDTE interfaces")>
'<assembly: AssemblyConfiguration("")>
'<assembly: AssemblyCompany("ic#code")>
'<assembly: AssemblyProduct("SharpDevelop")>
'<assembly: AssemblyCopyright("2000-2013 AlphaSierraPapa for the SharpDevelop Team")>
'<assembly: AssemblyTrademark("")>
'<assembly: AssemblyCulture("")>

' This sets the default COM visibility of types in the assembly to invisible.
' If you need to expose a type to COM, use <ComVisible(true)> on that type.
'<assembly: ComVisible(False)>

' The assembly version has following format :
'
' Major.Minor.Build.Revision
'
' You can specify all values by your own or you can build default build and revision
' numbers with the '*' character (the default):

'<assembly: AssemblyVersion("4.4.0")>

Imports System.Resources
Imports System.Reflection

<Assembly: System.Runtime.InteropServices.ComVisible(False)>
<Assembly: AssemblyCompany("ic#code")>
<Assembly: AssemblyProduct("SharpDevelop")>
<Assembly: AssemblyCopyright("2000-$INSERTYEAR$ AlphaSierraPapa for the SharpDevelop Team")>
<Assembly: AssemblyVersion(RevisionClass.Major + "." + RevisionClass.Minor + "." + RevisionClass.Build + "." + RevisionClass.Revision)>
<Assembly: AssemblyInformationalVersion(RevisionClass.FullVersion + "-$INSERTSHORTCOMMITHASH$")>
<Assembly: NeutralResourcesLanguage("en-US")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly", Justification := "AssemblyInformationalVersion does not need to be a parsable version")>

' DO NOT FORGET TO EDIT the GlobalAssemblyInfo.cs.template!
Friend NotInheritable Class RevisionClass
	Public Const Major As String = "5"
	Public Const Minor As String = "2"
	Public Const Build As String = "0"
	Public Const Revision As String = "0"
	
	Public Const FullVersion As String = Major + "." + Minor + "." + Build + ".0"
End Class
