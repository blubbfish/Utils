#if !NETCOREAPP
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die mit einer Assembly verknüpft sind.
[assembly: AssemblyTitle("Utils")]
[assembly: AssemblyDescription("Provides useful classes for other projects")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("BlubbFish")]
[assembly: AssemblyProduct("Utils")]
[assembly: AssemblyCopyright("Copyright © BlubbFish 2014 - 10.04.2021")]
[assembly: AssemblyTrademark("BlubbFish")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("de-DE")]


// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten. Wenn Sie auf einen Typ in dieser Assembly von 
// COM zugreifen müssen, legen Sie das ComVisible-Attribut für diesen Typ auf "true" fest.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("6f20376a-5c71-4979-9932-13c105d1c6e6")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.5.0")]
[assembly: AssemblyFileVersion("1.5.0")]
#endif

/**
 * 1.5.0 Add GetEvent so you can call events by string; Add OwnSingeton class
 * 1.4.0 Add Helper to Utils
 */
