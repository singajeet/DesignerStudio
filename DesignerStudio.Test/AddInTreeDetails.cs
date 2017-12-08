/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/10/2017
 * Time: 6:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

using System.Windows.Controls;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf;
using ICSharpCode.SharpDevelop.Workbench;

namespace CustomPad
{
	public class AddInTreeDetails : AbstractPadContent
	{
		StackPanel panel     = new StackPanel();
		RichTextBox tb = new RichTextBox();
		
		public AddInTreeDetails()
		{			
			tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			tb.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			panel.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			
			if (tb.ActualHeight < 200)
				tb.Height = 300;
			
			panel.Children.Add(tb);
			tb.AppendText(SD.AddInTree.GetInstalledAddInsListAsString());
			
			this.PadDescriptor.DefaultPosition = ICSharpCode.SharpDevelop.DefaultPadPositions.Left;
		}
		
		// return type is object: both WPF and Windows Forms controls are supported
		public override object Control {
			get {
				return panel;
			}
		}
	}
}