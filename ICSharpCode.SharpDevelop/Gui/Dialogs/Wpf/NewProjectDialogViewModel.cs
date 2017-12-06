/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 12/01/2017
 * Time: 22:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Templates;

namespace ICSharpCode.SharpDevelop.Services.Gui.Dialogs.Wpf
{
	/// <summary>
	/// Description of NewProjectDialogModelView.
	/// </summary>
	public class NewProjectDialogViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<TemplateItem> _alltemplates = new ObservableCollection<TemplateItem>();
		private ObservableCollection<Category> _categoryTreeViewItems = new ObservableCollection<Category>();
		private TargetFramework _selectedTargetFramework;
		private ObservableCollection<TargetFramework> _targetFrameworks;
		private  bool _createNewSolution;
		private bool _isCreateDirectoryForSolutionChecked= true;
		private bool _isCustomSolutionName= false;
		private string _projectLocationDirectory;		
		private Category _selectedCategory;
		private string _projectName;
		private string _solutionName;
		private ISolutionFolder _solutionFolder;
		internal ProjectTemplateResult _result;
		
		public bool IsCustomSolutionName{
			get { return _isCustomSolutionName; }
			set {
				_isCustomSolutionName = value;
						OnPropertyChanged();
					}
		}
		
		
		public ObservableCollection<TemplateItem> AllTemplates{
			get { return this._alltemplates; }
			set { this._alltemplates = value; 
				OnPropertyChanged();
			}
		}
		
		public ObservableCollection<Category> CategoryTreeViewItems{
			get { return this._categoryTreeViewItems; }
			set { this._categoryTreeViewItems = value; 
				OnPropertyChanged();
			}
		}
		
		public bool CreateNewSolution{
			get { return this._createNewSolution; }
			set { this._createNewSolution = value; 
				OnPropertyChanged();
			}
		}
		
