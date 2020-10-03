using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;

namespace Ticker
{
    internal class Program
    {
        private static readonly Dictionary<Type, Func<string, object>> Parsers = new Dictionary<Type, Func<string, object>>
        {
            [typeof(int)] = s => int.Parse(s),
            [typeof(double)] = s => double.Parse(s, CultureInfo.InvariantCulture),
        };
        
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите параметры маятника");
                var ballsCount = ReadParameter("Количество шаров", 1, int.MaxValue);
                var mass = ReadParameter("Масса одного шара", 0, double.MaxValue);
                var radius = ReadParameter("Радиус шара", 0, double.MaxValue);
                var bounce = ReadParameter("Коэффициент упругости", 0, 1.0);
                var threadLength = ReadParameter("Длина нити", radius, double.MaxValue);
                var viscosity = ReadParameter("Коэффициент вязкости среды", 0, 1.0);
                var ticker = new Ticker(ballsCount, mass, radius, bounce, threadLength, viscosity);

            
                while (true)
                {
                    Console.WriteLine("Провести опыт?\nEnter - для подтверждения\nN - создать новый маятник");
                    if (Console.ReadLine() == "N")
                        break;
                
                    Console.WriteLine(ticker.MakeTest());
                }
            }
        }
        
        public static T ReadParameter<T>(string name, T minValue , T maxValue) 
            where T: IComparable<T>
        {
            var type = typeof(T);
            var isCorrect = false;
            var result = default(T);
            while (!isCorrect)
            {
                try
                {
                    Console.Write($"{name} - ");
                    var input = Console.ReadLine();
                    result = (T)Parsers[type](input);
                    if (result.CompareTo(minValue) >= 0 && result.CompareTo(maxValue) <= 0)
                        isCorrect = true;
                    else
                        Console.WriteLine($"Неверное значение. Введите значение от {minValue} до {maxValue}");
                }
                catch (Exception)
                {
                    Console.WriteLine("Неверный формат.");
                }
            }

            return result;
        }
    }
}