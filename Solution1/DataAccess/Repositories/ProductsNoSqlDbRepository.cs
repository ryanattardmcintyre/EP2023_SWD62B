using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductsNoSqlDbRepository : IProducts
    {
        public void AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Product? GetProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
