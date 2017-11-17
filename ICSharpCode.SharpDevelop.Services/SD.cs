/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/2/2017
 * Time: 5:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.ClassBrowser;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Editor.Bookmarks;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Parser;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Templates;
using ICSharpCode.SharpDevelop.WinForms;
using ICSharpCode.SharpDevelop.Workbench;

namespace ICSharpCode.SharpDevelop
{
	/// <summary>
	/// Description of SD.
	/// </summary>
	public static class SD
	{
		/// <summary>
		/// Gets a service. Returns null if service is not found.
		/// </summary>
		public static T GetService<T>() where T : class
		{
			return ServiceSingleton.ServiceProvider.GetService<T>();
		}
		
		/// <summary>
		/// Gets a service. Returns null if service is not found.
		/// </summary>
		public static T GetRequiredService<T>() where T : class
		{
			return ServiceSingleton.ServiceProvider.GetRequiredService<T>();
		}
		
		/// <inheritdoc see="Core.IResourceService"/>
		public static Core.IResourceService ResourceService {
			get { return GetRequiredService<Core.IResourceService>(); }
		}
		
		/// <summary>
		/// Gets the <see cref="IMessageLoop"/> representing the main UI thread.
		/// </summary>
		public static IMessageLoop MainThread {
			get { return GetRequiredService<IMessageLoop>(); }
		}
		
		/// <inheritdoc see="IProjectService"/>
		public static IProjectService ProjectService {
			get { return GetRequiredService<IProjectService>(); }
		}
		
		/// <inheritdoc see="ILoggingService"/>
		public static ILoggingService Log {
			get { return GetRequiredService<ILoggingService>(); }
		}
		
		/// <inheritdoc see="IMessageService"/>
		public static IMessageService MessageService {
			get { return GetRequiredService<IMessageService>(); }
		}
		
		/// <summary>
		/// Gets the main service container for SharpDevelop.
		/// </summary>
		public static IServiceContainer Services {
			get { return GetRequiredService<IServiceContainer>(); }
		}
		
		/// <inheritdoc see="IPropertyService"/>
		public static IPropertyService PropertyService {
			get { return GetRequiredService<IPropertyService>(); }
		}
		
		/// <inheritdoc see="IWorkbench"/>
		public static IWorkbench Workbench {
			get { return GetRequiredService<IWorkbench>(); }
		}
		
		/// <inheritdoc see="IWinFormsService"/>
		public static IWinFormsService WinForms {
			get { return GetRequiredService<IWinFormsService>(); }
		}	
		
		/// <inheritdoc see="IFileService"/>
		public static IFileService FileService {
			get { return GetRequiredService<IFileService>(); }
		}
		
		/// <inheritdoc see="IStatusBarService"/>
		public static IStatusBarService StatusBar {
			get { return GetRequiredService<IStatusBarService>(); }
		}
		
		/// <inheritdoc see="IShutdownService"/>
		public static IShutdownService ShutdownService {
			get { return GetRequiredService<IShutdownService>(); }
		}
		
		/// <inheritdoc see="IDisplayBindingService"/>
		public static IDisplayBindingService DisplayBindingService {
			get { return GetRequiredService<IDisplayBindingService>(); }
		}
		
		/// <inheritdoc see="IUIService"/>
		public static IUIService UIService {
			get { return GetRequiredService<IUIService>(); }
		}
		
		/// <inheritdoc see="IAddInTree"/>
		public static IAddInTree AddInTree {
			get { return GetRequiredService<IAddInTree>(); }
		}
		
		/// <inheritdoc see="IGlobalAssemblyCacheService"/>
		public static IGlobalAssemblyCacheService GlobalAssemblyCache {
			get { return GetRequiredService<IGlobalAssemblyCacheService>(); }
		}
		
		/// <inheritdoc see="IAssemblyParserService"/>
		public static IAssemblyParserService AssemblyParserService {
			get { return GetRequiredService<IAssemblyParserService>(); }
		}
		
