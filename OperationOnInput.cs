using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorRPN
{
    static class OperationOnInput
    {
        static OperationOnInput()
        {

        }

        // Converts the input into reversed polish notation (RPN).
        public static List<Object> ConvertToRPN(List<char> expression)
        {
            //First splits the expression into numbers and operations.
            List<Object> exp = SplitIntoNumbers(expression);
            //Defying variables.
            Stack<Char> stack = new Stack<Char>();
            List<Object> converted = new List<Object>();
            char c1;

            foreach (Object o in exp)
            {
                if(o is float)
                {
                    //If the object is a number then adds it to the output.
                    converted.Add(o);
                }
                else
                {
                    char c=Convert.ToChar(o);
                    if (c == '(')
                    {
                        //If the object is an opening bracket then push it on the top of the stack.
                        stack.Push(c);
                    }
                    else if (c == ')')
                    {
                        // If the object is an closing bracket then pops objects from the stack and adds them to the output
                        // until the object is an opening bracket at the stack. If there is no opening bracket in the stack
                        // then stops and gives the error message.
                        if (stack.Count > 0)
                        {
                            c1 = stack.Peek();
                            while (c1 != '(')
                            {
                                if (stack.Count > 0)
                                {
                                    converted.Add(stack.Pop());
                                    c1 = stack.Peek();
                                }
                                else
                                {
                                    converted.Clear();
                                    Console.WriteLine("Błędne wyrażenie.");
                                    break;
                                }
                            }
                            //Removes an opening bracket from the stack
                            stack.Pop();
                        }
                        else
                        {
                            converted.Clear();
                            Console.WriteLine("Błędne wyrażenie.");
                            break;
                        }

                    }
                    else if (stack.Count > 0)
                    {
                        //The case when the object is an operator.
                        c1 = stack.Peek();

                        // Until the operator at the top of the stack has higher or the same priority that the given operator it pops the operator
                        // from the stack and adds it to the output.

                        while ((c1 == '*' || c1 == '/') && (c == '+' || c == '-') || (c1 == '/' && c == '*') || (c1 == '*' && c == '/')
                            || (c1 == '+' && c == '-') || (c1 == '-' && c == '+')||c1==c)
                        {
                            converted.Add(stack.Pop());
                            if (stack.Count > 0)
                            {
                                c1 = stack.Peek();
                            }
                            else
                            {
                                break;
                            }
                        }
                        //When the loop stops then push the operator at the top of the stack.
                        stack.Push(c);
                    }

                    else
                    {
                        //In any other case push the object at the top of the stack.
                        stack.Push(c);
                    }
                    
                }
            }
            if(stack.Count>0)
            {
                // If there left some object in the stack then pops them and adds to the output.
                // Checks if there are no brakcets, if there are then gives the error message.
                while(stack.Count>0)
                {
                    if (stack.Peek() == '(' || stack.Peek() == ')')
                    {
                        Console.WriteLine("Błędne wyrażenie.");
                        converted.Clear();
                        break;
                    }
                    else
                    {
                        converted.Add(stack.Pop());
                    }
                }
            }
            return converted;
        }


        // It splits the given string into numbers and operations chars. Converts correctly negative numbers.
        private static List<Object> SplitIntoNumbers(List<Char> expression)
        {

            List<char> exp = new List<char>(expression);
            List<Object> expModified = new List<Object>();
            char c = exp[0];


            if (c == '-')
            {
                //The case when first char in the list is '-', i.e. the expression starts with a negative number.

                exp.Remove(c);
                if ((exp.Where(x => x == '+').Count() == 0 && exp.Where(x => x == '-').Count() == 0 &&
                    exp.Where(x => x == '*').Count() == 0 && exp.Where(x => x == '/').Count() == 0 && exp.Where(x => x == '^').Count() == 0))
                {
                    //The case when the expression is just a negative number.

                    expModified.Add(float.Parse(ToNumber(exp).Item1) * (-1));
                }
                else
                {
                    expModified.Add(float.Parse(ToNumber(exp).Item1) * (-1));
                    exp = ToNumber(exp).Item2;
                    c = exp[0];
                }

            }


            while (exp.Count > 0)
            {
                c = exp[0];

                //Checks if the given character is a digit or not.

                if (Char.IsDigit(c))
                {
                    expModified.Add(float.Parse(ToNumber(exp).Item1));
                    exp = ToNumber(exp).Item2;
                }
                else
                {
                    //Checks if the character is a bracket.
                    if (c == '('|| c==')')
                    {
                        expModified.Add(c);
                        exp.Remove(c);
                        if (exp.Count > 0)
                        {
                            c = exp[0];
                        }
                        else
                        {
                            break;
                        }
                    }
                    else 
                    {

                        int k = exp.IndexOf(c);
                        if (Char.IsDigit(exp[k + 1]))
                        {
                            // The case when the second number in binary operation is a positive number, e.g. 3*2. 
                            expModified.Add(c);
                            exp.Remove(c);
                        }
                        else if (exp[k + 1] == '(')
                        {
                            expModified.Add(c);
                            exp.Remove(c);
                            c = exp[0];
                        }
                        else
                        {
                            // The case when the second number in binary operation is a negative number, e.g. 3*-2. 
                            expModified.Add(c);
                            exp.Remove(exp[k + 1]);
                            exp.Remove(c);
                            expModified.Add(float.Parse(ToNumber(exp).Item1) * (-1));
                            exp = ToNumber(exp).Item2;
                        }
                        
                    }
                }

            }
            return expModified;
        }


        // Method converts a first number appearing in the given string into a float number.
        //
        // Assume that given string starts with a number.
        private static (string, List<Char>) ToNumber(List<Char> exp1)
        {
            List<Char> exp = new List<Char>(exp1);
            char c1 = exp[0];
            string s = "";
            while (Char.IsDigit(c1) || c1 == ',' || c1 == '.')
            {
                if (c1 == ',')
                {
                    char c2 = '.';
                    s += c2;
                    exp.Remove(c1);
                }
                else
                {
                    s += c1;
                    exp.Remove(c1);
                }

                if (exp.Count > 0)
                {
                    c1 = exp[0];
                }
                else
                {
                    break;
                }
            }


            return (s, exp);
        }


    }

}

