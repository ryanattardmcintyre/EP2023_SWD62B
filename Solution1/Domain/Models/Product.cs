using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {

        /*//note: this is injected in the schema generation process...
         * 
         * protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Node>().Property(x => x.ID).HasDefaultValueSql("NEWID()");
            }
         */



        public Product()
        { 
          //  Id=Guid.NewGuid();
        }


        [Key] //note: by default .netcore, takes the property called "Id" and assumes that that is the primary key
        public Guid Id { get; set; } //B4A270B8-B756-411C-8C92-6DACF8CD4602

        [Required(ErrorMessage ="Name cannot be left blank")] //note: name cannot be left blank/null
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0,double.MaxValue)]
        public double RetailPrice { get; set; }

        [ForeignKey("Category")]
        public int CategoryFk { get; set; } //actual foreign key
        public virtual Category Category { get; set; } // navigational property
        //note: when this property does not populate itself with the data from the other
        //      table (e.g. Categories) normally its becuase LazyLoading was not enabled
  
        public string? Image { get; set; }

        public string Supplier { get; set; }
        public double WholesalePrice { get; set; }
        public int Stock { get; set; }
    }
}
