// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf;
//using System.Windows.Forms;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.Core.Presentation;

namespace ICSharpCode.SharpDevelop.Project
{
	/// <summary>
	/// Description of ProjectBrowserPanel.
	/// </summary>
	public class ProjectBrowserPanel : System.Windows.Controls.UserControl //System.Windows.Forms.UserControl
	{
		//ToolStrip             toolStrip;
		ToolBar toolStrip;
		ProjectBrowserControl projectBrowserControl;
		object[]       standardItems;
		//ItemCollection standardItems;		
		DockPanel dockPanel;
		
		public AbstractProjectBrowserTreeNode SelectedNode {
			get {
				return projectBrowserControl.SelectedNode;
			}
		}
		
		public AbstractProjectBrowserTreeNode RootNode {
			get {
				return projectBrowserControl.RootNode;
			}
		}
		
		public ProjectBrowserControl ProjectBrowserControl {
			get {
				return projectBrowserControl;
			}
		}
		
		public ProjectBrowserPanel()
		{
			projectBrowserControl      = new ProjectBrowserControl();
			projectBrowserControl.VerticalAlignment = VerticalAlignment.Stretch;
			projectBrowserControl.HorizontalAlignment = HorizontalAlignment.Stretch;
			
			DockPanel.SetDock(projectBrowserControl, Dock.Bottom);
			//Controls.Add(projectBrowserControl);
			dockPanel = new DockPanel();
			dockPanel.LastChildFill = true;
			
			
			if (SD.AddInTree.GetTreeNode("/SharpDevelop/Pads/ProjectBrowser/ToolBar/Standard", false) != null) {
				toolStrip = ToolBarService.CreateToolBar(this, this, "/SharpDevelop/Pads/ProjectBrowser/ToolBar/Standard");
				standardItems = new object[toolStrip.Items.Count];
				toolStrip.Items.CopyTo(standardItems, 0);
				
				DockPanel.SetDock(toolStrip, Dock.Top);
				dockPanel.Children.Add(toolStrip);
				Style tbStyle = Application.Current.TryFindResource("MaterialDesignToolBar") as Style;
				if (tbStyle != null)
					toolStrip.Style = tbStyle;
			}
			
			dockPanel.Children.Add(projectBrowserControl);			
			this.Content = dockPanel;
			
			
			projectBrowserControl.TreeView.SelectedItemChanging += projectBrowserControl_TreeView_SelectedItemChanging;
		}

		void projectBrowserControl_TreeView_SelectedItemChanging(object sender, RoutedEventArgs e)
		{
			//UpdateToolStrip(e.ChangedNode as AbstractProjectBrowserTreeNode);
		}
		
		void UpdateToolStrip(AbstractProjectBrowserTreeNode node)
		{
			if (toolStrip == null) return;
			toolStrip.Items.Clear();
			toolStrip.Items.AddRange(standardItems);
			//SD.WinForms.ToolbarService.UpdateToolbar(toolStrip);
			MenuService.UpdateText(toolStrip.Items);
			if (node != null && node.ToolbarAddinTreePath != null) {
				toolStrip.Items.Add(new Separator());
				toolStrip.Items.AddRange(SD.WinForms.ToolbarService.CreateToolStripItems(node.ToolbarAddinTreePath, node, false));
			}
		}
		
		public void ViewSolution(ISolution solution)
		{
			//UpdateToolStrip(null);
			projectBrowserControl.ViewSolution(solution);
		}
		
		/// <summary>
		/// Writes the current view state into the memento.
		/// </summary>
		public void StoreViewState(Properties memento)
		{
			projectBrowserControl.StoreViewState(memento);
		}
		
		/// <summary>
		/// Reads the view state from the memento.
		/// </summary>
		public void ReadViewState(Properties memento)
		{
			projectBrowserControl.ReadViewState(memento);
		}
		
		public void Clear()
		{
			projectBrowserControl.Clear();
			//UpdateToolStrip(null);
		}
		
		public void SelectFile(string fileName)
		{
			projectBrowserControl.SelectFile(fileName);
		}
	}
	
	public static class ItemCollectionExtension
	{
		public static void AddRange(this ItemCollection collection, object[] items)
		{
			foreach (object item in items) {
				collection.Add(item);
			}
		}
	}
}
