/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 2:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;

namespace CustomView
{
	/// <summary>
	/// Description of ShowCustomViewCommand.
	/// </summary>
	public class ShowCustomViewCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			SD.Workbench.ShowView(new MyCustomView());
		}
	}
}
