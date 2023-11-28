using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductsJsonRepository : IProducts
    {
        //GetProducts //from the list inside my json file
        //GetProduct //from the list inside my json file
        //AddProduct //to the list inside by json file.....





        string filePath;
        public ProductsJsonRepository(string pathToProductsFile) { 
            filePath = pathToProductsFile; 
        
            if (System.IO.File.Exists(filePath) == false) {

                using (var myFile = System.IO.File.Create(filePath))
                {
                    myFile.Close(); //if you dont close yourselves it will give you an error later on....
                }
            }
        
        
        }
        
        
        public void AddProduct(Product product)
        {
            product.Id = Guid.NewGuid(); //so the id is never left to 000000000000000

            var myList = GetProducts().ToList();

            myList.Add(product);

            string jsonString = JsonSerializer.Serialize(myList); //converts from an object to a json string

            System.IO.File.WriteAllText(filePath, jsonString);

        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Product? GetProduct(Guid id)
        {
           return GetProducts().SingleOrDefault(x=>x.Id == id);
        }

        public IQueryable<Product> GetProducts()
        {
            //StreamReader, System.IO.File.OpenText

            string allText = System.IO.File.ReadAllText(filePath);

            //StreamReader sr = new StreamReader(filePath);
            //allText= sr.ReadToEnd();  
            if (allText == "")
            {
                return new List<Product>().AsQueryable();
            } 
            else
            {
                //note: next line will convert from normal text into json-formatted-object
                try
                {
                    List<Product> products = JsonSerializer.Deserialize<List<Product>>(allText);
                    return products.AsQueryable();
                }
                catch
                {
                    return new List<Product>().AsQueryable();
                }
            }

        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
