/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/4/2017
 * Time: 5:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DesignerStudio.Startup
{
	/// <summary>
	/// Description of Program.
	/// </summary>
	public static class Program
	{
		[STAThread()]
		public static void Main(string[] args)
		{
			Setup.Instance
				.CreateApp()
				.SetAppPath(@"C:\Users\Admin\Documents\SharpDevelop Projects\DesignerStudio\DesignerStudio.Startup\bin\Debug")
				.UseCommandLineArgs(args)
				.SetDomPersistancePath(@"Dom")
				.SetAddInsPath(@"C:\Users\Admin\Documents\SharpDevelop Projects\DesignerStudio\DesignerStudio.Startup\bin\Debug")
				.SetConfigDirectoryPath(@"Settings")
				.AllowUserAddIns(false)
				.Run();
		}
	}
}
