using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0.01, 999999)]
        public decimal Price { get; set; }
    }
}
