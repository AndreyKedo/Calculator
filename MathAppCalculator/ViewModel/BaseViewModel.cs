using System;
using System.Windows.Input;
using System.ComponentModel;

namespace MathAppCalculator.ViewModel
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
    }

    class DelegateCommand : ICommand
    {
        private Action<object> action;
        private Func<object, bool> func;

        public DelegateCommand(Action<object> actionExecute, Func<object, bool> funcExecute)
        {
            action = actionExecute;
            func = funcExecute;
        }

        public DelegateCommand(Action<object> actionExecute) : this(actionExecute, null)
        {
            action = actionExecute;
        }

        public bool CanExecute(object parameter)
        {
            return func != null ? func.Invoke(parameter) : true;
        }

        public void Execute(object parameter)
        {
            action?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
