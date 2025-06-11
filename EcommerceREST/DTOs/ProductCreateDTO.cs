using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceREST.DTOs
{
    public class ProductCreateDTO : RegistryDTO
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }


        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }


        [Display(Name = "Descripción")]
        public string Description { get; set; }


        [Display(Name = "Categoría")] //Para cuando se selccione en el formulario
        [Required(ErrorMessage = "Debes seleccionar una categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Marca")]//Para cuando se selccione en el formulario
        [Required(ErrorMessage = "Debes seleccionar una marca")]
        public int BrandId { get; set; }
    }
}
