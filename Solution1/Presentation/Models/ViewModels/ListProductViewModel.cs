using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class ListProductViewModel
    {
         public Guid Id { get; set; } //B4A270B8-B756-411C-8C92-6DACF8CD4602

        [Required(ErrorMessage = "Name cannot be left blank")] //note: name cannot be left blank/null
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public double RetailPrice { get; set; }

        public string CategoryName { get; set; }

        public string? Image { get; set; }
        public int Stock { get; set; }

        public string Owner { get; set; }
    }
}
