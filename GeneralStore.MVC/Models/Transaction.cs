using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStore.MVC.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } //= new List<Product>();
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
        [Required]
        public int Quantity { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateOfTransaction { get; set; }
        [Display(Name ="Total Cost")]
        public decimal Total
        {
            get
            {
                if (Product == null)
                {
                    return 0;
                }
                decimal totalCost = Quantity * Product.Price;//get cost price times amount purchase
                return totalCost;
                //decimal tax = Decimal.Multiply(totalCost, 0.07m);// add sales tax after discount
                //return Math.Round(totalCost + tax);//Round to the nearest .00 place
            }
        }
    }
}