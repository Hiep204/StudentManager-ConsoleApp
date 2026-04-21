using Ass2.Models;

namespace Ass2.Services
{
    public class StudentService
    {
        private readonly StudentDbContext _context;
        public StudentService(StudentDbContext context)
        {
            _context = context;
        }
        public List<Student> GetAllStudent()
        {
            return _context.Students.ToList();
        }
        public Student? getStudentbyId(int id)
        {
            return _context.Students.FirstOrDefault(c => c.Id == id);
        }
        public void updateStudent(int id, Student student)
        {
            var oh = _context.Students.FirstOrDefault(c => c.Id == id);
            if(oh == null)
            {
                return;
            }
            oh.FullName = student.FullName;
            oh.Dob = student.Dob;
            _context.SaveChanges();
        }
        public void createStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }
        public void deleteStudent(int id)
        
        {
            var oh = _context.Students.FirstOrDefault(c => c.Id == id);
            if (oh == null)
            {
                return;
            }
            _context.Remove(oh);
            _context.SaveChanges();
        }
        public List<Student> sreachStudent(string keyword)
        {
            return _context.Students.Where(x => x.FullName.ToLower().Contains(keyword)).ToList();
        }
    }
  
}
