/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 10/31/2017
 * Time: 12:07 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using ICSharpCode.SharpDevelop;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Logging;
using ICSharpCode.SharpDevelop.Sda;
using ICSharpCode.SharpDevelop.Workbench;

namespace DesignerStudio.Startup
{
	/// <summary>
	/// Description of SharpDevelopMain.
	/// </summary>
	public class SharpDevelopMain
	{
		
		#region variables
		static SharpDevelopMain _instance;
		bool _useCheckEnvironment = false;
		bool _noLogo = false;
		string _configDirectory;
		string _applicationName;
		string _domPersistencePath;
		string _addinPath;
		string[] _commandLineArgs = null;
		bool _useExceptionBox = false;
		string _appPath;
		string _resourceAssemblyName = "DesignerStudio.Startup";
		#endregion
		
		#region constructors
		private SharpDevelopMain()
		{
		
		}
		#endregion
		
		#region public properties
		public string AppPath{
			get { return this._appPath; }
			set { this._appPath = value; }
		}
		
		public string ApplicationName{
			get { return this._applicationName; }
			set { this._applicationName = value; }
		}
		
		public string DomPersistencePath{
			get { return this._domPersistencePath; }
			set { this._domPersistencePath = value; }
		}
		
		public string AddinsPath{
			get { return this._addinPath; }
			set { _addinPath = value; }
		}
		
		public static SharpDevelopMain Instance{
			get { 
				if (_instance == null)
					_instance = new SharpDevelopMain();				
				return _instance;
			}
		}
		
		public string ResourceAssemblyName{
			get { return this._resourceAssemblyName; }
			set { this._resourceAssemblyName = value; }
		}
		
		public bool UseCheckEnvironment{
			get { return this._useCheckEnvironment; }
			set { this._useCheckEnvironment = value; }
		}
		
		public string[] CommandLineArgs {
			get {
				return _commandLineArgs;
			}
			
			set { _commandLineArgs = value; }
		}
		
		public bool NoLogo{
			get { return this._noLogo; }
			set { this._noLogo = true; }
		}
		
		public bool UseExceptionBox {
			get {
				#if DEBUG
				if (Debugger.IsAttached) return false;
				#endif
				foreach(string arg in _commandLineArgs) {
					if (arg.Contains("noExceptionBox")) return false;
				}
				return _useExceptionBox;
			}
			set { _useExceptionBox = value; }
		}
		
		public string ConfigDirectory{
			get { return this._configDirectory; }
			set { this._configDirectory = value; }
		}
		#endregion
		
		public void HandleMainException(Exception ex)
		{
			LoggingService.Fatal(ex);
			try {
				Application.Run(new ExceptionBox(ex, "Unhandled exception terminated SharpDevelop", true));
			} catch {
				MessageBox.Show(ex.ToString(), "Critical error (cannot use ExceptionBox)");
			}
		}
		
		public void Run()
		{
			// DO NOT USE LoggingService HERE!
			// LoggingService requires ICSharpCode.Core.dll and log4net.dll
			// When a method containing a call to LoggingService is JITted, the
			// libraries are loaded.
			// We want to show the SplashScreen while those libraries are loading, so
			// don't call LoggingService.
			
			#if DEBUG
			Control.CheckForIllegalCrossThreadCalls = true;
			#endif	
			
			Application.SetCompatibleTextRenderingDefault(false);
			SplashScreenForm.SetCommandLineArgs(CommandLineArgs);
			
			foreach (string parameter in SplashScreenForm.GetParameterList()) {
				if ("nologo".Equals(parameter, StringComparison.OrdinalIgnoreCase))
					NoLogo = true;
			}
			
			if (_useCheckEnvironment) {
				if (!CheckEnvironment())
					return;
			}
			
			if (!NoLogo) {
				SplashScreenForm.ShowSplashScreen();
			}
			try {
				RunApplication();
			} finally {
				if (SplashScreenForm.SplashScreen != null) {
					SplashScreenForm.SplashScreen.Dispose();
				}
			}
		}
		
		public bool CheckEnvironment()
		{
			// Safety check: our setup already checks that .NET 4 is installed, but we manually check the .NET version in case SharpDevelop is
			// used on another machine than it was installed on (e.g. "SharpDevelop on USB stick")
			if (!DotnetDetection.IsDotnet45Installed()) {
				MessageBox.Show("This version of SharpDevelop requires .NET 4.5. You are using: " + Environment.Version, "SharpDevelop");
				return false;
			}
			// Work around a WPF issue when %WINDIR% is set to an incorrect path
			string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows, Environment.SpecialFolderOption.DoNotVerify);
			if (Environment.GetEnvironmentVariable("WINDIR") != windir) {
				Environment.SetEnvironmentVariable("WINDIR", windir);
			}
			return true;
		}
		
