using App.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public virtual DbSet<Customer> Customers { get; set; }
        //public virtual DbSet<Product> Products { get; set; }
        //public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<AppIdentityUser> AspNetUsers { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Following> Followings { get; set; }

    }
}
