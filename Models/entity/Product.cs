
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthAPI.models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Units { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public DateTime CreatedOn { get; set; }

        public Product()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }
}