		public string ProjectLocationDirectory{
			get { 
				if (_projectLocationDirectory == null) {
				_projectLocationDirectory= ICSharpCode.Core.PropertyService.Get("ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.DefaultPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SharpDevelop Projects"));
				}
					
				return this._projectLocationDirectory;
			}
			set { this._projectLocationDirectory = value; 
				OnPropertyChanged();
			}
		}
		
		public Category SelectedCategory{
			get {				
				return this._selectedCategory; 
			}
			set { this._selectedCategory = value; 
				OnPropertyChanged();
				OnPropertyChanged("TargetFrameworks");
				OnPropertyChanged("SelectedTargetFramework");
			}
		}
		
		public ObservableCollection<TargetFramework> TargetFrameworks{
			get { 
				ObservableCollection<TargetFramework> tempTargetFrameworks = new ObservableCollection<TargetFramework>();
				foreach (TemplateItem item in ((Category)SelectedCategory).Templates) {
					tempTargetFrameworks.AddRange(item.Template.SupportedTargetFrameworks);
				}
				
				_targetFrameworks = new ObservableCollection<TargetFramework>(tempTargetFrameworks
				                                                 .Where(fx => fx.DisplayName != null && 
				                                                        fx.IsAvailable())
				                                                 .OrderBy(fx => fx.TargetFrameworkVersion)
				                                                 .Distinct()
				                                                 .ToList());
				return _targetFrameworks;
			}
		}
		
		public TargetFramework SelectedTargetFramework{
			get { 
				string lastUsedTargetFramework = ICSharpCode.Core.PropertyService.Get("Dialogs.NewProjectDialog.TargetFramework", TargetFramework.DefaultTargetFrameworkVersion);
				string lastUsedTargetFrameworkProfile = ICSharpCode.Core.PropertyService.Get("Dialogs.NewProjectDialog.TargetFrameworkProfile", TargetFramework.DefaultTargetFrameworkProfile);
				foreach (TargetFramework framework in _targetFrameworks) {
					if (framework.TargetFrameworkVersion.Equals(lastUsedTargetFramework)
					    && framework.TargetFrameworkProfile.Equals(lastUsedTargetFrameworkProfile)) {
						_selectedTargetFramework = framework;
						return framework;
					}
				}
				
				//if not found, select first element as default
				return _targetFrameworks[0];
			}
			set { 
				_selectedTargetFramework = value;				
			}
		}
		
		public bool IsCreateDirectoryForSolutionChecked{
			get { return this._isCreateDirectoryForSolutionChecked; }
			set { this._isCreateDirectoryForSolutionChecked = value; 
				OnPropertyChanged();
			}
		}
		
		public string ProjectName{
			get {
				if (this._projectName== null)
					this._projectName = "MyProject";
				return this._projectName; }
			set { this._projectName = value; 
				OnPropertyChanged();
				OnPropertyChanged("SolutionName");
			}
		}
		
		public string SolutionName{
			get { 
				if (_solutionName == null)
					return ProjectName;
				
				if (!IsCustomSolutionName)
					return _projectName;
				
				return this._solutionName; }
			set { this._solutionName = value; 
				OnPropertyChanged();
			}
		}
		
		public string NewProjectDirectory {
			get {
				if (IsCreateDirectoryForSolutionChecked) {
					return Path.Combine(NewSolutionDirectory, ProjectName);
				} else {
					return NewSolutionDirectory;
				}
			}
		}
		
		public string NewSolutionDirectory {
			get {
				string location = ProjectLocationDirectory;
				string name = IsCreateDirectoryForSolutionChecked ? SolutionName : ProjectName;
				return Path.Combine(location.Trim(), name.Trim());
			}
		}
		
		public ISolutionFolder SolutionFolder{
			get { return this._solutionFolder; }
			set { this._solutionFolder = value; 
				OnPropertyChanged();
			}
		}
		
		public NewProjectDialogViewModel(IEnumerable<TemplateCategory> templateCategories, bool createNewSolution)
		{
			this.CreateNewSolution = createNewSolution;
			MyInitializeComponents();
			
			InitializeTemplates(templateCategories);
			InitializeViewModel();
			
			ProjectLocationDirectory = ICSharpCode.Core.PropertyService.Get("ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.DefaultPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DesignerStudio Projects"));
		}

		void MyInitializeComponents()
		{
			//throw new NotImplementedException();
		}

		void InitializeTemplates(IEnumerable<TemplateCategory> templateCategories)
		{
			foreach (var templateCategory in Sorted(templateCategories)) {
				var cat = CreateCategory(templateCategory);
				if (!cat.IsEmpty)
					CategoryTreeViewItems.Add(cat);
			}
		}

		void InitializeViewModel()
		{
			string initialSelectedCategory = StringParser.Parse("C#\\${res:Templates.File.Categories.WindowsApplications}");			
			SelectedCategory = GetSelectedCategory(ICSharpCode.Core.PropertyService.Get("Dialogs.NewProjectDialog.LastSelectedCategory", initialSelectedCategory));
		}
		
		Category GetSelectedCategory(string categoryName)
		{
			foreach (Category cat in this._categoryTreeViewItems) {
				if (cat.Text.Equals(categoryName))
					return cat;
			}
			
			return null;
		}
		
		IEnumerable<TemplateCategory> Sorted(IEnumerable<TemplateCategory> templateCategories)
		{
			return templateCategories.OrderByDescending(c => c.SortOrder).ThenBy(c => StringParser.Parse(c.DisplayName));
		}

		Category CreateCategory(TemplateCategory templateCategory)
		{
			Category item = new Category(templateCategory.DisplayName);
			foreach (var subcategory in Sorted(templateCategory.Subcategories)) {
				var subItem = CreateCategory(subcategory);
				if (!subItem.IsEmpty)
					item.Items.Add(subItem);
			}
			
			foreach (var template in templateCategory.Templates.OfType<ProjectTemplate>()) {
				if (!template.IsVisible(SolutionFolder != null ? SolutionFolder.ParentSolution : null)) {
					// Do not show solution template when added a new project to existing solution
					continue;
				}
				TemplateItem templateItem = new TemplateItem(template);				
				AllTemplates.Add(templateItem);
				item.Templates.Add(templateItem);
			}
			
			return item;
		}
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if(PropertyChanged!= null)
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
	
	/// <summary>
	/// Holds a new file template
	/// </summary>
	public class TemplateItem : ListViewItem
	{
		ProjectTemplate template;
		
		public TemplateItem(ProjectTemplate template)
		{
			this.template = template;	

			StackPanel sp = new StackPanel();
			TextBlock tb = new TextBlock();
			tb.Text = template.DisplayName;
			sp.Children.Add(tb);
			
			this.Content = sp;
		}
		
		public ProjectTemplate Template {
			get {
				return template;
			}
		}
	}
	
	public class Category : TreeViewItem
	{
		ObservableCollection<TemplateItem> templates  = new ObservableCollection<TemplateItem>();
		string text;
		
		public string Text{
			get { return this.text; }
			set { this.text = value; }
		}
		
		public Category(string name) : base()
		{			
			this.Text = StringParser.Parse(name);	
			StackPanel sp = new StackPanel();
			TextBlock tb = new TextBlock();
			
			tb.Text = this.Text;
			sp.Orientation = Orientation.Horizontal;
			sp.Children.Add(tb);		
			
			this.Header = sp;
		}
		
		public ObservableCollection<TemplateItem> Templates {
			get {
				return templates;
			}
		}
		
		public bool IsEmpty {
			get { return templates.Count == 0; }
		}
	}
	
	public class ProjectNameValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string strValue = (string)value;
			if (String.IsNullOrEmpty(strValue))
				return new ValidationResult(false, "Project Name is required!");
			
			if(!char.IsLetter(strValue[0]))
				return new ValidationResult(false, StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.ProjectNameMustStartWithLetter}"));
			
			if (strValue.EndsWith(".", StringComparison.Ordinal)) {
				return new ValidationResult(false, StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.ProjectNameMustNotEndWithDot}"));
			}			
			
			return ValidationResult.ValidResult;
		}
	}
	
	public class SolutionNameValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string strValue = (string)value;
			if (String.IsNullOrEmpty(strValue))
				return new ValidationResult(false, "Solution Name is required!");
			
			if(!char.IsLetter(strValue[0]))
				return new ValidationResult(false, StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.ProjectNameMustStartWithLetter}"));
			
			if (strValue.EndsWith(".", StringComparison.Ordinal)) {
				return new ValidationResult(false, StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.ProjectNameMustNotEndWithDot}"));
			}			
			
			return ValidationResult.ValidResult;
		}
	}
	
	public class FilePathValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string strValue = (string)value;
			
			if (!FileUtility.IsValidDirectoryEntryName(strValue)
			    || !FileUtility.IsValidDirectoryEntryName(strValue))
			{
				return new ValidationResult(false, StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.IllegalProjectNameError}"));
			}		
			
			return ValidationResult.ValidResult;
		}
	}
	
	public class DirectoryPathValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string strValue = (string)value;
			
			if (!FileUtility.IsValidPath(strValue) || !Path.IsPathRooted(strValue)) {
				return new ValidationResult(false, StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.NewProjectDialog.SpecifyValidLocation}"));
			}		
			
			return ValidationResult.ValidResult;
		}
	}			
}
