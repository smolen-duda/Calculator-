using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace CalculatorRPN
{

    // Calculator using reversed polish notation to perform calculations.
    // Works correctly with negative numbers float pointing numbers with ',' or '.' as a separator, but gives result with '.'.
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            // Main loop.
            while (true)
            {
                
                Console.WriteLine("Podaj działanie do wykonania (Type 'q' to exit): ");
                string expresion = Console.ReadLine();
                if (!(expresion == "q"))
                {
                    char[] exp = expresion.ToCharArray();
                    List<Char> exp1 = exp.ToList();
                    bool check = exp1.All(c => Char.IsDigit(c) || c == ',' || c == '.' || c == '+' || c == '-' 
                    || c == '*' || c == '/' || c == '^'|| c=='('||c==')');
                    if (check)
                    {
                        Console.WriteLine(CalculatorManager.Calculate(exp1));

                    }
                    else
                    {
                        Console.WriteLine("Niepoprawne działanie.");
                    }

                }
                else
                {
                    break;

                }
            }
        }
    }
}
