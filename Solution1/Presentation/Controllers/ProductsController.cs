using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private ProductsRepository _productsRepository;
        private CategoriesRepository _categoriesRepository;
        public ProductsController(ProductsRepository productsRepository
            , CategoriesRepository categoriesRepository) { 
            _productsRepository = productsRepository;
            _categoriesRepository = categoriesRepository;
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

        public IActionResult Search(string keyword)
        {
            IQueryable<Product> list = _productsRepository.GetProducts().
                Where(x=>x.Name.StartsWith(keyword) || x.Description.Contains(keyword)); //no database call; updating the sql-statement
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

            return View("Index", output); //database call + re-using the same Index View*******

        }


        //1st action called - it will load the Create View where the user can type in the data
        [HttpGet]
        public IActionResult Create() {
            //we need to give the user a list of categories to choose from

            CreateProductViewModel myModel = new CreateProductViewModel();
            //populate the Categories list
            myModel.Categories = _categoriesRepository.GetCategories().ToList();

            return View();
        }

        //2nd action called - it will receive the data about the product which will then be saved into the db
        [HttpPost]
        public IActionResult Create(Product p)
        { }



    }
}
