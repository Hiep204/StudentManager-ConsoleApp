using StudentManager_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager_ConsoleApp.Services
{
    public class StudentServices
    {
        private List<Student> list = new List<Student>();

        public void addStudent(Student student)
        {
            list.Add(student);
        }
        public void ShowAll()
        {
            foreach(var s in list)
            {
                Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, GPA: {s.GPA}");
            }
        }
    }
}
