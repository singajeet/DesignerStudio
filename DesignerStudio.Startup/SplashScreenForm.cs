/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 10/30/2017
 * Time: 11:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DesignerStudio.Startup
{
	/// <summary>
	/// Description of SplashScreenForm.
	/// </summary>
	public sealed class SplashScreenForm : Form
	{
		static SplashScreenForm splashScreen;
		static List<string> requestedFileList = new List<string>();
		static List<string> parameterList = new List<string>();
		Bitmap bitmap;
		
		public static SplashScreenForm SplashScreen {
			get {
				return splashScreen;
			}
			set {
				splashScreen = value;
			}
		}
		
		public SplashScreenForm()
		{
			const string versionText = "Designer Studio"
				+ " " + RevisionClass.FullVersion
				#if DEBUG
				+ " (debug)"
				#endif
				;
			
			FormBorderStyle = FormBorderStyle.None;
			StartPosition   = FormStartPosition.CenterScreen;
			ShowInTaskbar   = false;
			// Stream must be kept open for the lifetime of the bitmap
			bitmap = new Bitmap(typeof(SplashScreenForm).Assembly.GetManifestResourceStream("DesignerStudio.Startup.Resources.SplashScreen.jpg"));
			this.ClientSize = bitmap.Size;
			using (Font font = new Font("Sans Serif", 4)) {
				using (Graphics g = Graphics.FromImage(bitmap)) {
					g.DrawString(versionText, font, Brushes.Black, 166 - 3 * versionText.Length, 142);
				}
			}
			BackgroundImage = bitmap;
		}
		
		public static void ShowSplashScreen()
		{
			splashScreen = new SplashScreenForm();
			splashScreen.Show();
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (bitmap != null) {
					bitmap.Dispose();
					bitmap = null;
				}
			}
			base.Dispose(disposing);
		}
		
		public static string[] GetParameterList()
		{
			return parameterList.ToArray();
		}
		
		public static string[] GetRequestedFileList()
		{
			return requestedFileList.ToArray();
		}
		
		public static void SetCommandLineArgs(string[] args)
		{
			requestedFileList.Clear();
			parameterList.Clear();
			
			foreach (string arg in args) {
				if (arg.Length == 0) continue;
				if (arg[0] == '-' || arg[0] == '/') {
					int markerLength = 1;
					
					if (arg.Length >= 2 && arg[0] == '-' && arg[1] == '-') {
						markerLength = 2;
					}
					
					string param = arg.Substring(markerLength);
					// The SharpDevelop AddIn project template uses /addindir:"c:\temp\"
					// but that actually means the last quote is escaped.
					// This HACK makes this work anyways by replacing the trailing quote
					// with a backslash:
					if (param.EndsWith("\"", StringComparison.Ordinal))
						param = param.Substring(0, param.Length - 1) + "\\";
					parameterList.Add(param);
				} else {
					requestedFileList.Add(arg);
				}
			}
		}
	}
}
