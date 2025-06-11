using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.Models
{
    public class Brand: Registry //Hereda los atributos de la clase Registry
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string LogoUrl { get; set; }

        //Propiedad de navegación
        public IEnumerable<Product>? Products { get; set; } //Una marca puede tener muchos productos (una colección de productos)
    }
}
