using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //Dependency Injection = is a design pattern which manages the creation of instances (objects) efficiently
    public class ProductsRepository
    {
        private ShoppingCartDbContext _shoppingCartDbContext;
        public ProductsRepository(ShoppingCartDbContext shoppingCartDbContext) { //assume that shoppingCartDbContext was already created
            _shoppingCartDbContext = shoppingCartDbContext;
        }

        //Read
        public IQueryable<Product> GetProducts()
        {
            //1000000
            return _shoppingCartDbContext.Products ; 
        }
        //IQueryable you can view the data inside while debugging
       

        //Creation
        public void AddProduct(Product product) {
            _shoppingCartDbContext.Products.Add(product);
            _shoppingCartDbContext.SaveChanges(); //commits to the database
        }

        //Update
        public void UpdateProduct(Product product) { }

        //Delete
        public void DeleteProduct(Product product) { }

    }
}
