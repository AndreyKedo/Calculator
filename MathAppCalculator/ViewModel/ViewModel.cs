using System;
using System.Linq;
using System.Windows.Input;
using MathAppCalculator.Model;

namespace MathAppCalculator.ViewModel
{
    class ViewModel : BaseViewModel
    {
        bool IsOperatorPressed = true;
        private string _Expression = string.Empty;
        public string Expression
        {
            get
            {
                if (_Expression == string.Empty)
                    return null;
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
                    if (_Expression.All(char.IsLetter))
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
                     if (_Expression != string.Empty)
                         return ((char.IsDigit(_Expression.Last())) || _Expression.Last() == ')');
                     return false;
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
                    Expression = ArithmeticParser.ToParse(Expression.Replace('×', '*').Replace('÷', '/'));
                    Expression = Expression.Replace('-', '~');

                    if (!Expression.Any((ch) => { return ch == ','; }))
                        IsOperatorPressed = true;
                }, (obj) =>
                 {
                     return (_Expression != string.Empty && _Expression.Last() != '~');
                 });
            }
        }

        public ICommand LeftBrackets
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Expression += "(";
                    IsOperatorPressed = true;
                }, (obj) =>
                {
                    if(_Expression != string.Empty)
                        return (!char.IsDigit(_Expression.Last())) && _Expression.Last() != '~' && _Expression.Last() != '.';
                    return true;
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
                    if (_Expression != string.Empty)
                        return (char.IsDigit(_Expression.Last()) || _Expression.Last() == ')') && IsOpenBracket();
                    return false;
                });
            }
        }

        public ICommand Negative
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Expression += "~";
                }, (obj) =>
                {
                    if(_Expression != string.Empty)
                        return _Expression != string.Empty ? _Expression.Last() == '(' || _Expression.Last() == '*' || _Expression.Last() == '/' : false;
                    return _Expression == string.Empty;
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
                     if (_Expression != string.Empty)
                         return char.IsDigit(_Expression.Last()) && IsOperatorPressed;
                     return false;
                 });
            }
        }

        public ICommand Clean
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Expression = string.Empty;
                    IsOperatorPressed = true;
                }, (obj) => 
                {
                    return _Expression != string.Empty;
                });
            }
        }
        #endregion

        bool IsOpenBracket()
        {
            foreach(var chr in _Expression)
            {
                if (chr == '(')
                    return true;
            }
            return false;
        }
    }
}
