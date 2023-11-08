using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{

    //note:  this interface will represent, any common methods
    //       needed to be able to manage the Products in the various
    //       data sources i will have in my website

    public interface IProducts
    {
        IQueryable<Product> GetProducts();
        Product? GetProduct(Guid id);
        void AddProduct(Product product);

        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
