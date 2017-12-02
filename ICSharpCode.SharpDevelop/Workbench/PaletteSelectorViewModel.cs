/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/14/2017
 * Time: 8:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ICSharpCode.SharpDevelop.Services.Commands;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace ICSharpCode.SharpDevelop.Services.Palette
{
	/// <summary>
	/// Description of PaletteSelectorViewModel.
	/// </summary>
	public class PaletteSelectorViewModel
    {
		ResourceDictionary resd;
		IEnumerable<Swatch> _swatches;
        public PaletteSelectorViewModel()
        {
        	
            _swatches = new SwatchesProvider().Swatches;            
        }

		public ICommand ToggleBaseCommand { 
        	get {
        		return new AnotherCommandImplementation(o => ApplyBase((bool)o));
        	}        
        }

        private static void ApplyBase(bool isDark)
        {
        	
            new PaletteHelper().SetLightDark(isDark);
        }

		public IEnumerable<Swatch> Swatches { 
        	get { 
				return _swatches;	
        	} 
        }

        public ICommand ApplyPrimaryCommand { 
			get{
				return new AnotherCommandImplementation(o => ApplyPrimary((Swatch)o));        		
        	}        
        }

        private static void ApplyPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }

		public ICommand ApplyAccentCommand { 
        	get { 
        		return new AnotherCommandImplementation(o => ApplyAccent((Swatch)o));	
        	} 
        }

        private static void ApplyAccent(Swatch swatch)
        {
            new PaletteHelper().ReplaceAccentColor(swatch);
        }
    }
}
