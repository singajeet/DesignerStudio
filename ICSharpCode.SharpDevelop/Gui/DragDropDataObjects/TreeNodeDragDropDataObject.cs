/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/18/2017
 * Time: 10:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf;

namespace ICSharpCode.SharpDevelop.Services.Gui.DragDropDataObjects
{
	/// <summary>
	/// Description of TreeNodeDragDropDataObject.
	/// </summary>
	[Serializable]
	public class TreeNodeDragDropDataObject
	{
		private string _name;
		private string _iconPath;
		private string _hintText;
		private TreeNode _draggedNode;
		private TreeNode _parentNode;
		private ItemCollection _childItems;
		
		public TreeNodeDragDropDataObject(TreeNode node)
		{
			_name = node.Text;
			_iconPath = node.IconPath;
			_hintText = node.HintText;
			_draggedNode = node;
			_parentNode = node.Parent as TreeNode;
			_childItems = node.Items;
		}
		
		
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		
		public string IconPath{
			get { return _iconPath; }
			set { _iconPath = value; }
		}
		
		public string HintText{
			get { return _hintText; }
			set { _hintText = value; }
		}
		
		public TreeNode DraggedNode{
			get { return _draggedNode; }
			set { _draggedNode = value; }
		}
		
		public TreeNode ParentNode{
			get { return _parentNode; }
			set { _parentNode = value; }
		}
		
		public ItemCollection ChildItems{
			get { return _childItems; }
			set { _childItems = value; }
		}
	}
}
