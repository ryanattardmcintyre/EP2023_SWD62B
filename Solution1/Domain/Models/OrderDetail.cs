using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderFK { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public Guid ProductFK { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }
     

    }
}
