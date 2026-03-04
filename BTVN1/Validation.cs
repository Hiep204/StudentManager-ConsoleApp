namespace BTVN1
{
    internal class Validation
    {
        public bool validateIntInput(int input, int min, int max)
        {
            if (input < min || input > max)
            {
                Console.WriteLine($"Input must be between {min} and {max}. Please try again.");
                return false;
            }
            return true;
        }

        public bool validateStringInput(string input, int min, int max)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be null or empty. Please enter a valid string.");
                return false;
            }

            if (input.Length < min || input.Length > max)
            {
                Console.WriteLine($"Input length must be between {min} and {max} characters. Please try again.");
                return false;
            }

            return true;
        }
        public static int GetIntValue(int min, int max, string mess)
        {
            int value;
            while (true)
            {
                Console.Write(mess);
                string input = Console.ReadLine();

                try
                {
                    value = int.Parse(input);

                    if (value < min || value > max)
                        throw new OverflowException($"Number must be between {min} and {max}");

                    return value;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format. Please enter a valid integer.");
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public static string GetStringValue(int min, int max, string mess)
        {
            string value;
            while(true)
            {
                try {
                    Console.Write(mess);
                    value = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Input cannot be null or empty.");
                    if (value.Length < min || value.Length > max)
                        throw new ArgumentOutOfRangeException($"Input length must be between {min} and {max} characters.");
                    return value;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
        }

    }
}
