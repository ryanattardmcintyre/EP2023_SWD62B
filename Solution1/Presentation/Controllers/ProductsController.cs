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

            return View(myModel);
        }

        //2nd action called - it will receive the data about the product which will then be saved into the db
        [HttpPost]
        public IActionResult Create(CreateProductViewModel p)
        {
            //validation
            try
            {
                _productsRepository.AddProduct(
                   new Product()
                   {
                       Description = p.Description,
                       Name = p.Name,
                       RetailPrice = p.RetailPrice,
                       WholesalePrice = p.WholesalePrice,
                       Stock = p.Stock,
                       Supplier = p.Supplier,
                       CategoryFk = p.CategoryFk
                   }
                    );

                TempData["message"] = "Product added successfully!";

                return RedirectToAction("Index");
            }
            catch ( Exception ex )
            {
                TempData["error"] = "Product not saved! Check your inputs!";
                p.Categories = _categoriesRepository.GetCategories().ToList();
                return View(p);
            }

 
        }


        public IActionResult Details(Guid id)
        {
            var product  = _productsRepository.GetProduct(id);
            if (product == null)
            {
                TempData["error"] = "No Product found";
                return RedirectToAction("Index");
            }
            else
            {
                ListProductViewModel myProduct = new ListProductViewModel()
                {
                    Description = product.Description,
                    Id = product.Id,
                    Name = product.Name,
                    RetailPrice = product.RetailPrice,
                    Stock = product.Stock,
                    CategoryName = product.Category.Name
                };
                //AutoMapper (from nuget)

                return View(myProduct);
            }
        }


        public IActionResult Delete (Guid id)
        {
            var product = _productsRepository.GetProduct(id);
            if (product == null) { TempData["error"] = "No product to delete!"; }
            else
            {
                _productsRepository.DeleteProduct(product);
                TempData["message"] = product.Name + " has been deleted";
            }

            return RedirectToAction("Index");

            //difference between 
            //return View("Index"); => opens directly the html page
            //                        (therefore if the page uses a Model with data...thats going to raise an error)
            //vs
            //return RedirectToAction("Index"); => redirect the user to the action and NOT the page
        }


    }
}
