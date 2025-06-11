using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceREST.DTOs
{
    public class ProductDTO: RegistryDTO
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string? Name { get; set; }


        [Display(Name = "Precio")]
        [Range(0.01, double.MaxValue, ErrorMessage ="El precio debe ser mayor a 0")]
        public decimal Price { get; set; }


        [Display(Name = "Descripción")]
        public string? Description { get; set; }


        [Display(Name = "Categoría")] //Para cuando se selccione en el formulario
        public int CategoryId { get; set; }

        [Display(Name = "Categoría")] //Para mostrar en la vista
        public string? Category { get; set; }

        [Display(Name = "Marca")]//Para cuando se selccione en el formulario
        public int BrandId { get; set; }

        [Display(Name = "Marca")] //Para mostrar en la vista
        public string? Brand { get; set; }
    }
}
