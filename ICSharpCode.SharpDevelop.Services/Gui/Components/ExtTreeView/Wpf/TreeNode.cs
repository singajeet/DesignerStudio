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
	
		#endregion

		StackPanel panel = new StackPanel();
		Viewbox viewbox = new Viewbox();
		string iconPath;
		TextBlock textLabelCtl = new TextBlock();
		TextBox textBoxCtl = new TextBox();
		
		public void SetIcon(string iconPath)
		{
			if (iconPath == null) {
				return;
			}
			this.iconPath = iconPath;
		}
		
		public string IconPath{
			get { 
				return iconPath;
			}
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
			get { return (string)GetValue(textProperty); }
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
				HintAssist.SetHint(this, "(editable)");
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
