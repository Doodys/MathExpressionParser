using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MathExpressionParser
{
    static class ExpressionParser
    {
        static void Main()
        {
            Console.WriteLine("Podaj wzór: ");
            Calculate(Console.ReadLine());
            Console.ReadKey();
        }

        private static void Calculate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));

            expression = ExpressionConverter(expression);

            try
            {
                var expressionDt = new DataTable();
                expressionDt.Columns.Add("expression", typeof(double), expression);
                expressionDt.Rows.Add();
                var result = (double)expressionDt.Rows[0][0];
                Console.WriteLine($"Wynik: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string ExpressionConverter(string expression)
        {
            if (expression.Contains(','))
                expression.Replace(',', '.');

            var expressionCharacters = new String(expression.Where(Char.IsLetter).ToArray());

            if(expressionCharacters.Any())
            {
                expression = AskForValues(expression, expressionCharacters);
            }

            return expression;
        }

        private static string AskForValues(string expression, string expressionCharacters)
        {
             var charactersValues = new Dictionary<string, double>();

            foreach (var character in expressionCharacters)
            {
                if(charactersValues.ContainsKey(character.ToString()))
                    continue;

                var shouldRepeat = true;

                while(shouldRepeat)
                {
                    Console.Write($"Podaj {character}: ");

                    if(double.TryParse(Console.ReadLine(), out double result))
                    {
                        charactersValues.Add(character.ToString(), result);
                        shouldRepeat = false;
                    }
                    else
                        Console.WriteLine("Nieprawidłowe dane wejściowe");
                }
            }

            return InsertValuesIntoExpression(charactersValues, expression);
        }

        private static string InsertValuesIntoExpression(Dictionary<string, double> values, string expression)
        {
            foreach(var value in values)
            {
                expression = expression.Replace(value.Key, value.Value.ToString());
            }

            return expression;
        }
    }
}
