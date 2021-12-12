using ASP_Project_MuhammadKhalid.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASP_Project_MuhammadKhalid.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define composite primary keys.
            modelBuilder.Entity<ClientAccount>()
                .HasKey(ps => new { ps.clientID, ps.accountNum });

            // Define foreign keys here. Do not use foreign key annotations.
            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.Client)
                .WithMany(p => p.ClientAccount)
                .HasForeignKey(fk => new { fk.clientID })
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.BankAccount)
                .WithMany(p => p.ClientAccount)
                .HasForeignKey(fk => new { fk.accountNum })
                .OnDelete(DeleteBehavior.Restrict);
        }
        }
}
