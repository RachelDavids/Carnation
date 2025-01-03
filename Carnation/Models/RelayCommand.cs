using System;
using System.Windows.Input;

namespace Carnation
{
    internal class RelayCommand(Action commandAction, Func<bool> canExecute = null)
        : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke() != false;
        }

        public void Execute(object parameter)
        {
            commandAction.Invoke();
        }
    }

    internal class RelayCommand<T>(Action<T> commandAction, Func<T, bool> canExecute = null)
        : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) != false;
        }

        public void Execute(object parameter)
        {
            commandAction.Invoke((T)parameter);
        }
    }
}
