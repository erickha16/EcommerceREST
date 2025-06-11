using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.Models
{
    public class Category: Registry //Hereda los atributos de la clase Registry
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //Dice que una categoria puede tener muchos productos (una colección de productos)
        //Se utiliza IEnumerable sobre List por cuestiones de rendimiento y flexibilidad
        public IEnumerable<Product>? Products { get; set; } //Propiedad de navegación
    }
}
