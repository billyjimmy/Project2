using static project2.Program;

namespace project2
{
    //Note for unary operators
    //https://inspirnathan.com/posts/155-handling-unary-operations-with-shunting-yard-algorithm/
    //unary operators are followed by a number, while subtraction could be separated by spaces

    class Program
    {
        public static void Main(string[] args)
        {
            bool loop = true;
            string input;
            Tokeniser tokeniser;
            while (loop)
            {
                Console.WriteLine("\n-Menu-\n1. Tokeniser\n2. Shunting Yard\n3. Binary Evaluator\n4. Expression Evaluator\n5. Quit\n");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1": //Tokeniser Demo
                        Console.WriteLine("Please enter expression");
                        input = Console.ReadLine();
                        tokeniser = new Tokeniser(input + '#');
                        string[] token;
                        do
                        {
                            token = tokeniser.NextToken();
                            Console.WriteLine(token[0]);
                        } while (token[0] != "#");
                        break;
                    case "2": //Shunting Yard
                        Console.WriteLine("Please enter expression");
                        input = Console.ReadLine();
                        tokeniser = new Tokeniser(input + '#');
                        foreach (string[] i in tokeniser.ShuntingYard())
                        {
                            Console.Write(i[0] + " ");
                        }
                        
                        break;
                    case "3": //Binary Evaluator

                        break;
                    case "4": //Full Evaluator

                        break;
                    case "5": // Quit
                        loop = false;
                        break;

                }
            }




        }

        //Evaluates a single binary operation
        public static int BinaryEval(string operand1, string operand2, string operation)
        {
            int result = 0;
            int op1 = Convert.ToInt32(operand1);
            int op2 = Convert.ToInt32(operand2);

            if (operation == "^") 
            {
                result = op1;
                for (int i = 1; i < op2; i++)
                {
                    result *= op1;
                }

            } else if (operation == "*")
            {
                result = op1 * op2 ;
            }
            else if (operation == "/")
            {
                result = op1 / op2;
            }
            else if (operation == "+")
            {
                result = op1 + op2;
            }
            else if (operation == "-")
            {
                result = op1 - op2;
            }

            return result;
        }

        public class Tokeniser
        {
            public Tokeniser(string expression) { //Don't really need a constructor
                SetExpression(expression);
            }
            
            string expression;
            int pointer = 0;
            const string operands = "1234567890";
            const string operators = "+-=*/^";
            const string singles = "()#ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //Holds single-char tokens, if any are found, return as token immediately
            public string[] NextToken() //Returns the next operand/operator/bracket
            {

                string[] token = { "", "" };
                string[] possibleTokens = { operands, operators, singles};
               

                token[0] += expression[pointer];
                pointer++;
                if (possibleTokens[2].Contains(token[0]))
                {
                    token[1] = "2";
                    return token;
                }
                if (possibleTokens[0].Contains(token[0]))
                {
                    token[1] = "0";
                } else if (possibleTokens[1].Contains(token[0])) {
                    token[1] = "1";
                }
                
                while (pointer < expression.Length && possibleTokens[int.Parse(token[1])].Contains(expression[pointer]))
                {
                    token[0] += expression[pointer];
                    pointer++;
                }
                
                return token;
            }

            public Queue<string[]> GetAllTokens()
            {
                Queue<string[]> tokens = new Queue<string[]>();
                string[] token;
                while ((token = NextToken())[0] != "#")
                {
                    tokens.Enqueue(token);
                } 
                return tokens;
            }
            public Queue<string[]> ShuntingYard()
            {
                Queue<string[]> tokens = GetAllTokens();
                Queue<string[]> postfix = GetAllTokens();
                Stack<string[]> stack = new Stack<string[]>();
                string[] token;
                string[] temp;
                int tokenCount = tokens.Count;
                for (int i = 0; i < tokenCount; i++)
                {
                    token = tokens.Dequeue();
                    if (token[1] == "0") //If token is number
                    {
                        postfix.Enqueue(token);
                    } else if (token[0] == "(")
                    {
                        stack.Push(token);
                    } else if (token[0] == ")")
                    {
                        while ((temp = stack.Pop())[0] != "(") // Until opening bracket reached, empty stack into postfix
                        {
                            postfix.Enqueue(temp);
                        }
                        
                    } else if (token[1] == "1")
                    {
                        if (stack.Count == 0)
                        {
                            stack.Push(token);
                        } else
                        {
                            while (stack.Count > 0 && StackPriority(stack.Peek()[0]) >= IncomingPriority(token[0]))
                            {
                                postfix.Enqueue(stack.Pop());
                            }
                            stack.Push(token);
                        }


                    }
                }

                foreach (string[] i in stack) //When no tokens left, empty the stack into postfix
                {
                    postfix.Enqueue(i);
                }
                //still need to empty whatever is left of stack
                return postfix;

            }
            public int IncomingPriority(string operate)
            {
                switch (operate)
                {
                    case "^":
                        return 4;
                        break;
                    case "*":
                        return 2;
                        break;
                    case "/":
                        return 2;
                        break;
                    case "+":
                        return 1;
                        break;
                    case "-":
                        return 1;
                        break;
                }
                return 0;
            }
            public int StackPriority(string operate)
            {
                switch (operate)
                {
                    case "^":
                        return 3;
                        break;
                    case "*":
                        return 2;
                        break;
                    case "/":
                        return 2;
                        break;
                    case "+":
                        return 1;
                        break;
                    case "-":
                        return 1;
                        break;
                }
                return 0;
            }
            public void SetExpression(string expression)
            {
                pointer = 0;

                //To handle unary operators
                if (expression[0] == '+' || expression[0] == '-')
                {
                    expression = "0" + expression;
                }

                this.expression = expression + "#";
            }

            public void ResetPointer()
            {
                this.pointer = 0;
            }
        }
    }
}