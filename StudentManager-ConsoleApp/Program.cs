using StudentManager_ConsoleApp.Models;
using StudentManager_ConsoleApp.Services;
using StudentManager_ConsoleApp.Untils;

StudentServices service = new StudentServices();

while (true)
{
    Console.WriteLine("\n1. Add Student");
    Console.WriteLine("2. Show All");
    Console.WriteLine("3. Exit");

    int choice = InputHelper.InputInt("Choose: ");

    switch (choice)
    {
        case 1:
            Student s = new Student();
            s.Id = InputHelper.InputInt("Enter Id: ");
            s.Name = InputHelper.InputString("Enter Name: ");
            s.GPA = InputHelper.InputDouble("Enter GPA: ");

            service.addStudent(s);
            break;

        case 2:
            service.ShowAll();
            break;

        case 3:
            return;
    }
}