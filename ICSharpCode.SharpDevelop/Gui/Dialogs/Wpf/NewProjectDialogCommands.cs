/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 12/6/2017
 * Time: 1:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Windows.Input;

namespace ICSharpCode.SharpDevelop.Services.Gui.Dialogs.Wpf
{
	/// <summary>
	/// Description of NewProjectDialogCommands.
	/// </summary>
	public class NewProjectDialogCommands
	{
		private static object syncRoot = new object();
		private static RoutedUICommand _createProjectCommand = null;
		private static RoutedUICommand _cancelDialogCommand = null;
		private static RoutedUICommand _browseFolderCommand = null;
		
		public static RoutedUICommand CreateProject{
			get { 
				object obj;
				Monitor.Enter(obj = NewProjectDialogCommands.syncRoot);
				try{
					if(NewProjectDialogCommands._createProjectCommand == null)
						NewProjectDialogCommands._createProjectCommand = new RoutedUICommand("CreateProject", "CreateProject", typeof(NewProjectDialogCommands));
				} finally {
					Monitor.Exit(obj);
				}
				
				return NewProjectDialogCommands._createProjectCommand;
			}
		}
		
		public static RoutedUICommand CancelDialog{
			get { 
				object obj;
				Monitor.Enter(obj = NewProjectDialogCommands.syncRoot);
				try{
					if(NewProjectDialogCommands._cancelDialogCommand == null)
						NewProjectDialogCommands._cancelDialogCommand = new RoutedUICommand("CancelDialog", "CancelDialog", typeof(NewProjectDialogCommands));
				} finally {
					Monitor.Exit(obj);
				}
				
				return NewProjectDialogCommands._cancelDialogCommand;
			}
		}
		
		public static RoutedUICommand BrowseFolders{
			get { 
				object obj;
				Monitor.Enter(obj = NewProjectDialogCommands.syncRoot);
				try{
					if(NewProjectDialogCommands._browseFolderCommand == null)
						NewProjectDialogCommands._browseFolderCommand = new RoutedUICommand("BrowseFolders", "BrowseFolders", typeof(NewProjectDialogCommands));
				} finally {
					Monitor.Exit(obj);
				}
				
				return NewProjectDialogCommands._browseFolderCommand;
			}
		}
	}
}
