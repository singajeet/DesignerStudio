/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/14/2017
 * Time: 6:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Input;
using ICSharpCode.SharpDevelop.Workbench;

namespace ICSharpCode.SharpDevelop.Services.Commands
{
	/// <summary>
    /// No WPF project is complete without it's own version of this.
    /// </summary>
    public class AnotherSimpleCommand : ICommand
    {
		Predicate<object> _canExecuteDelegate;
		Action<object> _executeDelegate;
		
		public AnotherSimpleCommand(Action<object> execute)
			: this(execute, null)
		{
			
		}
		
		public AnotherSimpleCommand(Action<object> execute, Predicate<object> canExecuteParam)
		{
			if (execute == null)
                throw new ArgumentNullException("execute");

            _canExecuteDelegate = canExecuteParam;
            _executeDelegate = execute;
		}

        public bool CanExecute(object parameter)
        {
			return _canExecuteDelegate == null || _canExecuteDelegate(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
			_executeDelegate(parameter);
        }
    }
}
