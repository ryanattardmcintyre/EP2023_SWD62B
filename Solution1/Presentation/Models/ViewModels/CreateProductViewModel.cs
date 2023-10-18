using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public int CategoryFk { get; set; } //actual foreign key
       // public string? Image { get; set; }

        public string Supplier { get; set; }
        public double WholesalePrice { get; set; }
        public int Stock { get; set; }
    }
}
