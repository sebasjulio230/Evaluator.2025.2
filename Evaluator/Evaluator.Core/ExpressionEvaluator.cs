using System.Globalization;
using System.Text;

namespace Evaluator.Core
{
    public class ExpressionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var tokens = Tokenize(infix);
            var postfix = InfixToPostfix(tokens);
            return Calculate(postfix);
        }

        //tokenization
        private static Queue<string> Tokenize(string infix)
        {
            var tokens = new Queue<string>();
            var sb = new StringBuilder();

            foreach (char c in infix.Replace(" ", ""))
            {
                if (char.IsDigit(c) || c == '.')
                {
                    sb.Append(c); // build multi-digit or decimal numbers
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        tokens.Enqueue(sb.ToString());
                        sb.Clear();
                    }
                    tokens.Enqueue(c.ToString());
                }
            }
            if (sb.Length > 0) tokens.Enqueue(sb.ToString());

            return tokens;
        }

        // Infix to Postfix Conversion
        private static Queue<string> InfixToPostfix(Queue<string> infixTokens)
        {
            var output = new Queue<string>();
            var operators = new Stack<string>();

            while (infixTokens.Count > 0)
            {
                var token = infixTokens.Dequeue();

                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    output.Enqueue(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                        output.Enqueue(operators.Pop());

                    if (operators.Count == 0 || operators.Pop() != "(")
                        throw new Exception("Unbalanced parentheses");
                }
                else 
                {
                    while (operators.Count > 0 &&
                           Precedence(operators.Peek()) >= Precedence(token))
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                var op = operators.Pop();
                if (op == "(" || op == ")")
                    throw new Exception("Unbalanced parentheses");
                output.Enqueue(op);
            }

            return output;
        }

        private static int Precedence(string op) => op switch
        {
            "^" => 4,
            "*" or "/" or "%" => 3,
            "+" or "-" => 2,
            _ => 0
        };

        //Evaluates the postfix expression using a stack
        private static double Calculate(Queue<string> postfix)
        {
            var stack = new Stack<double>();

            while (postfix.Count > 0)
            {
                var token = postfix.Dequeue();

                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    if (stack.Count < 2)
                        throw new Exception("Invalid expression");

                    double b = stack.Pop();
                    double a = stack.Pop();

                    stack.Push(token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        "^" => Math.Pow(a, b),
                        _ => throw new Exception($"Invalid operator: {token}")
                    });
                }
            }

            if (stack.Count != 1)
                throw new Exception("Invalid expression");

            return stack.Pop();
        }
    }
}