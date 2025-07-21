using Exam_Portal.Models;
using Exam_Portal_Web_Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam_Portal.Models
{
    public class Appdbcontext : IdentityDbContext<User>
    {
        public Appdbcontext(DbContextOptions<Appdbcontext> options):base(options)
        {
            
        }
        public DbSet<User> users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ExamStatus> ExamStatuses { get; set; }

    }
}
