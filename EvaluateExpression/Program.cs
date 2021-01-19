using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateExpression
{
    class Program
    {
        static string separator = "_";
        static void Main(string[] args)
        {
            //https://www.dcode.fr/reverse-polish-notation
            //https://www.youtube.com/watch?v=QxHRM0EQHiQ&t=398s&ab_channel=ErbComputerScience
            //https://www.youtube.com/watch?v=vnxOTu44-xg&ab_channel=ErbComputerScience
            List<string> list_expression = new List<string>();
            list_expression.Add("(2+1)+((5*2)-(5-6)*4)");
            list_expression.Add("(2+1)+((15*2)-(5-6)*4)");
            list_expression.Add("1+2*3-4");
            list_expression.Add("(4+7)+12/3");
            list_expression.Add("6*3-(4-5)+2");

            string[] lines = File.ReadAllLines("data.txt");
            for (int i = 0; i < list_expression.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("EXPRESSION:" + list_expression[i]);
                Console.WriteLine("--------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("RPN FORM: " + RPN(list_expression[i]));
                Console.WriteLine("--------------------------------------");
                Console.WriteLine(string.Equals(lines[i], solve_RPN(RPN(list_expression[i]))));
                Console.WriteLine("RESULT: " + solve_RPN(RPN(list_expression[i])));
                Console.WriteLine("***********************************************");
                Console.WriteLine();
            }

            //Console.WriteLine(solve_RPN("2_1_+_15_2_*_5_6_-_4_*_-_+_"));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Made by DragoShell");
            Console.ReadKey();
        }

        static string RPN(string expression)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Stack mystack = new Stack();
            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < expression.Length; i++)
                if (Char.IsDigit(expression[i]))
                {
                    StringBuilder sb_nr = new StringBuilder();
                    int k = i;
                    while (i < expression.Length - 1 && Char.IsDigit(expression[i + 1]))
                        i++;
                    for (int j = k; j <= i; j++)
                    {
                        sb_nr.Append(expression[j]);
                    }
                    sb.Append(sb_nr.ToString() + separator);
                }
                else
                {
                    if (expression[i] == '(')
                        mystack.Push(expression[i]);
                    else if (expression[i] == ')' && mystack.Count > 0)
                    {
                        while (mystack.Count > 0 && (char)mystack.Peek() != '(')
                        {
                            if (mystack.Count > 0 && (char)mystack.Peek() != '(')
                            {
                                sb.Append(mystack.Pop().ToString() + separator);
                            }
                        }
                        if (mystack.Count > 0)
                        {
                            mystack.Pop();
                        }

                    }
                    else if (IsOperator(expression[i]))
                    {
                        if (mystack.Count == 0)
                            mystack.Push(expression[i]);
                        else if ((Prior(expression[i]) <= Prior((char)mystack.Peek())))
                        {
                            while (mystack.Count > 0 && (Prior(expression[i]) <= Prior((char)mystack.Peek())))
                            {
                                if ((char)mystack.Peek() != '(')
                                {
                                    sb.Append(mystack.Pop().ToString() + separator);
                                }

                            }
                            mystack.Push(expression[i]);
                        }
                        else
                        {
                            mystack.Push(expression[i]);
                        }
                    }
                }


            int u = 0;
            foreach (var var in mystack)
            {
                if (u == mystack.Count - 1)
                {
                    sb.Append(var);
                }
                else
                {
                    sb.Append(var + separator);
                }
                u++;
            }


            Console.ForegroundColor = ConsoleColor.Green;
            return sb.ToString();
        }
        static bool IsOperator(char c)
        {
            return (c == '-' || c == '+' || c == '*' || c == '/');
        }
        static int Prior(char c)
        {
            switch (c)
            {
                case '+':
                    return 1;
                case '-':
                    return 1;
                case '*':
                    return 2;
                case '/':
                    return 2;
                case '^':
                    return 3;
                case ')':
                    return 0;
                case '(':
                    return 0;
                default:
                    throw new ArgumentException("Parametru Gresit");
            }
        }

        static string solve_RPN(string expression)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Stack<string> mystack = new Stack<string>();
            for (int i = 0; i < expression.Length; i++)
            {
                if (Char.IsDigit(expression[i]))
                {
                    StringBuilder sb_nr = new StringBuilder();
                    int k = i;
                    while (i < expression.Length - 1 && Char.IsDigit(expression[i + 1]))
                        i++;
                    for (int j = k; j <= i; j++)
                    {
                        sb_nr.Append(expression[j]);
                    }
                    mystack.Push(sb_nr.ToString());
                }
                else if (IsOperator(expression[i]))
                {
                    //String s = mystack.Peek();
                    int b = int.Parse(mystack.Pop());
                    int a = int.Parse(mystack.Pop());
                    if (expression[i] == '+')
                    {
                        int c = a + b;
                        mystack.Push(c.ToString());
                    }
                    else if (expression[i] == '-')
                    {
                        int c = a - b;
                        mystack.Push(c.ToString());
                    }
                    else if (expression[i] == '*')
                    {
                        int c = a * b;
                        mystack.Push(c.ToString());
                    }
                    else if (expression[i] == '/')
                    {
                        int c = a / b;
                        mystack.Push(c.ToString());
                    }

                }
            }

            return mystack.Peek().ToString();
        }
    }
}

