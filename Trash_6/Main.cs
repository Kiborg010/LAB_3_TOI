using System;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace project
{
    class Program1
    {
        static string strMain = "";
        static void Main() 
        {
            Console.WriteLine("Введите строку: ");
            strMain = Console.ReadLine();
            string strResult = "";
            bool flagMistake = false;
            if (Task(strMain) == 0)
            {
                for (int i = 0; i < addends.Length; i++)
                {
                    if (Addend(addends[i]))
                    {
                        strResult += addends[i];
                        try
                        {
                            strResult += allSigns[i];
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }
                    else
                    {
                        flagMistake = true;
                        break;
                    }
                }
                if (strMain.Replace(" ", "") == strResult && !flagMistake)
                {
                    Console.WriteLine("Строка корректна");
                }
                Console.Write("Первый уровень: ");
                int counter = 0;
                foreach (var child in tree.Root.Children)
                {
                    Console.Write($"{child.Data} \t");
                    try
                    {
                        Console.Write($"{allSigns[counter]} \t");
                        counter++;
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                }
                Console.Write($"\n\nВторой уровень: ");
                foreach (var child in tree.Root.Children)
                {
                    foreach (var child1 in child.Children)
                    {
                        Console.Write($"{child1.Data} \t");
                    }
                }
                Console.Write($"\n\nТретий уровень: ");
                foreach (var child in tree.Root.Children)
                {
                    foreach (var child1 in child.Children)
                    {
                        foreach (var child2 in child1.Children)
                        {
                            Console.Write($"{child2.Data} \t");
                        }
                    }
                }
                Console.Write($"\n\nЧетвёртый уровень: ");
                foreach (var child in tree.Root.Children)
                {
                    foreach (var child1 in child.Children)
                    {
                        foreach (var child2 in child1.Children)
                        {
                            foreach (var child3 in child2.Children)
                            {
                                Console.Write($"{child3.Data} \t");
                            }
                        }
                    }
                }
                Console.WriteLine();
                Console.Write($"\n\nПятый уровень: ");
                foreach (var child in tree.Root.Children)
                {
                    foreach (var child1 in child.Children)
                    {
                        foreach (var child2 in child1.Children)
                        {
                            foreach (var child3 in child2.Children)
                            {
                                foreach (var child4 in child3.Children)
                                {
                                    Console.Write($"{child4.Data} \t");
                                }
                            }
                        }
                    }
                }
            }
            else if (Task(strMain) == 1)
            {
                Console.WriteLine("Ошибка: не хватает слагаемых");
            }
            else if (Task(strMain) == 2)
            {
                Console.WriteLine("Ошибка: не хватает скобок");
            }
            
        }

        static string letterDown = "abcdefghijklmnopqrstuvwxyz";
        static string letterUp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string digit = "0123456789";
        static string digitAndDot = "." + digit;
        static char[] allSigns = new char[0];
        static bool flag = false;
        static string[] addends = new string[0];
        static Tree tree = new Tree(strMain);

        public static int Task(string str)
        {
            string addend = "";
            int scansymbol = 0;
            string[] signs = new string[] { "+", "-", "–", "=", "!", "&", "|", ">", "<" };
            string strWithoutGaps = str.Replace(" ", "");
            str = strWithoutGaps;
            int countBracketRight = 0;
            int countBracketLeft = 0;
            while (scansymbol < str.Length)
            {
                if (str[scansymbol] == ' ')
                {
                    scansymbol++;
                }
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) || ((countBracketLeft == countBracketRight) && (!signs.Contains(str[scansymbol].ToString())))))
                {
                    if (!flag)
                    {
                        if (str[scansymbol] == '(')
                        {
                            countBracketLeft++;
                        }
                        else if (str[scansymbol] == ')')
                        {
                            countBracketRight++;
                        }
                    }
                    addend += str[scansymbol];
                    scansymbol++;
                }
                if ((scansymbol == str.Length) && (countBracketLeft != countBracketRight))
                {
                    return 2;
                }
                flag = true;
                Array.Resize(ref addends, addends.Length + 1);
                addends[addends.Length - 1] = addend;
                if ((scansymbol < str.Length) && signs.Contains(str[scansymbol].ToString()))
                {
                    Array.Resize(ref allSigns, allSigns.Length + 1);
                    allSigns[allSigns.Length - 1] = str[scansymbol];
                    flag = false;
                }
                addend = "";
                scansymbol++;
            }
            if ((addends.Length - allSigns.Length != 1) || (addends.Contains("")))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool Expression(string str, ref Node child)
        {
            string addend = "";
            int scansymbol = 0;
            string[] signs = new string[] { "+", "-", "–", "=", "!", "&", "|", ">", "<" };
            string strWithoutGaps = str.Replace(" ", "");
            str = strWithoutGaps;
            int countBracketRight = 0;
            int countBracketLeft = 0;
            string[] addendsLocal = new string[0];
            string[] signsLocal = new string[0];
            while (scansymbol < str.Length)
            {
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) || ((countBracketLeft == countBracketRight) && (!signs.Contains(str[scansymbol].ToString())))))
                {
                    if (str[scansymbol] == '(')
                    {
                        countBracketLeft++;
                    }
                    else if (str[scansymbol] == ')')
                    {
                        countBracketRight++;
                    }
                    addend += str[scansymbol];
                    scansymbol++;
                }
                Array.Resize(ref signsLocal, signsLocal.Length + 1);
                try
                {
                    signsLocal[signsLocal.Length - 1] = str[scansymbol].ToString();
                }
                catch (IndexOutOfRangeException)
                {
                    if (signs.Contains(str[scansymbol - 1].ToString()))
                    {
                        signsLocal[signsLocal.Length - 1] = str[scansymbol - 1].ToString();
                    }
                }
                Array.Resize(ref addendsLocal, addendsLocal.Length + 1);
                addendsLocal[addendsLocal.Length - 1] = addend;
                addend = "";
                scansymbol++;
            }
            int counter = 0;
            foreach (string el in addendsLocal)
            {
                if (!Addend(el, ref child))
                {
                    return false;
                }
                Node subChild = new Node(signsLocal[counter]);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
                counter++;
            }
            return true;
        }

        public static bool Addend(string str, ref Node child)
        {
            Node subChild = new Node(str);
            ref Node subChildRef = ref subChild;
            child.AddChild(subChild);
            string factor = "";
            int scansymbol = 0;
            string[] signsLocal = new string[0];
            char[] signs = new char[] { '*', '/', '^', '$', '%' };
            while (scansymbol < str.Length)
            {
                while ((scansymbol < str.Length) && !signs.Contains(str[scansymbol]))
                {
                    factor += str[scansymbol];
                    scansymbol++;
                }
                int checkFactor = Factor(factor, ref subChildRef);
                if (checkFactor == 0)
                {
                    return true;
                }
                else if (checkFactor == 2)
                {
                    Console.WriteLine($"Ошибка: некорректная функция: {factor}");
                    return false;
                }
                else if (checkFactor == 3)
                {
                    Console.WriteLine($"Ошибка: некорректная переменная: {factor}");
                    return false;
                }
                else if (checkFactor == 4)
                {
                    Console.WriteLine($"Ошибка: содержание недопустимого элемента: {factor}");
                    return false;
                }
                else if (checkFactor == 5)
                {
                    Console.WriteLine($"Ошибка: некорректное выражение: {factor}");
                    return false;
                }
                else if (checkFactor == 6)
                {
                    Console.WriteLine($"Ошибка: содержание недопустимого элемента: {factor}");
                    return false;
                }
                try
                {
                    subChild.AddChild(new Node(str[scansymbol].ToString()));
                }
                catch (IndexOutOfRangeException)
                {
                    if (signs.Contains(str[scansymbol - 1]))
                    {
                        subChild.AddChild(new Node(str[scansymbol - 1].ToString()));
                    }
                };
                factor = "";
                scansymbol++;
            }
            return true;
        }

        public static bool Addend(string str)
        {
            Node subChild = new Node(str);
            ref Node subChildRef = ref subChild;
            tree.Root.AddChild(subChild);
            string factor = "";
            int scansymbol = 0;
            char[] signs = new char[] { '*', '/', '^', '$', '%' };
            while (scansymbol < str.Length)
            {
                while ((scansymbol < str.Length) && !signs.Contains(str[scansymbol]))
                {
                    factor += str[scansymbol];
                    scansymbol++;
                }
                int checkFactor = Factor(factor, ref subChildRef);
                if (checkFactor == 0)
                {
                    return true;
                }
                else if (checkFactor == 2)
                {
                    Console.WriteLine($"Ошибка: некорректная функция: {factor}");
                    return false;
                }
                else if (checkFactor == 3)
                {
                    Console.WriteLine($"Ошибка: некорректная переменная: {factor}");
                    return false;
                }
                else if (checkFactor == 4)
                {
                    Console.WriteLine($"Ошибка: содержание недопустимого элемента: {factor}");
                    return false;
                }
                else if (checkFactor == 5)
                {
                    Console.WriteLine($"Ошибка: некорректное выражение: {factor}");
                    return false;
                }
                else if (checkFactor == 6)
                {
                    Console.WriteLine($"Ошибка: содержание недопустимого элемента: {factor}");
                    return false;
                }
                try
                {
                    subChild.AddChild(new Node(str[scansymbol].ToString()));
                }
                catch (IndexOutOfRangeException)
                {
                    if (signs.Contains(str[scansymbol - 1]))
                    {
                        subChild.AddChild(new Node(str[scansymbol - 1].ToString()));
                    }
                };
                factor = "";
                scansymbol++;
            }
            return true;
        }

        public static int Factor(string str, ref Node child)
        {
            string identifier = "";
            string function = "";
            string number = "";
            string expression = "";
            int scansymbol = 0;
            int countBracketLeft = 0;
            int countBracketRight = 0;
            if (letterUp.Contains(str[scansymbol]))
            {
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
                if (!Function(str, ref subChildRef))
                {
                    return 2;
                }
            }
            else if (letterDown.Contains(str[scansymbol]))
            {
                if (!Identifier(str))
                {
                    return 3;
                }
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
            }
            else if (digitAndDot.Contains(str[scansymbol]))
            {
                if (!Number(str))
                {
                    return 4;
                }
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
            }
            else if (str[scansymbol] == '(')
            {
                str = str.Remove(0, 1);
                str = str.Remove(str.Length - 1, 1);
                Node subChildBracketLeft = new Node("(");
                child.AddChild(subChildBracketLeft);
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
                Node subChildBracketRight = new Node(")");
                child.AddChild(subChildBracketRight);
                if (!Expression(str, ref subChild))
                {
                    return 5;
                }
            }
            else
            {
                return 6;
            }
            return 1;
        }

        public static bool Function(string str,ref Node child)
        {
            Node subChild = new Node(str);
            ref Node subChildRef = ref subChild;
            child.AddChild(subChild);
            int scansymbol = 0;
            string nameFunction = "";
            string argFirst = "";
            string argSecond = "";
            int countBracketLeft = 0;
            int countBracketRight = 0;
            while (str[scansymbol] != '(')
            {
                nameFunction += str[scansymbol];
                scansymbol++;
            }
            if ((nameFunction == "MAX") || (nameFunction == "MIN"))
            {
                scansymbol++;
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) || ((countBracketLeft == countBracketRight) && (str[scansymbol] != ';'))))
                {
                    if (str[scansymbol] == '(')
                    {
                        countBracketLeft++;
                    }
                    else if (str[scansymbol] == ')')
                    {
                        countBracketRight++;
                    }
                    argFirst += str[scansymbol];
                    scansymbol++;
                }
                if ((argFirst.Count(f => (f == ')')) != argFirst.Count(f => (f == '('))) || !Expression(argFirst, ref subChildRef))
                {
                    return false;
                }
                scansymbol++;
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) || ((countBracketLeft == countBracketRight) && (str[scansymbol] != ')'))))
                {
                    if (str[scansymbol] == '(')
                    {
                        countBracketLeft++;
                    }
                    else if (str[scansymbol] == ')')
                    {
                        countBracketRight++;
                    }
                    argSecond += str[scansymbol];
                    scansymbol++;
                }
                if (argSecond == "" || !Expression(argSecond, ref subChildRef))
                {
                    return false;
                }
                return true;
            }
            else if (nameFunction == "COS" || nameFunction == "SIN")
            {
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) || ((countBracketLeft == countBracketRight) && (str[scansymbol] != ')'))))
                {
                    if (str[scansymbol] == '(')
                    {
                        countBracketLeft++;
                    }
                    else if (str[scansymbol] == ')')
                    {
                        countBracketRight++;
                    }
                    argFirst += str[scansymbol];
                    scansymbol++;
                }
                if (argFirst[0] == '(')
                {
                    argFirst = argFirst.Remove(0, 1);
                    argFirst = argFirst.Remove(argFirst.Length - 1, 1);
                }
                if (!Expression(argFirst, ref subChildRef))
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool Identifier(string str)
        {
            return true;
        }

        public static bool Number(string str)
        {
            int count = str.Count(f => (f == '0')) + str.Count(f => (f == '1')) + str.Count(f => (f == '2')) + str.Count(f => (f == '3')) + str.Count(f => (f == '4')) +
               str.Count(f => (f == '5')) + str.Count(f => (f == '6')) + str.Count(f => (f == '7')) + str.Count(f => (f == '8')) + str.Count(f => (f == '9')) + str.Count(f => (f == '.'));
            if ((count != str.Length) || (str.Count(f => (f == '.'))) > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}