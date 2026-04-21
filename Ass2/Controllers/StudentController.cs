using Ass2.Models;
using Ass2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ass2.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _service;

        public StudentController(StudentService service)
        {
            _service = service;
        }

        public IActionResult List(string keyword)
        {
            var students = string.IsNullOrWhiteSpace(keyword)
                ? _service.GetAllStudent()
                : _service.sreachStudent(keyword);

            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _service.createStudent(student);
                return RedirectToAction("List");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _service.getStudentbyId(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _service.updateStudent(student.Id, student);
                return RedirectToAction("List");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _service.getStudentbyId(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.deleteStudent(id);
            return RedirectToAction("List");
        }

        public IActionResult Detail(int id)
        {
            var student = _service.getStudentbyId(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}