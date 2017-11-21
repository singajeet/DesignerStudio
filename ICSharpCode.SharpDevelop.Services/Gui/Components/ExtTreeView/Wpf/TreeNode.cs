/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 10:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.SharpDevelop.Services.Gui.DragDropDataObjects;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace ICSharpCode.SharpDevelop.Services.Gui.Components.ExtTreeView.Wpf
{
	/// <summary>
	/// Description of TreeNode.
	/// </summary>
	public class TreeNode : TreeViewItem, INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<NodeChangingEventArgs> NodeTextChanging;
		public event EventHandler<NodeChangedEventArgs> NodeTextChanged;
		public event EventHandler<TreeNodeDragDropEventArgs> TreeNodeDragStarted;
		public event EventHandler<TreeNodeDragDropEventArgs> GetDragFeedback;
		public event EventHandler<TreeNodeDragDropEventArgs> TreeNodeDropCompleted;
	
		#endregion

		StackPanel panel = new StackPanel();
		Viewbox viewbox = new Viewbox();
		TextBlock textLabelCtl = new TextBlock();
		TextBox textBoxCtl = new TextBox();
		
		TreeNodeDragDropDataObject _data;
		
		public static readonly DependencyProperty hintTextProperty =
			DependencyProperty.Register("HintText", typeof(string), typeof(TreeNode),
				new FrameworkPropertyMetadata());
		
		public string HintText{
			get { return ((string)GetValue(hintTextProperty)) ?? "Name"; }
			set { SetValue(hintTextProperty, value); 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("HintText"));
			}
		}
		
		public static readonly DependencyProperty iconPathProperty =
			DependencyProperty.Register("IconPath", typeof(string), typeof(TreeNode),
				new FrameworkPropertyMetadata());
		
		public string IconPath{
			get { return (string)GetValue(iconPathProperty); }
			set { SetValue(iconPathProperty, value); 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("IconPath"));
			}
		}
		
		public TreeNode Clone()
		{
			TreeNode newNode = new TreeNode();
			newNode.HintText = this.HintText;
			newNode.IconPath = this.IconPath;
			newNode.Text = this.Text;
			
			return newNode;
		}
		
		public TreeNode DeepClone()
		{
			TreeNode newNode = Clone();
			foreach (TreeNode item in this.Items) {
				newNode.Items.Add(item.Clone());
			}
			return newNode;
		}
		
		/// <summary>
		/// Declared for compatibility and should be removed in future
		/// </summary>
		/// <param name="iconPath"></param>
		[Obsolete("SetIcon() is obsolete, use IconPath property instead")]
		public void SetIcon(string iconPath)
		{
			if (iconPath == null) {
				return;
			}
			this.IconPath = iconPath;
		}
		
		public static readonly DependencyProperty isInEditModeProperty =
			DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(TreeNode),
			                            new FrameworkPropertyMetadata());
		
		public bool IsInEditMode {
			get { return (bool)GetValue(isInEditModeProperty); }
			set { SetValue(isInEditModeProperty, value);
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("IsInEditMode"));
			}
		}
		
		public static readonly DependencyProperty textProperty = 
			DependencyProperty.Register("Text", typeof(string), typeof(TreeNode),
				new FrameworkPropertyMetadata());
		
		public string Text{
			get { return ((string)GetValue(textProperty)) ?? "Empty"; }
			set { SetValue(textProperty, value); 
				textLabelCtl.Text = value;
				textBoxCtl.Text = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Text"));
			}
		}
		
		string oldText;
		
		public TreeNode()
		{
			panel.Orientation = Orientation.Horizontal;
			panel.Children.Add(viewbox);			
			panel.Children.Add(textLabelCtl);
			panel.Children.Add(textBoxCtl);
			
			if (this.IconPath != null) {
				if (this.IconPath.Contains(";")) {
					string[] packIconValues = IconPath.Split(';');
					string packIconType = packIconValues[0];
					string packIconKind = packIconValues[1];
							
					switch (packIconType) {
						case "PackIconMaterial":
							PackIconMaterial iconMat = new PackIconMaterial();
							((PackIconMaterial)iconMat).Kind = (PackIconMaterialKind)Enum
													.Parse(typeof(PackIconMaterialKind),
								packIconKind);
						
							viewbox.Child = iconMat;
							break;
						case "PackIconMaterialLight":
							PackIconMaterialLight iconMatLight = new PackIconMaterialLight();
							((PackIconMaterialLight)iconMatLight).Kind = (PackIconMaterialLightKind)Enum
													.Parse(typeof(PackIconMaterialLightKind),
								packIconKind);
						
							viewbox.Child = iconMatLight;
							break;
						case "PackIconModern":
							PackIconModern iconModern = new PackIconModern();
							((PackIconModern)iconModern).Kind = (PackIconModernKind)Enum
													.Parse(typeof(PackIconModernKind),
								packIconKind);
						
							viewbox.Child = iconModern;
							break;
						case "PackIconOcticons":
							PackIconOcticons iconOcti = new PackIconOcticons();
							((PackIconOcticons)iconOcti).Kind = (PackIconOcticonsKind)Enum
													.Parse(typeof(PackIconOcticonsKind),
								packIconKind);
						
							viewbox.Child = iconOcti;
							break;
						case "PackIconSimpleIcons":
							PackIconSimpleIcons iconSimple = new PackIconSimpleIcons();
							((PackIconSimpleIcons)iconSimple).Kind = (PackIconSimpleIconsKind)Enum
													.Parse(typeof(PackIconSimpleIconsKind),
								packIconKind);
						
							viewbox.Child = iconSimple;
							break;
						case "PackIconEntypo":
							PackIconEntypo iconEnt = new PackIconEntypo();
							((PackIconEntypo)iconEnt).Kind = (PackIconEntypoKind)Enum
													.Parse(typeof(PackIconEntypoKind),
								packIconKind);
						
							viewbox.Child = iconEnt;
							break;
						case "PackIconFontAwesome":
							PackIconFontAwesome icon = new PackIconFontAwesome();
							((PackIconFontAwesome)icon).Kind = (PackIconFontAwesomeKind)Enum
													.Parse(typeof(PackIconFontAwesomeKind),
								packIconKind);
						
							viewbox.Child = icon;
							break;
					}
			
					viewbox.Margin = new Thickness(0, 0, 8, 0);
				}
			}
			
			Style materialDesignTreeViewItem = Application.Current.TryFindResource("MaterialDesignTreeViewItem") as Style;
			if (materialDesignTreeViewItem != null)
				this.Style = materialDesignTreeViewItem;
			
			Style materialDesignTextbox = Application.Current.TryFindResource("MaterialDesignFloatingHintTextBox") as Style;
			if (materialDesignTextbox != null) {
				textBoxCtl.Style = materialDesignTextbox;
				HintAssist.SetHint(this, "(" + HintText + ")");
			}			
			
			this.Header = panel;
			
			textBoxCtl.Visibility = Visibility.Collapsed;
			textBoxCtl.LostFocus+= textBox_LostFocus;
			textBoxCtl.IsVisibleChanged+= textBox_IsVisibleChanged;
			textBoxCtl.KeyDown+= textBox_KeyDown;
			
			textLabelCtl.MouseLeftButtonDown += text_MouseLeftButtonDown;
			textLabelCtl.KeyDown += text_KeyDown;
		}
		
		public void SetEditMode()
		{
			IsInEditMode = true;
			textLabelCtl.Visibility = Visibility.Collapsed;
			textBoxCtl.Visibility = Visibility.Visible;	
		}

		void text_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F2) {
				if (this.IsSelected) {
					SetEditMode();
				}				
			}
		}
		void text_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (this.IsSelected) {
				SetEditMode();
				e.Handled = true;
			}
		}

		void textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Enter) {
				IsInEditMode = false;
				textLabelCtl.Visibility = Visibility.Visible;
				textBoxCtl.Visibility = Visibility.Collapsed;
				Text = textLabelCtl.Text = textBoxCtl.Text;
				if(NodeTextChanged != null)
					NodeTextChanged(this, new NodeChangedEventArgs(Text));
			}
			
			if (e.Key == Key.Escape) {
				var tb = sender as TextBox;
				tb.Text = oldText;
				IsInEditMode = false;
				textLabelCtl.Visibility = Visibility.Visible;
				textBoxCtl.Visibility = Visibility.Collapsed;
			}
		}
		void textBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var tb = sender as TextBox;
			if (tb.IsVisible) {
				tb.Focus();
				tb.SelectAll();
				oldText = tb.Text;
				if(NodeTextChanging != null)
					NodeTextChanging(this, new NodeChangingEventArgs(oldText));
			}
		}
		void textBox_LostFocus(object sender, RoutedEventArgs e)
		{
			IsInEditMode =  false;
			textLabelCtl.Visibility = Visibility.Visible;
			textBoxCtl.Visibility = Visibility.Collapsed;
			Text = textLabelCtl.Text = textBoxCtl.Text;
			if(NodeTextChanged != null)
				NodeTextChanged(this, new NodeChangedEventArgs(Text));
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			if (e.LeftButton == MouseButtonState.Pressed) {
				
				if(_data == null)
					_data = new TreeNodeDragDropDataObject(this);
				DragDrop.DoDragDrop(this, _data, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.None);
				
				if (TreeNodeDragStarted != null) {
					TreeNodeDragStarted(this, new TreeNodeDragDropEventArgs(_data, e));
				}
			}
		}
		
		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (e.AllowedEffects.HasFlag(DragDropEffects.Move) || e.AllowedEffects.HasFlag(DragDropEffects.Copy)) {
			
			if (e.Data.GetDataPresent("ICSharpCode.SharpDevelop.Services.Gui.DragDropDataObjects.TreeNodeDragDropDataObject")
			   || e.Data.GetDataPresent(DataFormats.FileDrop)) {
					
				TreeNodeDragDropDataObject draggedNodeData = (TreeNodeDragDropDataObject)e.Data.GetData("ICSharpCode.SharpDevelop.Services.Gui.DragDropDataObjects.TreeNodeDragDropDataObject");
				
				
					if (e.KeyStates == DragDropKeyStates.ControlKey) {
						e.Effects = DragDropEffects.Copy;
						
						//Make copy of the dragged node
						TreeNode copiedNode = (TreeNode)draggedNodeData.DraggedNode.Clone();						
						
						//Add copied node as child of the current node
						this.AddChild(copiedNode);		
						
						if (TreeNodeDropCompleted != null) {
							TreeNodeDropCompleted(this, new TreeNodeDragDropEventArgs(_data, e, this, copiedNode));
						}						
						
						e.Handled = true;
					} else {						
					
						TreeNode draggedNode = draggedNodeData.DraggedNode;
						
						if (draggedNode != this) {
							//If item being dragged has parent then unlink it from that parent
							if (draggedNode.Parent != null) {
							
								//Parent is an TreeNode or TreeViewItem
								if (draggedNode.Parent is TreeNode) {
									TreeNode parent = draggedNode.Parent as TreeNode;
									parent.Items.Remove(draggedNode);
								}
							
								//Parent is the TreeView itself
								if (draggedNode.Parent is System.Windows.Controls.TreeView) {
									//Need to get TreeView instance and remove node from it 
								}
							}
						
							//Add dragged node as child of the current node
							this.Items.Add(draggedNode);
							
							e.Effects = DragDropEffects.Move;
						
							if (TreeNodeDropCompleted != null) {
								TreeNodeDropCompleted(this, new TreeNodeDragDropEventArgs(_data, e, this));
							}						
							
							e.Handled = true;
						} else {
							e.Effects = DragDropEffects.None;
						}
					}
					
				} else
					e.Effects = DragDropEffects.None;
			} else
				e.Effects = DragDropEffects.None;
		}
		
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			base.OnGiveFeedback(e);
			if (e.Effects.HasFlag(DragDropEffects.Copy)) {
				Mouse.SetCursor(Cursors.Cross);
			} else if (e.Effects.HasFlag(DragDropEffects.Move)) {
				Mouse.SetCursor(Cursors.Pen);
			} else {
				Mouse.SetCursor(Cursors.No);
			}
			
			if (GetDragFeedback != null)
				GetDragFeedback(this, new TreeNodeDragDropEventArgs(_data, e, this));
		}	
		
		public void EnsureVisible()
		{
			this.IsSelected = true;
			Visibility = Visibility.Visible;
			this.ExpandSubtree();
			this.Focus();
		}
		
		public virtual void Remove()
		{
			if (this.Parent != null)
				(this.Parent as TreeNode).Items.Remove(this);
		}
		
	}
	
	public class TreeNodeDragDropEventArgs : EventArgs
	{
		public TreeNodeDragDropDataObject TreeNodeDraggedData;
		public GiveFeedbackEventArgs FeedbackData;
		public DragEventArgs DragData;
		public TreeNode DraggedOnNode;
		public TreeNode CopiedNode;
		public MouseEventArgs MouseData;
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data)
		{
			TreeNodeDraggedData = data;
		}
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data, MouseEventArgs mouseData)
		{
			TreeNodeDraggedData = data;
			MouseData = mouseData;
		}
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data, GiveFeedbackEventArgs feedback)
		{
			TreeNodeDraggedData = data;
			FeedbackData = feedback;
		}
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data, GiveFeedbackEventArgs feedback, TreeNode draggedOn)
		{
			TreeNodeDraggedData = data;
			FeedbackData = feedback;
			DraggedOnNode = draggedOn;
		}
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data, DragEventArgs dragData)
		{
			TreeNodeDraggedData = data;
			DragData = dragData;
		}
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data, DragEventArgs dragData, TreeNode draggedOn)
		{
			TreeNodeDraggedData = data;
			DragData = dragData;
			DraggedOnNode = draggedOn;
		}
		
		public TreeNodeDragDropEventArgs(TreeNodeDragDropDataObject data, DragEventArgs dragData, TreeNode draggedOn, TreeNode copy)
		{
			TreeNodeDraggedData = data;
			DragData = dragData;
			DraggedOnNode = draggedOn;
			CopiedNode = copy;
		}
	}
	
	public class NodeChangingEventArgs : EventArgs
	{
		public string OldText;
		public NodeChangingEventArgs(string text){
			OldText = text;
		}
	}
	
	public class NodeChangedEventArgs : EventArgs
	{
		public string NewText;
		public NodeChangedEventArgs(string text){
			NewText = text;
		}
	}
}
