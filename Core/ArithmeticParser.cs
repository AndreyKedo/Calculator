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
        /// <returns>Возвращает строку содержащию ответ или сообщение об ошибке</returns>
        public static string ToParse(string str)
        {
            double outpute = 0;
            if (str != "")
            {
                Str = "(" + str + ")";
                if (IfBracketsEqual())
                {
                    outpute = ExpressionHandling();
                    if (Double.IsNaN(outpute))
                        return "Не хватает операнда";
                    if (Double.IsInfinity(outpute))
                        return "Деление на ноль невозможно";

                    return outpute.ToString();
                }
            }
            return "Не хватает скобок";
        }

        /// <summary>
        /// Метод парсит выражение и выполняет арифмитические действия над операндами
        /// </summary>
        /// <returns>Возвращает результат выражения, или NaN если не хватает операнда.</returns>
        private static double ExpressionHandling()
        {
            short i = 0;
            char chr;
            Stack<double> number = new Stack<double>();
            Stack<char> operation = new Stack<char>();
            try
            {
                while (i < Str.Length)
                {
                    chr = Str[i];
                    if (Char.IsDigit(chr) || chr == '~')
                    {
                        number.Push(NumberConcatination(ref i));
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
            }
            catch (InvalidOperationException)
            {
                return Double.NaN;
            }

            return number.Pop();
        }

        /// <summary>
        /// Метод конкатенирует последоватльность чисел в строку
        /// </summary>
        /// <param name="index">Индекс массива</param>
        /// <returns>Возвращает строку</returns>
        private static double NumberConcatination(ref short index)
        {
            short i = index;
            bool isNegative = false;
            string _buffer = String.Empty;

            if (Str[i] == '~')
            {
                i++;
                isNegative = true;
            }

            while (Char.IsDigit(Str[i]) || Str[i] == ',')
            {
                _buffer += Str[i];
                i++;
            }
            index = --i;
            return isNegative ? (Double.Parse(_buffer) * -1) : Double.Parse(_buffer);
        }

        /// <summary>
        /// Метод производит арифметическую операцию над операндами
        /// </summary>
        /// <param name="chr">Оператор</param>
        /// <param name="num">Стек с числами</param>
        private static void Calc(char chr, ref Stack<double> num)
        {
            double _buff = 0;
            switch (chr)
            {
                case '+':
                    _buff = num.Pop();
                    num.Push(num.Pop() + _buff);
                    break;
                case '-':
                    _buff = num.Pop();
                    num.Push(num.Pop() - _buff);
                    break;
                case '*':
                    _buff = num.Pop();
                    num.Push(num.Pop() * _buff);
                    break;
                case '/':
                    _buff = num.Pop();
                    num.Push(num.Pop() / _buff);
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
            return countS('(') == countS(')');
        }

    }
}
