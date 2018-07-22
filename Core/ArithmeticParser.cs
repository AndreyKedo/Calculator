using System;
using System.Collections.Generic;

namespace Core
{
    public static class ArithmeticParser
    {
        private static string Str { get; set; }
        private static Func<char, uint> countS = (ch) =>
        {
            uint _count = 0;
            for (int i = 0; i < Str.Length; i++)
                if (Str[i] == ch)
                {
                    _count++;
                }
            return _count;
        };

        public static string ToParse(string str)
        {
            double outpute = 0;
            if (str != "")
            {
                Str = "(" + str + ")";
                if (IfBracketsEqual())
                {
                    outpute = ExpressionHandling(Str);
                    if (Double.IsInfinity(outpute))
                        return "Деление на ноль невозможно";

                    return outpute.ToString();
                }
            }
            return "Empty line";
        }

        private static double ExpressionHandling(string str)
        {
            int i = 0;
            char chr;
            Stack<double> number = new Stack<double>();
            Stack<char> operation = new Stack<char>();
            while (i < str.Length)
            {
                chr = str[i];
                if (Char.IsDigit(chr))
                {
                    number.Push(Double.Parse(NumberConcatination(ref i, str)));
                }
                else if (operation.Count == 0 || chr == '(')
                {
                    operation.Push(chr);
                }
                else if (chr == '+' || chr == '-' || chr == '*' || chr == '/')
                {
                    if (PriorityOperation(chr) > PriorityOperation(operation.Peek()))
                    {
                        operation.Push(chr);
                    }
                    else if (PriorityOperation(chr) == PriorityOperation(operation.Peek()))
                    {
                        while (operation.Count != 0 && (PriorityOperation(chr) == PriorityOperation(operation.Peek())))
                        {
                            Calc(operation.Pop(), ref number);
                        }
                        if (operation.Count != 0 && (PriorityOperation(chr) > PriorityOperation(operation.Peek())))
                        {
                            operation.Push(chr);
                        }
                    }
                    else
                    {
                        while (PriorityOperation(chr) < PriorityOperation(operation.Peek()))
                        {
                            Calc(operation.Pop(), ref number);
                        }
                        operation.Push(chr);
                    }
                }
                else
                {
                    while (operation.Peek() != '(')
                    {
                        Calc(operation.Pop(), ref number);
                    }
                    operation.Pop();
                }
                if (i < str.Length)
                    i++;
            }
            return number.Pop();
        }

        private static string NumberConcatination(ref int index, string str)
        {
            string buffer = String.Empty;
            int j = index;
            while (Char.IsDigit(str[j]))
            {
                buffer += str[j];
                j++;
            }
            index = (j - 1);
            return buffer;
        }

        private static string FractionalNumber(string[] str)
        {
            string _joinStr = "";
            _joinStr = string.Join(_joinStr, str, 0, str.Length);
            return _joinStr;
        }

        private static void Calc(char chr, ref Stack<double> num)
        {
            double buff = 0;
            switch (chr)
            {
                case '+':
                    buff = num.Pop();
                    num.Push(num.Pop() + buff);
                    break;
                case '-':
                    buff = num.Pop();
                    num.Push(num.Pop() - buff);
                    break;
                case '*':
                    buff = num.Pop();
                    num.Push(num.Pop() * buff);
                    break;
                case '/':
                    buff = num.Pop();
                    num.Push(num.Pop() / buff);
                    break;
            }
        }

        private static byte PriorityOperation(char chr)
        {
            switch (chr)
            {
                case '+':
                    return 1;
                case '-':
                    return 2;
                case '*':
                case '/':
                    return 3;
            }
            return 0;
        }

        private static bool IfBracketsEqual()
        {
            return (countS('(') == countS(')'));
        }

    }
}
