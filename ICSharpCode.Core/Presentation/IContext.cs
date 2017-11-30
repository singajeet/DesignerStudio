/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/9/2017
 * Time: 1:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;

namespace ICSharpCode.Core.Presentation
{
	/// <summary>
	/// Description of IContext.
	/// </summary>
	public interface IContext
	{
		UIElement InputBindingOwner { get; set; }
		string ActivationMethod { get; set; }
		bool ImmediatelyExpandMenuBuildersForShortcuts { get; set; }
	}
}
