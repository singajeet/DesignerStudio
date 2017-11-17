/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 10:02 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.SharpDevelop.Services.Gui.Wpf;

namespace ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf
{
	/// <summary>
	/// Description of ExtTreeNode.
	/// </summary>
	public class ExtTreeNode : TreeNode, IDisposable, IClipboardHandler, IComparable
	{
		string contextmenuAddinTreePath = null;
		protected bool isInitialized    = false;		
		
		public static readonly RoutedEvent CollapsingEvent =
        EventManager.RegisterRoutedEvent("Collapsing",
        RoutingStrategy.Bubble, typeof(RoutedEventHandler),
        typeof(ExtTreeNode));

	    public static readonly RoutedEvent ExpandingEvent =
	        EventManager.RegisterRoutedEvent("Expanding",
	        RoutingStrategy.Bubble, typeof(RoutedEventHandler),
	        typeof(ExtTreeNode));
    
	    public event RoutedEventHandler Collapsing{
	    	add { AddHandler(CollapsingEvent, value); }
	    	remove { RemoveHandler(CollapsingEvent, value); }
	    }
		
	    public event RoutedEventHandler Expanding{
	    	add { AddHandler(ExpandingEvent, value); }
	    	remove { RemoveHandler(ExpandingEvent, value); }
	    }

		protected virtual void OnExpanding(RoutedEventArgs routedEventArgs)
		{
			RaiseEvent(routedEventArgs);
		}
	    protected override void OnExpanded(RoutedEventArgs e)
		{
	    	OnExpanding(new RoutedEventArgs(ExpandingEvent, this));
			base.OnExpanded(e);
		}

		protected virtual void OnCollapsing(RoutedEventArgs routedEventArgs)
		{
			RaiseEvent(routedEventArgs);
		}
	    protected override void OnCollapsed(RoutedEventArgs e)
		{
	    	OnCollapsing(new RoutedEventArgs(CollapsingEvent, this));
			base.OnCollapsed(e);
		}
			    
		public bool IsInitialized {
			get {
				return isInitialized;
			}
		}
		
		public virtual string ContextmenuAddinTreePath {
			get {
				return contextmenuAddinTreePath;
			}
			set {
				contextmenuAddinTreePath = value;
			}
		}
		
		TreeNode internalParent;
		
		public new TreeNode ParentItem {
			get {
				return internalParent;
			}
		}
		
		public void AddTo(TreeNode node)
		{
			internalParent = node;
			AddTo(node.Items);
		}
		public void AddTo(System.Windows.Controls.TreeView view)
		{
			internalParent = null;
			AddTo(view.Items);
		}
		
		public void Insert(int index, TreeNode parentNode)
		{
			internalParent = parentNode;
			parentNode.Items.Insert(index, this);
			Refresh();
		}
		
		public void Insert(int index, System.Windows.Controls.TreeView view)
		{
			internalParent = null;
			view.Items.Insert(index, this);
			Refresh();
		}
		
		void AddTo(ItemCollection nodes)
		{
			nodes.Add(this);
			Refresh();
		}
		
		protected virtual void Initialize()
		{
		}
		
		public void PerformInitialization()
		{
			if (!isInitialized) {
				Initialize();
				isInitialized = true;
			}
		}
		
		
		
		public virtual void ActivateItem()
		{
			this.ExpandSubtree();
		}
		
		public virtual void CheckedChanged()
		{
		}
		
		public virtual void Refresh()
		{
			//SetIcon(image);
//			foreach (TreeNode node in Items) {
//				if (node is ExtTreeNode) {
//					((ExtTreeNode)node).Refresh();
//				}
//			}
		}
		
		#region Label edit
		protected bool canLabelEdit = false;
		public virtual bool CanLabelEdit {
			get {
				return canLabelEdit;
			}
		}
		
		/// <summary>
		/// This method is before a label edit starts.
		/// </summary>
		public virtual void BeforeLabelEdit()
		{
		}
		
		/// <summary>
		/// This method is called when a label edit has finished.
		/// The New Name is the 'new name' the user has given the node. The
		/// node must handle the name change itself.
		/// </summary>
		public virtual void AfterLabelEdit(string newName)
		{
			throw new NotImplementedException();
		}
		
		#endregion
		public virtual bool Visible {
			get {
				return this.Visible;
			}
		}
		
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragOver(e);
			ExtTreeNode node = e.Source as ExtTreeNode;
			if (node != null) {
				IDataObject data = e.Data;
				if (data != null) {
					DragDrop.DoDragDrop(this, data, DragDropEffects.All);
				}
			}
		}
		
		public IEnumerable<ExtTreeNode> AllNodes {
			get {
				foreach (ExtTreeNode n in Items) {
					yield return n;
				}
				foreach (ExtTreeNode n in invisibleNodes) {
					yield return n;
				}
			}
		}
		
