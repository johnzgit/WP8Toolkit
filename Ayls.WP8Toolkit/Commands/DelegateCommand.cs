using System;
using System.Windows.Input;

namespace Ayls.WP8Toolkit.Commands
{
    public class DelegateCommand : ICommand
    {
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _executeAction;

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null)
        {
        }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException("executeAction");
            }
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            var result = true;
            if (_canExecute != null)
            {
                result = _canExecute(parameter);
            }

            return result;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }

        public void Execute(object parameter)
        {
            this._executeAction(parameter);
        }
    }
}
