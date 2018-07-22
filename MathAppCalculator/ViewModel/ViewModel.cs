using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq;

namespace MathAppCalculator.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        private const string _ExpressionText = "Выражение";
        private string _Expression = _ExpressionText;
        public string Expression
        {
            get
            {
                return _Expression;
            }
            set
            {
                _Expression = value;
                OnPropertyChanged(nameof(Expression));
            }
        }

        #region Нумирация
        public ICommand Number
        {
            get
            {
                return new DelegateCommand((str) =>
                {
                    if (Expression == _ExpressionText) { Expression = ""; }
                    Expression += str.ToString();
                });
            }
        }
        #endregion

        #region Операции
        public ICommand Operation
        {
            get
            {
                return new DelegateCommand((str) => 
                {
                    if((Expression != _ExpressionText && (Expression.Last() >= '0' && Expression.Last() <= '9')) || Expression.Last() == ')')
                        Expression += str.ToString();
                });
            }
        }
        #endregion

        #region Остальные действия

        public ICommand Calc
        {
            get
            {
                return new DelegateCommand((obj) => 
                {
                    if(Expression != _ExpressionText)
                        Expression = Core.ArithmeticParser.ToParse(Expression);
                });
            }
        }

        public ICommand LeftBrackets
        {
            get
            {
                return new DelegateCommand((obj) => 
                {
                    if (!(Expression.Last() >= '0' && Expression.Last() <= '9'))
                    {
                        if (Expression == _ExpressionText)
                            Expression = "";
                        Expression += "(";
                    }
                });
            }
        }

        public ICommand RightBrackets
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (Expression != _ExpressionText && (Expression.Last() >= '0' && Expression.Last() <= '9'))
                    {
                        Expression += ")";
                    }
                });
            }
        }

        public ICommand Negative
        {
            get
            {
                return new DelegateCommand((obj) => 
                {
                    if ((Expression.Last() == '(' || Expression.Last() == '*' || Expression.Last() == '/') || Expression == _ExpressionText) {
                        if (Expression == "Выражение")
                            Expression = "";
                        Expression += "-";
                    }
                });
            }
        }

        public ICommand Dot
        {
            get
            {
                return new DelegateCommand((obj) => 
                {
                    if (Expression.Last() >= '0' && Expression.Last() <= '9')
                        Expression += ".";
                });
            }
        }

        public ICommand Clean
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Expression = _ExpressionText;
                });
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string str)
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
