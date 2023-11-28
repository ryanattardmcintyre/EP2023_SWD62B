using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using System.IO;

namespace Presentation.Controllers
{
    //note: (From today) we are making the business logic Controller, more flexible/open by asking for any instance which inherits from IProducts
    //note (is no longer applicable): (what we were doing until today): we are making the business logic Controller asking for an instance of ProductsRepository
    public class ProductsController : Controller
    {
        private IProducts _productsRepository;
        private CategoriesRepository _categoriesRepository;
        public ProductsController(IProducts productsRepository
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
                           CategoryName =  _categoriesRepository.GetCategories().SingleOrDefault(x=>x.Id == p.CategoryFk).Name
                           //p.Category.Name //above code was applied because Categories were not yet migrated to be stored in json
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
                             CategoryName = _categoriesRepository.GetCategories().SingleOrDefault(x => x.Id == p.CategoryFk).Name
                         };

            return View("Index", output); //database call + re-using the same Index View*******

        }


        //1st action called - it will load the Create View where the user can type in the data
        [HttpGet]
        [Authorize] //method is now only accessible by non-anonymous users
        public IActionResult Create() {
            //we need to give the user a list of categories to choose from

            CreateProductViewModel myModel = new CreateProductViewModel();
            //populate the Categories list
            myModel.Categories = _categoriesRepository.GetCategories().ToList();

            return View(myModel);
        }

        //2nd action called - it will receive the data about the product which will then be saved into the db
        [HttpPost]
        [Authorize] //method is now only accessible by non-anonymous users
        public IActionResult Create(CreateProductViewModel p, [FromServices]IWebHostEnvironment host)
        {
            //validation
           
            try
            {
                ModelState.Remove("Categories");
                //isValid is going to be true if all the validators show a true;
                //if one of the validators signals a non-acceptable input isValid is going to be false
                if (ModelState.IsValid == false)
                {
                    p.Categories = _categoriesRepository.GetCategories().ToList();
                    return View(p);
                }

                string relativePath = "";
                //upload of an image
                if(p.ImageFile != null)
                {
                    //1. generate a unique filename
                    string newFilename = Guid.NewGuid().ToString()
                        + Path.GetExtension(p.ImageFile.FileName); //.jpg
                    //762F8E31-5D1E-4FFF-BA30-95D63E48FE55.jpg

                    //2. form the relative path
                    relativePath = "/images/" + newFilename;

                    //3. form the absolute path
                    //   to save the physical file //C:\Users\attar\source\repos\EP2023_SWD62B\Solution1\Presentation\wwwroot
                    string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

                    //4. save the image in the folder
                    using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        p.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                }

                //set the relative path in the Image property
                _productsRepository.AddProduct(
                   new Product()
                   {
                       Description = p.Description,
                       Name = p.Name,
                       RetailPrice = p.RetailPrice,
                       WholesalePrice = p.WholesalePrice,
                       Stock = p.Stock,
                       Supplier = p.Supplier,
                       CategoryFk = p.CategoryFk,
                       Image = relativePath,
                       Owner = User.Identity.Name
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
                    CategoryName = _categoriesRepository.GetCategories().SingleOrDefault(x => x.Id == product.CategoryFk).Name,
                    Image = product.Image,
                    Owner = product.Owner
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
                if (User.Identity.Name == product.Owner)
                {
                    _productsRepository.DeleteProduct(product);
                    TempData["message"] = product.Name + " has been deleted";
                }
                else
                {
                    TempData["error"] = "Access Denied!";
                }
            }

            return RedirectToAction("Index");

            //difference between 
            //return View("Index"); => opens directly the html page
            //                        (therefore if the page uses a Model with data...thats going to raise an error)
            //vs
            //return RedirectToAction("Index"); => redirect the user to the action and NOT the page
        }



        [HttpGet]
        public IActionResult Edit(Guid id) {

            EditProductViewModel myEditingModel = new EditProductViewModel(); //This has to be changed to UpdateProductViewModel

            myEditingModel.Categories = _categoriesRepository.GetCategories().ToList();


            var myEditingProduct = _productsRepository.GetProduct(id);

            //editable
            myEditingModel.RetailPrice = myEditingProduct.RetailPrice;
            myEditingModel.WholesalePrice=     myEditingProduct.WholesalePrice;
            myEditingModel.Stock= myEditingProduct.Stock;
            myEditingModel.CategoryFk = myEditingProduct.CategoryFk;
            myEditingModel.Description=myEditingProduct.Description;
            myEditingModel.Stock = myEditingProduct.Stock;
            myEditingModel.Supplier = myEditingProduct.Supplier;
            myEditingModel.Name = myEditingProduct.Name;

            //non-editable //these are used to display them on screen/evaluate them only!
            myEditingModel.Image = myEditingProduct.Image;
            myEditingModel.Id = myEditingProduct.Id;

            return View(myEditingModel); //so that in the textboxes the original details will be shown to the user to be edited
        
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel p, [FromServices] IWebHostEnvironment host)
        {
            //validation

            try
            {
                string relativePath = "";
                //upload of an image
                if (p.ImageFile != null)
                {
                    //1. generate a unique filename
                    string newFilename = Guid.NewGuid().ToString()
                        + Path.GetExtension(p.ImageFile.FileName); //.jpg
                    //762F8E31-5D1E-4FFF-BA30-95D63E48FE55.jpg

                    //2. form the relative path
                    relativePath = "/images/" + newFilename;

                    //3. form the absolute path
                    //   to save the physical file //C:\Users\attar\source\repos\EP2023_SWD62B\Solution1\Presentation\wwwroot
                    string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

                    //4. save the image in the folder
                    using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        p.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }

                    ///////////////////////////////  delete the old image
                    string oldAbsolutePath = host.WebRootPath + "\\images\\" + System.IO.Path.GetFileName(p.Image);
                    //or call this:  _productsRepository.GetProduct(p.Id).Image
                    System.IO.File.Delete(oldAbsolutePath);

                }
                else
                {
                    //retain the old image
                    //note: old image will still be saved
                    //p << paremeter, Image contains the old relative path, p is populated because the hidden retained p.Id
                    relativePath = p.Image; // _productsRepository.GetProduct(p.Id).Image; //<<Image contains the relative path 
                }

                //set the relative path in the Image property
                _productsRepository.UpdateProduct(
                   new Product()
                   {
                       Description = p.Description,
                       Name = p.Name,
                       RetailPrice = p.RetailPrice,
                       WholesalePrice = p.WholesalePrice,
                       Stock = p.Stock,
                       Supplier = p.Supplier,
                       CategoryFk = p.CategoryFk,
                       Image = relativePath,
                       Id= p.Id //<<<<<<<<<<<<<<<<<< imp: because in the UpdateProduct(...) it will identify the product that needs to be overwritten
                   }
                    );

                TempData["message"] = "Product updated successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product not saved! Check your inputs!";
                p.Categories = _categoriesRepository.GetCategories().ToList();
                return View(p);
            }


        }




    }
}
