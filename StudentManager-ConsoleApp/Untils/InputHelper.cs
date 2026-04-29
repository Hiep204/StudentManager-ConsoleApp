using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager_ConsoleApp.Untils
{
    public class InputHelper
    {
        public static int InputInt(string message)
        {
            int result;
            do
            {
                Console.Write(message);
            } while (!int.TryParse(Console.ReadLine(), out result));
            return result;
        }

        public static string InputString(string message)
        {
            string? input;
            do
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
            } while (string.IsNullOrEmpty(input));
            return input;
        }
        public static double InputDouble(string message)
        {
            double result;
            do
            {
                Console.Write(message);
            } while (!double.TryParse(Console.ReadLine(), out result));
            return result; 
        }
    }
}
