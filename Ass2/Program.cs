using Ass2.Models;
using Ass2.Services;
using Microsoft.EntityFrameworkCore;

namespace Ass2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<StudentDbContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
            builder.Services.AddScoped<SubjectService>();
            builder.Services.AddScoped<StudentService>();

            var app = builder.Build();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Student}/{action=List}/{id?}"
            );

            app.Run();
        }
    }
}
