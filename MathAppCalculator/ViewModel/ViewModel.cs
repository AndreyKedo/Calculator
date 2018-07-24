using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq;

namespace MathAppCalculator.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        bool IsOperatorPressed = true;
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
                    if (Expression.All(char.IsLetter))
                        Expression = "";
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
                    if((Char.IsDigit(Expression.Last())) || Expression.Last() == ')')
                    {
                        Expression += str.ToString();
                        IsOperatorPressed = true;
                    }
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
                    if (Expression != _ExpressionText && Expression.Last() != '~')
                    {
                        Expression = Core.ArithmeticParser.ToParse(Expression.Replace('×', '*')
                            .Replace('÷', '/')
                            .Replace('.', ','));
                        Expression = Expression.Replace('-', '~');
                        IsOperatorPressed = true;
                    }
                });
            }
        }

        public ICommand LeftBrackets
        {
            get
            {
                return new DelegateCommand((obj) => 
                {
                    if (!Char.IsDigit(Expression.Last()) && Expression.Last() != '~')
                    {
                        if (Expression == _ExpressionText)
                            Expression = "";
                        Expression += "(";
                        IsOperatorPressed = true;
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
                    if ((Char.IsDigit(Expression.Last())) || Expression.Last() == ')')
                    {
                        Expression += ")";
                        IsOperatorPressed = true;
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
                            Expression = "";
                        Expression += "~";
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
                    if (Char.IsDigit(Expression.Last()) && IsOperatorPressed)
                    {
                        Expression += ".";
                        IsOperatorPressed = false;
                    }
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
                    IsOperatorPressed = true;
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
