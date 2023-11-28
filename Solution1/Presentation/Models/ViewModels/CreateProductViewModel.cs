using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;

namespace Presentation.Models.ViewModels
{
    public class CreateProductViewModel
    {
       
        public List<Category> Categories { get; set; }


        [Required(ErrorMessage = "Name cannot be left blank")] //note: name cannot be left blank/null
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public double RetailPrice { get; set; }

        [Required(ErrorMessage = "Category was not chosen")]
        [Presentation.Validators.CategoryValidator()]
        public int CategoryFk { get; set; } //actual foreign key
       // public string? Image { get; set; }

        public string Supplier { get; set; }

        [DisplayName("Wholesale Price")]
        public double WholesalePrice { get; set; }
        public int Stock { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
