/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 10:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ICSharpCode.SharpDevelop.Services.Gui.Wpf
{
	/// <summary>
	/// Description of IClipboardHandler.
	/// </summary>
	public interface IClipboardHandler
	{
		bool EnableCut {
			get;
		}
		bool EnableCopy {
			get;
		}
		bool EnablePaste {
			get;
		}
		bool EnableDelete {
			get;
		}
		bool EnableSelectAll {
			get;
		}
		
		void Cut();
		void Copy();
		void Paste();
		void Delete();
		void SelectAll();
	}
}
