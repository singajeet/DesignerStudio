/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/30/2017
 * Time: 11:13 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.SharpDevelop.Services.Gui.Dialogs.Wpf
{
	/// <summary>
	/// Interaction logic for NewProjectDialog.xaml
	/// </summary>
	public partial class NewProjectDialog : UserControl
	{
		public NewProjectDialog()
		{
			InitializeComponent();			
		}
		
		public void SetDataContext(object context)
		{			
			this.DataContext = context;					
		}
		void CategoriesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			
			((NewProjectDialogViewModel)this.DataContext).SelectedCategory = (Category)e.NewValue;
		}
		
		void TargetFrameworkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}
		
		void TemplatesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TemplatesListView.SelectedItems.Count == 1) {
				DescriptionLabel.Text = StringParser.Parse(((TemplateItem)TemplatesListView.SelectedItem).Template.Description);
				CreateProjectButton.IsEnabled = true;
			} else {
				DescriptionLabel.Text = String.Empty;
				CreateProjectButton.IsEnabled = false;
			}
		}
		
		void CategoriesTreeView_SourceUpdated(object sender, DataTransferEventArgs e)
		{
			ProjectLocationPathTxtBox.Text = ICSharpCode.Core.PropertyService.Get("ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.DefaultPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SharpDevelop Projects"));			
			((TreeViewItem)CategoriesTreeView.Items[0]).IsSelected = true;
		}
	}
}