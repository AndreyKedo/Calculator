using System;
using System.Linq;
using System.Windows.Input;

namespace MathAppCalculator.ViewModel
{
    class ViewModel : BaseViewModel
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
                    Expression += str.ToString();
                    IsOperatorPressed = true;
                }, (obj) =>
                 {
                     return ((Char.IsDigit(Expression.Last())) || Expression.Last() == ')');
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
                    Expression = Core.ArithmeticParser.ToParse(Expression.Replace('×', '*').Replace('÷', '/'));
                    Expression = Expression.Replace('-', '~');

                    if (!Expression.Any((ch) => { return ch == ','; }))
                        IsOperatorPressed = true;
                }, (obj) =>
                 {
                     return (Expression != _ExpressionText && Expression.Last() != '~');
                 });
            }
        }

        public ICommand LeftBrackets
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (Expression == _ExpressionText)
                        Expression = "";
                    Expression += "(";
                    IsOperatorPressed = true;
                }, (obj) =>
                {
                    return !Char.IsDigit(Expression.Last()) && Expression.Last() != '~' && Expression.Last() != '.';
                });
            }
        }

        public ICommand RightBrackets
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Expression += ")";
                    IsOperatorPressed = true;
                }, (obj) =>
                {
                    return Char.IsDigit(Expression.Last()) || Expression.Last() == ')';
                });
            }
        }

        public ICommand Negative
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (Expression == _ExpressionText)
                        Expression = "";
                    Expression += "~";
                }, (obj) =>
                {
                    return ((Expression.Last() == '(' || Expression.Last() == '*' || Expression.Last() == '/') || Expression == _ExpressionText);
                });
            }
        }

        public ICommand Dot
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Expression += ",";
                    IsOperatorPressed = false;
                }, (obj) =>
                 {
                     return Char.IsDigit(Expression.Last()) && IsOperatorPressed;
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
    }
}
