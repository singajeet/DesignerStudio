/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/14/2017
 * Time: 5:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;
using System.Windows.Controls;


namespace ICSharpCode.SharpDevelop.Services.Palette
{
	/// <summary>
	/// Interaction logic for PaletteSelector.xaml
	/// </summary>
	public partial class PaletteSelector : UserControl
	{
		ResourceDictionary resd;
		//PaletteSelectorViewModel _viewModel;
		public PaletteSelector()
		{
			InitializeComponent();
			//_viewModel = new PaletteSelectorViewModel();
			//this.DataContext = _viewModel;
			resd = this.Resources; 
		}
	}
	
	

}