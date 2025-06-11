using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceREST.Models
{
    public class Product : Registry //Hereda los atributos de la clase Registry
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        //Propiedades de navegación
        public Category Category { get; set; } //Una categoría puede tener muchos productos (una colección de productos)
        public Brand Brand { get; set; } //Una marca puede tener muchos productos (una colección de productos)

    }
}