		public void RunApplication()
		{
			// The output encoding differs based on whether SharpDevelop is a console app (debug mode)
			// or Windows app (release mode). Because this flag also affects the default encoding
			// when reading from other processes' standard output, we explicitly set the encoding to get
			// consistent behaviour in debug and release builds of SharpDevelop.
			
			#if DEBUG
			// Console apps use the system's OEM codepage, windows apps the ANSI codepage.
			// We'll always use the Windows (ANSI) codepage.
			try {
				Console.OutputEncoding = System.Text.Encoding.Default;
			} catch (IOException) {
				// can happen if SharpDevelop doesn't have a console
			}
			#endif
			
			LoggingService.Info("Starting up {0}", ApplicationName);
			try {
				StartupSettings startup = new StartupSettings();
				#if DEBUG
				startup.UseSharpDevelopErrorHandler = UseExceptionBox;
				#endif
				
				Assembly exe = typeof(SharpDevelopMain).Assembly;
				if(String.IsNullOrEmpty(AppPath))
					startup.ApplicationRootPath = Path.Combine(Path.GetDirectoryName(exe.Location), "..");
				else
					startup.ApplicationRootPath = AppPath;
				startup.AllowUserAddIns = true;
				
				LoggingService.Debug("ApplicationRootPath has been set to [{0}]", startup.ApplicationRootPath);
				
				if(String.IsNullOrEmpty(ConfigDirectory))
					ConfigDirectory = ConfigurationManager.AppSettings["settingsPath"];
				
				if (String.IsNullOrEmpty(ConfigDirectory)) {
					startup.ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
						(ApplicationName ?? "DesignerStudio") + RevisionClass.Major);
				} else {
					startup.ConfigDirectory = Path.Combine(Path.GetDirectoryName(exe.Location), ConfigDirectory);
				}
				
				LoggingService.Debug("ConfigDirectory location has been set to [{0}]", startup.ConfigDirectory);
				
				if(String.IsNullOrEmpty(DomPersistencePath))
					DomPersistencePath = ConfigurationManager.AppSettings["domPersistencePath"];
				
				startup.DomPersistencePath = DomPersistencePath;
				
				if (string.IsNullOrEmpty(startup.DomPersistencePath)) {
					startup.DomPersistencePath = Path.Combine(Path.GetTempPath(), (ApplicationName ?? "DesignerStudio") + RevisionClass.Major + "." + RevisionClass.Minor);
					#if DEBUG
					startup.DomPersistencePath = Path.Combine(startup.DomPersistencePath, "Debug");
					#endif
				} else if (startup.DomPersistencePath == "none") {
					startup.DomPersistencePath = null;
				}
				
				LoggingService.Debug("DomPersistencePath has been set to [{0}]", startup.DomPersistencePath);
				
				if (String.IsNullOrEmpty(AddinsPath))
					AddinsPath = "AddIns";
				
				startup.AddAddInsFromDirectory(Path.Combine(startup.ApplicationRootPath, AddinsPath));
				
				// allows testing addins without having to install them
				foreach (string parameter in SplashScreenForm.GetParameterList()) {
					if (parameter.StartsWith("addindir:", StringComparison.OrdinalIgnoreCase)) {
						startup.AddAddInsFromDirectory(parameter.Substring(9));
					}
				}
				
				if (!String.IsNullOrEmpty(ResourceAssemblyName)) {
					startup.ResourceAssemblyName = ResourceAssemblyName;
					LoggingService.Debug("ResourceAssemblyName has been set to [{0}]", ResourceAssemblyName);
				}
				
				
				LoggingService.Info("Init SharpDevelopHost with AppDomain[{0}]", AppDomain.CurrentDomain.ToString());
				SharpDevelopHost host = new SharpDevelopHost(AppDomain.CurrentDomain, startup);
				
				string[] fileList = SplashScreenForm.GetRequestedFileList();
				if (fileList.Length > 0) {
					if (LoadFilesInPreviousInstance(fileList)) {
						LoggingService.Info("Aborting startup, arguments will be handled by previous instance");
						return;
					}
				}
				
				host.BeforeRunWorkbench += delegate {
					if (SplashScreenForm.SplashScreen != null) {
						SplashScreenForm.SplashScreen.BeginInvoke(new MethodInvoker(SplashScreenForm.SplashScreen.Dispose));
						SplashScreenForm.SplashScreen = null;
					}
				};
				
				WorkbenchSettings workbenchSettings = new WorkbenchSettings();
				workbenchSettings.RunOnNewThread = false;
				for (int i = 0; i < fileList.Length; i++) {
					workbenchSettings.InitialFileList.Add(fileList[i]);
				}
				SDTraceListener.Install();
				host.RunWorkbench(workbenchSettings);
			} finally {
				LoggingService.Info("Leaving RunApplication()");
			}
		}
		
		public bool LoadFilesInPreviousInstance(string[] fileList)
		{
			try {
				foreach (string file in fileList) {
					if (SD.ProjectService.IsSolutionOrProjectFile(FileName.Create(file))) {
						return false;
					}
				}
				return SingleInstanceHelper.OpenFilesInPreviousInstance(fileList);
			} catch (Exception ex) {
				LoggingService.Error(ex);
				return false;
			}
		}
	}
}
