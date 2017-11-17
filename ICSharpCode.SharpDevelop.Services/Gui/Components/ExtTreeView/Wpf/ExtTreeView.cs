/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 10:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.Core.Presentation;
using ICSharpCode.SharpDevelop.Services.Utils;

namespace ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf
{
	/// <summary>
	/// Description of ExtTreeView.
	/// </summary>
	public class ExtTreeView : System.Windows.Controls.TreeView
	{
		List<ExtTreeNode> cutNodes = new List<ExtTreeNode>();
		bool isSorted = false;
		bool allowSort = true;
		
		 public static readonly RoutedEvent SelectedItemChangingEvent =
	        EventManager.RegisterRoutedEvent("SelectedItemChanging",
	        RoutingStrategy.Bubble, typeof(RoutedEventHandler),
	        typeof(ExtTreeView));
		
		 public event RoutedEventHandler SelectedItemChanging{
		 	add { AddHandler(SelectedItemChangingEvent, value); }
			remove { RemoveHandler(SelectedItemChangingEvent, value); }
		 }
		 
		public List<ExtTreeNode> CutNodes {
			get {
				return cutNodes;
			}
		}
		
		/// <summary>
		/// Gets/Sets whether the ExtTreeView does its own sorting.
		/// </summary>
		public bool IsSorted {
			get {
				return isSorted;
			}
			set {
				isSorted = value;
			}
		}
		
		public bool AllowSort{
			get {
				return allowSort;
			}
			set {
				allowSort = value;
			}
		}
		
		public ExtTreeView()
		{
			
		}
		
		protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			Sort();
		}
		
		protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Enter) {
				ExtTreeNode node = SelectedItem as ExtTreeNode;
				if(node != null)
					node.ActivateItem();				
			}
			e.Handled = true;
		}
		
		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			ExtTreeNode node = e.Source as ExtTreeNode;
			if (node != null)
				node.ActivateItem();
		}
		
		bool canClearSelection = true;
		
		/// <summary>
		/// Gets/Sets whether the user can clear the selection by clicking in the empty area.
		/// </summary>
		public bool CanClearSelection {
			get {
				return canClearSelection;
			}
			set {
				canClearSelection = value;
			}
		}
		
		int mouseClickNum; // 0 if mouse button is not pressed, otherwise click number (1=normal, 2=double click)
		
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			mouseClickNum = e.ClickCount;
			TreeNode item = e.Source as TreeNode;
			if (item != null) {
				if (SelectedItem != item) {
					//SelectedItem = item;
					
				}
			} else {
				if (CanClearSelection) {
				}
					//SelectedItem = null;
			}
		}
		
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			mouseClickNum = 0;
		}
		
		protected override void OnSelectedItemChanged(System.Windows.RoutedPropertyChangedEventArgs<object> e)
		{	
			OnSelectedItemChanging(new TreeNodeChangingRoutedEventArgs(e.OldValue as TreeNode, SelectedItemChangingEvent, this));
			base.OnSelectedItemChanged(e);
		}

		void OnSelectedItemChanging(TreeNodeChangingRoutedEventArgs routedEventArgs)
		{
			ExtTreeNode node = routedEventArgs.ChangingNode as ExtTreeNode;
			if (node != null) {
				node.ContextMenu = MenuService.CreateContextMenu(node, node.ContextmenuAddinTreePath);
			}
		}
		
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			e.Effects = DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.None;
		}
		
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			ExtTreeNode node = e.Source as ExtTreeNode;
			
			if (node != null) {
				HandleDragOver(e, node);
				
				if (e.Effects != DragDropEffects.None) {
					//SelectedItem = node;
				}
			}
		}

		void HandleDragOver(DragEventArgs e, ExtTreeNode node)
		{
			DragDropEffects effect = DragDropEffects.None;
			
			if (e.KeyStates == DragDropKeyStates.ControlKey) {
				effect = DragDropEffects.Copy;
			} else {
				effect = DragDropEffects.Move;
			}
		}
		
		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			ExtTreeNode node = e.Source as ExtTreeNode;
			
			if (node != null) {
				HandleDragOver(e, node);
				
				if (e.Effects != DragDropEffects.None) {
					DragDrop.DoDragDrop(node, e.Data, e.Effects);
					SortParentNodes(node);
				}
			}
		}
		
		void DeleteNode(ExtTreeNode node)
		{
			if (node == null) {
				return;
			}
			
			if (node.EnableDelete) {
				node.Visibility = Visibility.Visible;
				node.IsSelected = true;
				//SelectedItem = node;
				node.Delete();
			}
		}
		
		public new void Sort()
		{
			SortNodes(Items, true);
			IsSorted = true;
		}

		public void SortNodes(ItemCollection nodes, bool recursive)
		{
			if (!AllowSort) {
				return;
			}
			nodes.BubbleSort();
			
			if (recursive) {
				foreach (object item in nodes) {
					SortNodes(((ExtTreeNode)item).Items, true);
				}
			}
		}		

		public void ClearCutNodes()
		{
			foreach (ExtTreeNode node in CutNodes) {
				node.DoPerformCut = false;
			}
			CutNodes.Clear();			
		}		
		
		public void Clear()
		{
			this.Items.Clear();
		}
		
		
		private void SortParentNodes(TreeNode treeNode)
		{
			TreeNode parent = ((ExtTreeNode)treeNode).ParentItem;
			SortNodes((parent == null) ? Items : parent.Items, false);
		}
	}
	
	public class TreeNodeChangingRoutedEventArgs : RoutedEventArgs
	{
		public TreeNode ChangingNode;
		
		public TreeNodeChangingRoutedEventArgs(TreeNode changingNode, RoutedEvent eventObject, object source) :
			base(eventObject, source)
		{
			ChangingNode = changingNode;
		}
	}
}
