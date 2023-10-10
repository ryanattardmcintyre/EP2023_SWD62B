using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    public class ShoppingCartDbContext: IdentityDbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
           : base(options)
        {
        }
    }
}
