using System;

namespace Ayls.WP8Toolkit.Commands
{
    public class DelegateCommand : BaseCommand
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

        public override bool CanExecute(object parameter)
        {
            var result = true;
            if (_canExecute != null)
            {
                result = _canExecute(parameter);
            }

            return result;
        }

        public override void Execute(object parameter)
        {
            this._executeAction(parameter);
        }
    }
}
