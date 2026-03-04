using System;
using System.Collections.Generic;
using System.Linq;

namespace BTVN1
{
    internal class Manage
    {
        private List<Course> courses = new List<Course>();

        public void AddCourse()
        {
            Console.Write("Enter 'c' for Course, 'o' for CourseOnline: ");
            string type = Console.ReadLine();

            Course course;

            if (type.ToLower() == "c")
            {
                course = new Course();
            }
            else if (type.ToLower() == "o")
            {
                course = new CourseOnline();
            }
            else
            {
                Console.WriteLine(" Invalid type. Please enter 'c' or 'o'.");
                return;
            }

            course.Input(); 
            courses.Add(course);
        }

        public void DisplayCourses()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("The course list is empty.");
                return;
            }

            Console.WriteLine("\n--- Course List ---");
            foreach (var course in courses)
            {
                Console.WriteLine(course.ToString());
            }
        }

        public void FindCoursesByDateRange()
        {
            try
            {
                Console.Write("Enter start date (dd/MM/yyyy): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);

                Console.Write("Enter end date (dd/MM/yyyy): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);

                var foundCourses = courses
                    .Where(c => c.startDate >= startDate && c.startDate <= endDate)
                    .ToList();

                if (foundCourses.Count == 0)
                {
                    Console.WriteLine("No courses found in the given date range.");
                    return;
                }

                Console.WriteLine("\n--- Matching Courses ---");
                foreach (var course in foundCourses)
                {
                    Console.WriteLine(course.ToString());
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(" Invalid date format. Please use dd/MM/yyyy.");
            }
        }

        public void SortCoursesByTitle()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("The list is empty. Cannot sort.");
                return;
            }

            var sortedCourses = courses.OrderBy(c => c.title).ToList();

            Console.WriteLine("\n--- Course List Sorted by Title ---");
            foreach (var course in sortedCourses)
            {
                Console.WriteLine(course.ToString());
            }
        }
        public void readCoursesFromFile(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        Course c = null;
                        if (line[0] == 'C')
                        {
                            c = new Course();
                        }else
                        {
                            c = new CourseOnline();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading from the file: {ex.Message}");
            }
        }
    }
}
