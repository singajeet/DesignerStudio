/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/10/2017
 * Time: 1:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;

namespace ICSharpCode.Core.Presentation
{
	/// <summary>
	/// Description of Utils.
	/// </summary>
	public static class Utils
	{
		public static void ApplyPackIcon(Control control, string packIconKey)
		{
			string packIconKind = packIconKey;

			object packIconControl = control.Template
													.FindName("PackIcon", control) 
														as object;					
			
			
			if(packIconControl is PackIconMaterial){
				PackIconMaterial packIcon = (PackIconMaterial)packIconControl;
				packIcon.Kind = (PackIconMaterialKind)Enum
									.Parse(typeof(PackIconMaterialKind), packIconKind);
			}
			else if(packIconControl is PackIconMaterialLight){
				PackIconMaterialLight packIconLight = (PackIconMaterialLight)packIconControl;
				packIconLight.Kind = (PackIconMaterialLightKind)Enum
										.Parse(typeof(PackIconMaterialLightKind), packIconKind);
			}
			else if(packIconControl is PackIconModern){
				PackIconModern packIconModern = (PackIconModern)packIconControl;
				packIconModern.Kind = (PackIconModernKind)Enum
										.Parse(typeof(PackIconModernKind), packIconKind);
			}
			else if(packIconControl is PackIconOcticons){
				PackIconOcticons packIconOcticons = (PackIconOcticons)packIconControl;
				packIconOcticons.Kind = (PackIconOcticonsKind)Enum
										.Parse(typeof(PackIconOcticonsKind), packIconKind);
			}
			else if(packIconControl is PackIconSimpleIcons){
				PackIconSimpleIcons packIconSimple = (PackIconSimpleIcons)packIconControl;
				packIconSimple.Kind = (PackIconSimpleIconsKind)Enum
										.Parse(typeof(PackIconSimpleIconsKind), packIconKind);					
			} 
			else if (packIconControl is PackIconEntypo) {
				PackIconEntypo packIconSimple = (PackIconEntypo)packIconControl;
				packIconSimple.Kind = (PackIconEntypoKind)Enum
										.Parse(typeof(PackIconEntypoKind), packIconKind);					
			} else if (packIconControl is PackIconFontAwesome) {
				PackIconFontAwesome packIconSimple = (PackIconFontAwesome)packIconControl;
				packIconSimple.Kind = (PackIconFontAwesomeKind)Enum
										.Parse(typeof(PackIconFontAwesomeKind), packIconKind);					
			
			}
		}
	}
}
