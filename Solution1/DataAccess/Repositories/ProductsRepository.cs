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
       
        public Product? GetProduct(Guid id)
        {
            //Select Top 1 Product.Name, Product.Id, product.... From Products Where Product.id = id
            return GetProducts().SingleOrDefault(x => x.Id == id);
        }


        //Creation
        public void AddProduct(Product product) {
            _shoppingCartDbContext.Products.Add(product);
            _shoppingCartDbContext.SaveChanges(); //commits to the database
        }

        //Update
        public void UpdateProduct(Product product) {

            var originalProduct = GetProduct(product.Id); //constraint : the id is never going to change
            if (originalProduct != null)
            {
                originalProduct.Supplier = product.Supplier;
                originalProduct.RetailPrice = product.RetailPrice;
                originalProduct.WholesalePrice = product.WholesalePrice;
                originalProduct.Name = product.Name;
                originalProduct.CategoryFk = product.CategoryFk;
                originalProduct.Description = product.Description;
                originalProduct.Stock = product.Stock;

                _shoppingCartDbContext.SaveChanges();

            }
        
        }

        //Delete
        public void DeleteProduct(Product product) {
            _shoppingCartDbContext.Products.Remove(product);
            _shoppingCartDbContext.SaveChanges();
        
        }

    }
}
