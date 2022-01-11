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
            // wykonaj funkcję Calculate wymaganą w zadaniu
            Calculate(Console.ReadLine());
            // nie wyłączaj okna konsoli od razu po obliczeniach
            Console.ReadKey();
        }

        private static void Calculate(string expression)
        {
            // sprawdź, czy wejściowy wzór nie jest pusty
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));

            // przekonwertuj wzór wczytując potrzebne wartości odpowiadające literom
            // oraz podmieniając je we wzorze
            expression = ExpressionConverter(expression);

            // wykonaj obliczenie wykorzystując DataTable oraz przeprowadź walidację
            // łapiąc błedy przy obliczeniu w bloku catch
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
            // podmień wszelkie przecinki wartości liczbowych na kropki
            // aby uniknąć problemów z wartościami zmiennoprzecinkowymi
            if (expression.Contains(','))
                expression.Replace(',', '.');

            // zbierz ze wzoru do stringa wszystkie elementy przedstawione literami
            var expressionCharacters = new String(expression.Where(Char.IsLetter).ToArray());

            // sprawdź, czy we wzorze były takie elementy
            if(expressionCharacters.Any())
            {
                // poproś użytkownika o wartości elementów
                expression = AskForValues(expression, expressionCharacters);
            }

            return expression;
        }

        private static string AskForValues(string expression, string expressionCharacters)
        {
            // stwórz słownik litera - wartość
            var charactersValues = new Dictionary<string, double>();

            // poproś o wartość każdego z elementów
            foreach (var character in expressionCharacters)
            {
                // aby uniknąć powtórzeń w słowniku sprawdź, czy dana litera już się
                // w nim znajduje
                if(charactersValues.ContainsKey(character.ToString()))
                    continue;

                var shouldRepeat = true;

                // pętla w przypadku wprowadzenia złej wartości (najczęściej nie-liczbowej)
                while(shouldRepeat)
                {
                    Console.Write($"Podaj {character}: ");

                    // walidacja, czy wprowadzona wartość jest liczbą zmiennoprzecinkową
                    if(double.TryParse(Console.ReadLine(), out double result))
                    {
                        // uzupełnienie słownika
                        charactersValues.Add(character.ToString(), result);
                        shouldRepeat = false;
                    }
                    else
                        Console.WriteLine("Nieprawidłowe dane wejściowe");
                }
            }

            // wprowadzenie zebranych wartości do wzoru
            return InsertValuesIntoExpression(charactersValues, expression);
        }

        private static string InsertValuesIntoExpression(Dictionary<string, double> values, string expression)
        {
            // podmiana liter we wzorze na podane wcześniej wartości
            foreach(var value in values)
            {
                expression = expression.Replace(value.Key, value.Value.ToString());
            }

            return expression;
        }
    }
}
