using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Парсер арифметических выражений
    /// Данны класс реализует сущность позволяющую
    /// преобразовывать выражение типа строка в число например,
    /// input: 2+2*2-(2+2/2)
    /// output: 3
    /// </summary>
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

        /// <summary>
        /// Метод парсинга арифметического выражения.
        /// </summary>
        /// <param name="str">Принимает строку в виде арифметического выражения.</param>
        /// <returns>Возвращает строку содержащию ответ.</returns>
        public static string ToParse(string str)
        {
            double outpute = 0;
            if (str != "")
            {
                Str = "(" + str + ")";
                if (IfBracketsEqual())
                {
                    outpute = ExpressionHandling();
                    if (Double.IsInfinity(outpute))
                        return "Деление на ноль невозможно";

                    return outpute.ToString();
                }
            }
            return "Empty line";
        }

        /// <summary>
        /// Метод парсит выражение и выполняет арифмитические действия над операндами
        /// </summary>
        /// <returns>Возвращает результат выражения</returns>
        private static double ExpressionHandling()
        {
            int i = 0;
            char chr;
            Stack<double> number = new Stack<double>();
            Stack<char> operation = new Stack<char>();
            while (i < Str.Length)
            {
                chr = Str[i];
                if (Char.IsDigit(chr))
                {
                    number.Push(Double.Parse(NumberConcatination(ref i, Str)));
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
                if (i < Str.Length)
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

        /// <summary>
        /// Метод производит арифметическую операцию над операндами
        /// </summary>
        /// <param name="chr">Оператор</param>
        /// <param name="num">Стек с числами</param>
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

        /// <summary>
        /// Метод определяет приоритет операции
        /// </summary>
        /// <param name="chr">Оператор</param>
        /// <returns>Возвращает приориет оператора</returns>
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
