namespace project2
{
    //Note for unary operators
    //https://inspirnathan.com/posts/155-handling-unary-operations-with-shunting-yard-algorithm/
    //unary operators are followed by a number, while subtraction could be separated by spaces


    //Store tokens in list/array, not string, store the value and type e.g. operator/operand
    //Infix expression will be in the format: operand , operator , operand, ...
    //So then turn to postfix, then eval
    //-2+2 1st is unary 2nd is binary, when you reach the second, it bust be binary becuase 2- -2+ is not valid.
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please enter expression:");
            string expression = Console.ReadLine();
            Tokeniser tokeniser = new Tokeniser(expression);

            string token;         

            do
            {
                token = tokeniser.NextToken();
                Console.WriteLine(token);
            } while (token != "#");


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
            //test git
            string expression;
            int pointer = 0;
            const string numbers = "1234567890";
            const string operators = "+-=*/^";
            const string singles = "()#"; //Holds single-char tokens, if any are found, return as token immediately
            public string NextToken() //Returns the next operand/operator/bracket
            {
                string token = expression[pointer].ToString();
                string tokenType = "";

                //Skip spaces, this allows multi-digit postfix numbers by using spaces to separate them
                while (token == " ") 
                {
                    token = expression[++pointer].ToString();
                }           

                //If is a single-char token, like a bracket, return.
                if (singles.Contains(token))
                {
                    return token;
                }

                
                if (numbers.Contains(token))
                {
                    tokenType = numbers;
                } else if (operators.Contains(token))
                {
                    tokenType = operators;
                }

                //This allows for multi-char operands and operators, by joining adjacent operands/operators into a single token.
                while (pointer < expression.Length - 1 && tokenType.Contains(expression[++pointer]))
                {
                    token += expression[pointer];
                }

                return token;
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