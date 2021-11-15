using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorRPN
{
    delegate float BinaryOperation(float a, float b);

    static class CalculatorManager
    {
        
        static CalculatorManager()
        {
            
        }

        //Permorms the calculations.
        public static float Calculate(List<char> expression)
        {
            // Conversion of the expression into RPN.
             List<Object> exp = OperationOnInput.ConvertToRPN(expression);
            

            // Declaring a dictionary which maps character of arithmetic operation to its results.
            Dictionary<char, BinaryOperation> map = new Dictionary<char, BinaryOperation>();
            map.Add('+', (a, b) => a + b);
            map.Add('-', (a, b) => a - b);
            map.Add('*', (a, b) => a * b);
            map.Add('/', Division);

            //Defying variables;
            float result = 0;
            float x = 0;
            float y = 0;
            Stack<float> stack = new Stack<float>();

            foreach(Object o in exp)
            {
                // If the object in the list is a number then push it on the top of teh stack.
                BinaryOperation op;
                if (o is float)
                {
                    stack.Push(Convert.ToSingle(o));
                }
                else
                {
                    // If the object is an operator then pops two numbers from the stack and calculate
                    // the value of respective operation and push it into stack.
                    map.TryGetValue(Convert.ToChar(o), out op);
                    x = stack.Pop();
                    y = stack.Pop();
                    stack.Push(op(y,x));
                }
            }
            if (stack.Count > 0)
            {
                result = stack.Pop();
            }
            return result;
        }

        //Auxilary method for division of two numbers.
        private static float Division(float a, float b)
        {
            if(b!=0)
            {
                return a / b;
            }
            else
            {
                Console.WriteLine("Nie można dzielić przez 0.");
                return float.NaN;
            }
        }
    }
}
