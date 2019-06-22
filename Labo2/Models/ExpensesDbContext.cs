using Labo2.Models;
using Microsoft.EntityFrameworkCore;
using System;
//using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Models
{
    public class ExpensesDbContext : DbContext
    {
        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasIndex(u => u.Username).IsUnique();
            });
            
            //asa fac sa fie unica o cheie primara compusa (cu HasKey)
            //builder.Entity<UserUserRole>(entity => {
            //    entity.HasKey(u => new { u.UserId, u.UserRoleId }).IsUnique();
            //});

            builder.Entity<Comment>()
                .HasOne(e => e.Expense)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.AddedBy)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User_UserRole> User_UserRoles { get; set; }
    }
}
