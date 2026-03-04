using System;
using System.Globalization;

namespace BTVN1
{
    internal class Course
    {
        public int Id { get; set; } // Fixed IDE1006: Naming rule violation
        public string Title { get; set; } = string.Empty; // Fixed CS8618: Added default value and IDE1006: Naming rule violation
        public DateTime StartDate { get; set; } // Fixed IDE1006: Naming rule violation
        internal Validation Validator { get => _validator; set => _validator = value; }
        private readonly Validation _validator = new Validation(); // Fixed IDE0044: Made field readonly and IDE0090: Simplified 'new' expression

        public virtual void Input()
        {
            Console.WriteLine("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || !Validator.validateIntInput(id, 1, 1000)) // Fixed CS8604: Null reference argument
            {
                Console.WriteLine("Invalid ID.");
                return;
            }
            Id = id;

            Console.WriteLine("Enter course title: ");
            string? title = Console.ReadLine();
            if (string.IsNullOrEmpty(title) || !Validator.validateStringInput(title, 1, 100)) // Fixed CS8604: Null reference argument
            {
                Console.WriteLine("Invalid title.");
                return;
            }
            Title = title;

            Console.WriteLine("Enter start date (dd/MM/yyyy): ");
            string? dateInput = Console.ReadLine();
            if (string.IsNullOrEmpty(dateInput) || !DateTime.TryParseExact(dateInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate)) // Fixed CS8604: Null reference argument
            {
                Console.WriteLine("Invalid date format. Please use dd/MM/yyyy.");
                return;
            }
            StartDate = startDate;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, StartDate: {StartDate:dd/MM/yyyy}"; // Fixed IDE0071: Simplified interpolation
        }

        public void ReadFromLine(string line) // Fixed IDE1006: Naming rule violation
        {
            if (string.IsNullOrEmpty(line)) // Fixed CS8604: Null reference argument
            {
                throw new ArgumentException("Input line cannot be null or empty.", nameof(line));
            }

            string[] items = line.Split('|');
            if (items.Length < 3)
            {
                throw new FormatException("Input line is not in the correct format.");
            }

            Id = int.Parse(items[0]); // Assuming input is valid, otherwise add validation
            Title = items[1];
            StartDate = DateTime.ParseExact(items[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}

