/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 2:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Workbench;

namespace CustomView
{
	/// <summary>
	/// Description of MyCustomView.
	/// </summary>
	public class MyCustomView : AbstractViewContent
	{
		Panel panel = new Panel();
		Label testLabel = new Label();
		
		public MyCustomView()
		{
			testLabel.Text = "Hello World!";
			testLabel.Location = new Point(8, 8);
			panel.Controls.Add(testLabel);
			
			TitleName = "My Custom View 1";
		}
		
		public override object Control {
			get {
				return panel;
			}
		}
		
		public override void Load(OpenedFile file, System.IO.Stream stream)
		{
			
		}
		
		public override void Save(OpenedFile file, System.IO.Stream stream)
		{
			
		}
		
		public override void Dispose()
		{
			testLabel.Dispose();
			panel.Dispose();
		}
	}
}
