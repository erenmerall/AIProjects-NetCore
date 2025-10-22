using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project1_ApiDemo.Entities;

namespace NetCoreAI.Project1_ApiDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=ERENMERAL;initial catalog=ApiAIDb;integrated security=true;TrustServerCertificate=True");
            }
        }
        public DbSet<Customer> Customers { get; set; }

    }
}