		/// <inheritdoc see="IEditorControlService"/>
		public static IEditorControlService EditorControlService {
			get { return GetRequiredService<IEditorControlService>(); }
		}
		
		/// <inheritdoc see="IMSBuildEngine"/>
		public static IMSBuildEngine MSBuildEngine {
			get { return GetRequiredService<IMSBuildEngine>(); }
		}
		
		/// <inheritdoc see="ITemplateService"/>
		public static ITemplateService Templates {
			get { return GetRequiredService<ITemplateService>(); }
		}
		
		/// <inheritdoc see="IBuildService"/>
		public static IBuildService BuildService {
			get { return GetRequiredService<IBuildService>(); }
		}
		
		/// <inheritdoc see="IDebuggerService"/>
		public static IDebuggerService Debugger {
			get { return GetRequiredService<IDebuggerService>(); }
		}
		
		/// <summary>
		/// Equivalent to <code>SD.Workbench.ActiveViewContent.GetService(type)</code>,
		/// but does not throw a NullReferenceException when ActiveViewContent is null.
		/// (instead, null is returned).
		/// </summary>
		public static object GetActiveViewContentService(Type type)
		{
			var workbench = ServiceSingleton.ServiceProvider.GetService(typeof(IWorkbench)) as IWorkbench;
			if (workbench != null) {
				var activeViewContent = workbench.ActiveViewContent;
				if (activeViewContent != null) {
					return activeViewContent.GetService(type);
				}
			}
			return null;
		}
		
		/// <summary>
		/// Equivalent to <code>SD.Workbench.ActiveViewContent.GetService&lt;T&gt;()</code>,
		/// but does not throw a NullReferenceException when ActiveViewContent is null.
		/// (instead, null is returned).
		/// </summary>
		public static T GetActiveViewContentService<T>() where T : class
		{
			return (T)GetActiveViewContentService(typeof(T));
		}
		
		/// <summary>
		/// Returns a task that gets completed when the service is initialized.
		/// 
		/// This method does not try to initialize the service -- if no other code forces the service
		/// to be initialized, the task will never complete.
		/// </summary>
		/// <remarks>
		/// This method can be used to solve cyclic dependencies in service initialization.
		/// </remarks>
		public static Task<T> GetFutureService<T>() where T : class
		{
			return GetRequiredService<SharpDevelopServiceContainer>().GetFutureService<T>();
		}
		
		/// <inheritdoc see="IClipboard"/>
		public static IClipboard Clipboard {
			get { return GetRequiredService<IClipboard>(); }
		}
		
		/// <inheritdoc see="IAnalyticsMonitor"/>
		public static IAnalyticsMonitor AnalyticsMonitor {
			get { return GetRequiredService<IAnalyticsMonitor>(); }
		}
		
		/// <inheritdoc see="IParserService"/>
		public static IParserService ParserService {
			get { return GetRequiredService<IParserService>(); }
		}
		
		/// <inheritdoc see="ITreeNodeFactory"/>
		public static ITreeNodeFactory TreeNodeFactory {
			get { return GetRequiredService<ITreeNodeFactory>(); }
		}
		
		/// <inheritdoc see="IClassBrowser"/>
		public static IClassBrowser ClassBrowser {
			get { return GetRequiredService<IClassBrowser>(); }
		}
		
		/// <inheritdoc see="IBookmarkManager"/>
		public static IBookmarkManager BookmarkManager {
			get { return GetRequiredService<IBookmarkManager>(); }
		}
		
		/// <inheritdoc see="IOutputPad"/>
		public static IOutputPad OutputPad {
			get { return GetRequiredService<IOutputPad>(); }
		}
		
		/// <inheritdoc see="IFileSystem"/>
		public static IFileSystem FileSystem {
			get { return GetRequiredService<IFileSystem>(); }
		}
	}
}
