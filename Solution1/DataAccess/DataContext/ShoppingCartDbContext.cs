using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.DataContext
{
    public class ShoppingCartDbContext: IdentityDbContext //it gives you the ability to use the IdentityManagement module
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
           : base(options)
        {
        }

        //note: class names are given in singular
        //note: table names/properties are given in plural

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }


        //OnModelCreating will make sure that for every new product that i create (even) in the database ...a guid will be automatically generated


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        } 

    }
}
