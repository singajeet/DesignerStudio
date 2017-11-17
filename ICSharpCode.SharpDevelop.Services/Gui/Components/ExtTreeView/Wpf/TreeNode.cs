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
using ControlzEx;
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
		PackIconMaterial iconCtl = new PackIconMaterial();
		TextBlock textLabelCtl = new TextBlock();
		TextBox textBoxCtl = new TextBox();
		
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
			panel.Children.Add(iconCtl);
			panel.Children.Add(textLabelCtl);
			panel.Children.Add(textBoxCtl);
			
			this.Header = panel;
			
			textBoxCtl.Visibility = Visibility.Collapsed;
			textBoxCtl.LostFocus+= textBox_LostFocus;
			textBoxCtl.IsVisibleChanged+= textBox_IsVisibleChanged;
			textBoxCtl.KeyDown+= textBox_KeyDown;
			
			textLabelCtl.MouseLeftButtonDown += text_MouseLeftButtonDown;
			textLabelCtl.KeyDown += text_KeyDown;
		}

		void text_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F2) {
				if (this.IsSelected) {
					IsInEditMode = true;
					textLabelCtl.Visibility = Visibility.Collapsed;
					textBoxCtl.Visibility = Visibility.Visible;	
				}				
			}
		}
		void text_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (this.IsSelected) {
				IsInEditMode = true;
				textLabelCtl.Visibility = Visibility.Collapsed;
				textBoxCtl.Visibility = Visibility.Visible;
				
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
