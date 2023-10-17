using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private ProductsRepository _productsRepository;
        public ProductsController(ProductsRepository productsRepository) { 
        _productsRepository = productsRepository;
        }

        //Model(s)- they model the database
        //ViewModel(s) - they will carry data from the backend to the frontend
        public IActionResult Index()
        {
            
            IQueryable<Product> list = _productsRepository.GetProducts();
            //convert from List<Product> >>>>>> List<ListProductViewModel>

            var output = from p in list
                       select new ListProductViewModel()
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Description = p.Description,
                           Image = p.Image,
                           RetailPrice = p.RetailPrice,
                           Stock = p.Stock,
                           CategoryName = p.Category.Name
                       };

            return View(output);
        }

        
    }
}
