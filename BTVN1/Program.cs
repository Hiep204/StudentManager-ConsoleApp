using System;

namespace BTVN1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manage manager = new Manage();
            int choice;

            do
            {
                Console.WriteLine("\n========= MENU =========");
                Console.WriteLine("1. Add a course");
                Console.WriteLine("2. Display course list");
                Console.WriteLine("3. Search courses by date range");
                Console.WriteLine("4. Sort courses by title");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option (1-5): ");

                bool isValid = int.TryParse(Console.ReadLine(), out choice);
                if (!isValid)
                {
                    Console.WriteLine(" Invalid choice. Please enter a number between 1 and 5.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        manager.AddCourse();
                        break;
                    case 2:
                        manager.DisplayCourses();
                        break;
                    case 3:
                        manager.FindCoursesByDateRange();
                        break;
                    case 4:
                        manager.SortCoursesByTitle();
                        break;
                    case 5:
                        Console.WriteLine(" Exiting the program.");
                        return;
                    default:
                        Console.WriteLine(" Invalid choice. Please select from 1 to 5.");
                        break;
                }
            } while (true);
        }
    }
}
