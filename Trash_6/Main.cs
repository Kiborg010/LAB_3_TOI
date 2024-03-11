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
        static string strMain = ""; //Строка ввода выражения для проверки
        static string letterDown = "abcdefghijklmnopqrstuvwxyz"; //Алфавит с буквами нижнего регистра
        static string letterUp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //Алфавит с буквами верхнего регистра
        static string digit = "0123456789"; //Алфавит с цифрами
        static string digitAndDot = "." + digit; //Алфавит с цифрами и с точкой. Это нам понадобится для проверки вещественных чисел
        static char[] allSigns = new char[0]; 
        static string[] addends = new string[0];
        static Tree tree = new Tree(strMain); //Создаём дерево с корнем равным начальной строке

        public static int Task(string str) //Первая исполняемая функция разбора. Нужна для разбиения начальной строки на слагаемые и базовой проверки на коррекность строки
        {
            string addend = ""; //Переменная для записи слагаемого
            int scanSymbol = 0; //Индекс сканируемого символа в строке
            string[] signs = new string[] { "+", "-", "–", "=", "!", "&", "|", ">", "<" }; //Знаки для разделения строки на слагаемые. Можно заметить, что все знаки длиной один. Небольшие пояснения: '!' - это эквивалент '!='; '&' - это логическое 'И'; '|' - это логическое 'ИЛИ'
            string strWithoutGaps = str.Replace(" ", ""); //Из введённой строки нужно удалить все пробелы
            str = strWithoutGaps;
            int countBracketRight = 0; //В выражении может несколько уровней скобок. Далее подробнее, но пока что зададим переменную для количества правосторонних скобок
            int countBracketLeft = 0; //Переменная для подсчёта левосторонних скобок
            while (scanSymbol < str.Length) //Пока сканируемый символ не дошёл до конца строки
            {
                while ((scanSymbol < str.Length) && ((countBracketLeft != countBracketRight) || 
                    ((countBracketLeft == countBracketRight) && (!signs.Contains(str[scanSymbol].ToString()))))) //Ещё один цикл с проверкой того, что сканируемый символ не дошёл до конца. При этом либо количество правых и левых скобок не совпадает; либо количества совпадают, но при этом нет знака для разделения слагаемых.
                {
                    if (str[scanSymbol] == '(') //Считаем количество левосторонних скобок
                    {
                        countBracketLeft++;
                    }
                    else if (str[scanSymbol] == ')') //Считаем количество правосторонних скобок
                    {
                        countBracketRight++;
                    }
                    addend += str[scanSymbol]; //К текущему слагаемому прибавляем считываемый символ
                    scanSymbol++; //Сдвигаем индекс сканируемого символа
                }
                if ((scanSymbol == str.Length) && (countBracketLeft != countBracketRight)) //Если всё-таки до конца строки дошли, но при этом количество правых и левых скобок не совпадает, то выкидываем ошибку типа 2, то есть ошибка неправильного написания скобок
                {
                    return 2;
                }
                Array.Resize(ref addends, addends.Length + 1); 
                addends[addends.Length - 1] = addend; //Добавляем в общее количество слагаемых 
                if ((scanSymbol < str.Length) && signs.Contains(str[scanSymbol].ToString())) //Если элемент содержится в знаках, которые разделяют слагаемые, то добавляем знак в массив, чтобы потом вывести в нужном порядке
                {
                    Array.Resize(ref allSigns, allSigns.Length + 1);
                    allSigns[allSigns.Length - 1] = str[scanSymbol];
                }
                addend = ""; //Обнуляем слагаемое
                scanSymbol++; //Снова сдвигаем индекс сканируемого символа
            } //После того, как как всю строку разобрали, то надо проверить правильность расстановки знаков, то есть исключить случаи типа '+a+b' или 'a+b+' или 'a++++b', и тому подобное
            if ((addends.Length - allSigns.Length != 1) || (addends.Contains(""))) //Если количество слагаемых не отличается от количества знаков на 1 в пользу слагаемых, или же есть пустое слагаемое, то тогда делаем вывод об ошибке ввода слагаемых. Код этой ошибки 1 
            {
                return 1;
            }
            else //Если все проверки пройдены, тогда возвращаем код ошибки 0: нет ошибки
            {
                return 0;
            }
        }

        public static bool Expression(string str, ref Node child) //Произошло как бы разделение изначального Expression на два варианта: Task и Expression. Task нужен только для первого запуска, проверки правильности именно базовой строки, деления её на слагаемые. Expression работает уже с выражениями которые стоят по дереву на уровень ниже. Также оно способно строить новые узлы дерева
        {
            string addend = ""; //В целом многое аналогично прдыдущему методу. Но есть и существенные отличия, они пояснены
            int scansymbol = 0;
            string[] signs = new string[] { "+", "-", "–", "=", "!", "&", "|", ">", "<" };
            string strWithoutGaps = str.Replace(" ", "");
            str = strWithoutGaps;
            int countBracketRight = 0;
            int countBracketLeft = 0;
            string[] addendsLocal = new string[0]; //В переданном выражении тоже есть слагаемые - локальные, их складываем в этот массив
            string[] signsLocal = new string[0]; //Также и со знаками. Их тоже записываем
            while (scansymbol < str.Length)
            {
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) || 
                    ((countBracketLeft == countBracketRight) && (!signs.Contains(str[scansymbol].ToString())))))
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
                try //Добавляем знак в дерево
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
            int counter = 0; //Индекс для того, чтобы обращаться к знакам
            foreach (string el in addendsLocal) //Перебираем слагаемые
            {
                if (!Addend(el, ref child)) //Если проверка слагаемого дала ошибку, то тоже возвращается ошибка
                {
                    return false;
                }
                Node subChild = new Node(signsLocal[counter]); //Создаём новый узел на уровень ниже того, который переданный с помощью обращения к массиву знаков по индексу
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild); //Добавляем этот узел
                counter++;
            }
            return true;
        }

        public static bool Addend(string str, ref Node child) //Метод для разбора слагаемого. Здесь передаётся ссылка на родителя, чтобы от него можно было указать потомков - новые узлы дерева
        {
            Node subChild = new Node(str);
            ref Node subChildRef = ref subChild;
            child.AddChild(subChild); //Здесь эти новые узлы и добавляются
            string factor = "";
            int scansymbol = 0;
            char[] signs = new char[] { '*', '/', '^', '$', '%' }; //Знаки тоже немного пояснить надо: '^' - возведение в степень, '$' - деление без остатка, '%' - нахождение остатка при делении
            int countBracketRight = 0;
            int countBracketLeft = 0;
            while (scansymbol < str.Length)
            {
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) ||
                    ((countBracketLeft == countBracketRight) && (!signs.Contains(str[scansymbol])))))
                {
                    if (str[scansymbol] == '(')
                    {
                        countBracketLeft++;
                    }
                    else if (str[scansymbol] == ')')
                    {
                        countBracketRight++;
                    }
                    factor += str[scansymbol]; //Слагаемое подразделяется на множители. 
                    scansymbol++;
                }
                int checkFactor = Factor(factor, ref subChildRef); //Проверяем множители
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
                try //Добавляем знак в дерево
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

        public static bool Addend(string str) //Тоже метод разбора слагаемого, но без параметра ссылки, так как в данном слагаемые ссылаются на главный корень дерева
        {
            Node subChild = new Node(str);
            ref Node subChildRef = ref subChild;
            tree.Root.AddChild(subChild); //Здесь эта ссылка и происходит
            string factor = "";
            int scansymbol = 0;
            char[] signs = new char[] { '*', '/', '^', '$', '%' };
            int countBracketRight = 0;
            int countBracketLeft = 0;
            while (scansymbol < str.Length)
            {
                while ((scansymbol < str.Length) && ((countBracketLeft != countBracketRight) ||
                    ((countBracketLeft == countBracketRight) && (!signs.Contains(str[scansymbol])))))
                {
                    if (str[scansymbol] == '(')
                    {
                        countBracketLeft++;
                    }
                    else if (str[scansymbol] == ')')
                    {
                        countBracketRight++;
                    }
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

        public static int Factor(string str, ref Node child) //Функция для разбора множителя
        {
            int scansymbol = 0;
            if (letterUp.Contains(str[scansymbol])) //Если есть большая буква, то это должна быть функция
            {
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
                if (!Function(str, ref subChildRef)) //Проверяем корректность функции. Иначе ошибка типа 2: некорректная функция
                {
                    return 2;
                }
            }
            else if (letterDown.Contains(str[scansymbol])) //Если есть маленькая буква, то это должна быть переменная
            {
                if (!Identifier(str)) //Проверяем переменную, иначе ошибка типа 3. Пока что переменная будет верная всегда
                {
                    return 3;
                }
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
            }
            else if (digitAndDot.Contains(str[scansymbol])) //Если строка содержит точку или цифру, то это число
            {
                if (!Number(str)) //Делаем проверку числа
                {
                    return 4;
                }
                Node subChild = new Node(str);
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
            }
            else if (str[scansymbol] == '(') //Если строка начинается со скобки, то это должно быть выражение вида '(<expression>)', которое нужно разбить на '(', <expression>, ')'
            {
                str = str.Remove(0, 1); //Так как мы проверили, что все скобки закрываются, то можем спокойно удалять первый и последний элементы, зная, что это точно скобки
                str = str.Remove(str.Length - 1, 1);
                Node subChildBracketLeft = new Node("(");
                child.AddChild(subChildBracketLeft);
                Node subChild = new Node(str); //Добавляем скобки в дерево
                ref Node subChildRef = ref subChild;
                child.AddChild(subChild);
                Node subChildBracketRight = new Node(")");
                child.AddChild(subChildBracketRight);
                if (!Expression(str, ref subChild)) //Проверяем корректность внутреннего выражения
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

        public static bool Function(string str,ref Node child) //Функция для обработки встроенной функции
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
            while (str[scansymbol] != '(') //Определяем имя функции, считывая название до '('
            {
                nameFunction += str[scansymbol];
                scansymbol++;
            }
            if ((nameFunction == "MAX") || (nameFunction == "MIN")) //Эти функции принимают два аргумента черз знак разделения ';'
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
                if ((argFirst.Count(f => (f == ')')) != argFirst.Count(f => (f == '('))) || !Expression(argFirst, ref subChildRef)) //Если количество скобок не совпадает или выражение внутри аргумента не корректное, то возвращаем false
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
                if (argSecond == "" || !Expression(argSecond, ref subChildRef)) //Проверяем второй аргумент на пустоту и корректность выражения
                {
                    return false;
                }
                return true;
            }
            else if (nameFunction == "COS" || nameFunction == "SIN") //У этих функций один аргумент
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

        public static bool Identifier(string str) //Переменная всегда правильная, но обработчик есть всё равно
        {
            return true;
        }

        public static bool Number(string str) //Функция для проверки числа
        {
            int count = str.Count(f => (f == '0')) + str.Count(f => (f == '1')) + //Подсчитываем количество цифр и точек в строке
                str.Count(f => (f == '2')) + str.Count(f => (f == '3')) + 
                str.Count(f => (f == '4')) + str.Count(f => (f == '5')) + 
                str.Count(f => (f == '6')) + str.Count(f => (f == '7')) + 
                str.Count(f => (f == '8')) + str.Count(f => (f == '9')) + 
                str.Count(f => (f == '.'));
            if ((count != str.Length) || (str.Count(f => (f == '.'))) > 1) //Если длина строки не совпадает с этой суммой, то понятно, что содержатся посторонние элементы. Также проверяем, чтобы не было ситуации, когда несколько точек в числе
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static string signs = "+-–=!&|><*/^$%()";
        public static void TraverseTree(Node node, int index, string strIndex)
        {
            string basa = strIndex;
            foreach (var child in node.Children)
            {
                if (child.Data != null)
                {
                    strIndex = basa + $"{index}.";
                    Console.Write($"{strIndex} {child.Data}\t");
                    index++;
                }
            }
            Console.WriteLine();
            index = 1;
            foreach (var child in node.Children)
            {
                if (child.Data != null && !signs.Contains(child.Data))
                {
                    strIndex = basa + $"{index}.";
                    TraverseTree(child, 1, strIndex);
                }
                index++;
            }
            
        }

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
                    int index = 1;
                    TraverseTree(tree.Root, index, "");
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
    }
}