		protected List<ExtTreeNode> invisibleNodes = new List<ExtTreeNode>();
		public virtual void UpdateVisibility()
		{
			for (int i = 0; i < invisibleNodes.Count;) {
				if (invisibleNodes[i].Visible) {
					invisibleNodes[i].AddTo(this);
					invisibleNodes.RemoveAt(i);
					continue;
				}
				++i;
			}
			
			foreach (TreeNode node in Items) {
				if (node is ExtTreeNode) {
					ExtTreeNode extTreeNode = (ExtTreeNode)node;
					if (!extTreeNode.Visible) {
						invisibleNodes.Add(extTreeNode);
					}
				}
			}
			
			foreach (TreeNode node in invisibleNodes) {
				Items.Remove(node);
			}
			
			foreach (TreeNode node in Items) {
				if (node is ExtTreeNode) {
					((ExtTreeNode)node).UpdateVisibility();
				}
			}
		}
		
		public ExtTreeNode()
		{
		}

		#region IDisposable implementation
		bool isDisposed = false;
		
		public bool IsDisposed {
			get {
				return isDisposed;
			}
		}
		
		public void Dispose()
		{
			isDisposed = true;
			foreach (TreeNode node in Items) {
				if (node is IDisposable) {
					((IDisposable)node).Dispose();
				}
			}
		}

		#endregion
		
		protected int sortOrder = 0;
		public virtual int SortOrder {
			get {
				return sortOrder;
			}
		}
		
		public virtual string CompareString {
			get {
				return Header.ToString();
			}
		}

		#region IComparable implementation
		public int CompareTo(object obj)
		{
			ExtTreeNode extTreeNode = obj as ExtTreeNode;
			
			if (extTreeNode == null) {
				TreeNode treeNode = obj as TreeNode;
				return string.Compare(this.CompareString, treeNode.Header.ToString(), StringComparison.CurrentCulture);
			}
			return string.Compare(this.CompareString, extTreeNode.CompareString, StringComparison.CurrentCulture);
		}
		
		#endregion		
		int GetInsertionIndex(ItemCollection nodes, System.Windows.Controls.TreeView treeView)
		{
			if (treeView == null) {
				return nodes.Count;
			}
			
			for (int i = 0; i < nodes.Count; ++i) {
				if (this.CompareTo((ExtTreeNode)nodes[i]) < 0) {
					return i;
				}
			}
			
			return nodes.Count;
		}
		
		int GetInsertionIndex(ItemCollection nodes)
		{
			for (int i = 0; i < nodes.Count; ++i) {
				if (this.CompareTo((ExtTreeNode)nodes[i]) < 0) {
					return i;
				}
			}			
			return nodes.Count;
		}

		#region IClipboardHandler implementation
		bool doPerformCut = false;
		public virtual bool DoPerformCut {
			get {
				ExtTreeNode parent = ParentItem as ExtTreeNode;
				return parent == null ? doPerformCut : doPerformCut | parent.DoPerformCut;
			}
			set {
				this.doPerformCut = value;
				if (this.doPerformCut) {
					//((ExtTreeView)System.Windows.Controls.TreeView).CutNodes.Add(this);					
				}
				Refresh();
			}
		}
		
		/// <summary>
		/// Inserts this node into the specified TreeView at the position
		/// determined by the comparer of the TreeView, assuming that
		/// all other immediate child nodes of the TreeView are in sorted order.
		/// </summary>
		public void InsertSorted(System.Windows.Controls.TreeView treeView)
		{
			this.Insert(this.GetInsertionIndex(treeView.Items, treeView), treeView);
		}
		
		/// <summary>
		/// Inserts this node into the specified <paramref name="parentNode"/>
		/// at the position determined by the comparer
		/// of the TreeView which contains the <paramref name="parentNode"/>,
		/// assuming that all other immediate child nodes of the <paramref name="parentNode"/>
		/// are in sorted order.
		/// </summary>
		public void InsertSorted(TreeNode parentNode)
		{
			this.Insert(this.GetInsertionIndex(parentNode.Items), parentNode);
		}

		public void Cut()
		{
			throw new NotImplementedException();
		}

		public void Copy()
		{
			throw new NotImplementedException();
		}

		public void Paste()
		{
			throw new NotImplementedException();
		}

		public void Delete()
		{
			throw new NotImplementedException();
		}

		public void SelectAll()
		{
			throw new NotImplementedException();
		}

		public bool EnableCut {
			get {
				return false;
			}
		}

		public bool EnableCopy {
			get {
				return false;
			}
		}

		public bool EnablePaste {
			get {
				return false;
			}
		}

		public bool EnableDelete {
			get {
				return false;
			}
		}

		public bool EnableSelectAll {
			get {
				return false;
			}
		}

		#endregion
	}
}
