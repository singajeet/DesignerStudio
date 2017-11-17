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
using ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf;
using ICSharpCode.SharpDevelop.Workbench;

namespace CustomPad
{
	public class MyCustomPad : AbstractPadContent
	{
		StackPanel panel     = new StackPanel();
		//Label testLabel = new Label();
		ExtTreeView treeview;
		
		public MyCustomPad()
		{
//			testLabel.Text     = "Hello World!";
//			testLabel.Location = new Point(8, 8);
//			panel.Controls.Add(testLabel);
			
			treeview = new ExtTreeView();
			ExtTreeNode treeNode1 = new ExtTreeNode();
			treeNode1.Text = "Node1";
			treeNode1.AddTo(treeview);
			ExtTreeNode treeNode2 = new ExtTreeNode();
			treeNode2.Text = "Node2";
			treeNode2.AddTo(treeNode1);
			ExtTreeNode treeNode3 = new ExtTreeNode();
			treeNode3.Text = "Node3";
			treeNode3.AddTo(treeNode2);
			panel.Children.Add(treeview);
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