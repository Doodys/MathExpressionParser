using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj wzór: ");
            Calculate(Console.ReadLine());
        }

        private static void Calculate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));

            var expressionDt = new DataTable();
            expressionDt.Columns.Add("expression", typeof(double), expression);
            expressionDt.Rows.Add();
            var result = (double)expressionDt.Rows[0][0];

            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static string ExpressionConverter(string expression)
        {
            if (expression.Contains(','))
                expression.Replace(',', '.');

            var expressionCharacters = new String(expression.Where(Char.IsLetter).ToArray());

            if(expressionCharacters.Any())
            {

            }

            return expression;
        }

        private static string AskForValues(string expression, string expressionCharacters)
        {
            Dictionary<string, double> charactersValues = new();

            foreach (var character in expressionCharacters)
            {
                Console.Write($"Podaj {character}: ");

                if(double.TryParse(Console.ReadLine(), out double result))
                {

                }
                else
                    Console.WriteLine("Nieprawidłowe dane wejściowe");
            }
        }
    }
}
