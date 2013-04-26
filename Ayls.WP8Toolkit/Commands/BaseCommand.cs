using System;
using System.Windows.Input;

namespace Ayls.WP8Toolkit.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract bool CanExecute(object parameter);

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }

        public abstract void Execute(object parameter);
    }
}
