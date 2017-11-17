/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/4/2017
 * Time: 2:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;
using DesignerStudio.Startup;

namespace DesignerStudio.Startup
{
	/// <summary>
	/// Description of Setup.
	/// </summary>
	public class Setup : ISetup
	{
		SharpDevelopMain _main;
		string[] commandLineArgs;
		static Setup _setup;
		
		public static Setup Instance{
			get {
				if (_setup == null)
					_setup = new Setup();
				return _setup;
			}
		}
		
		public Setup CreateApp()
		{
			_main = SharpDevelopMain.Instance;
			return this;
		}
		
		public Setup CreateApp(string appName)
		{
			return CreateApp().SetApplicationName(appName);
		}
		
		public Setup SetAppPath(string path)
		{
			_main.AppPath = path;
			return this;
		}
		
		public Setup UseExceptionBox()
		{
			_main.UseExceptionBox = true;
			return this;			
		}
		
		public Setup UseCommandLineArgs(string[] args)
		{
			_main.CommandLineArgs = args;
			return this;
		}
		
		public Setup NoLogo()
		{
			_main.NoLogo = true;
			return this;
		}
		
		public Setup SetApplicationName(string appName){
			_main.ApplicationName = appName;
			return this;
		}
		
		public Setup SetDomPersistancePath(string path)
		{
			_main.DomPersistencePath = path;
			return this;
		}
		
		public Setup SetAddInsPath(string path)
		{
			_main.AddinsPath = path;
			return this;
		}
		
		public Setup SetConfigDirectoryPath(string path)
		{
			_main.ConfigDirectory = path;
			return this;
		}
		
		public Setup SetResourceAssemblyName(string assyName)
		{
			_main.ResourceAssemblyName = assyName;
			return this;
		}
		
		public void Run()
		{
			if (_main.UseExceptionBox) {
				try{
					_main.Run();
				} catch (Exception ex) {
					try{
					_main.HandleMainException(ex);
					} catch (Exception loadError) {
						MessageBox.Show(loadError.ToString(), "Critical error (Logging service defect?)");
					}
				}
			} else {
				_main.Run();
			}
		}
		
	}
}
