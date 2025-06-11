using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.DTOs
{
    public class CategoryDTO: RegistryDTO
    {
        public int Id { get; set; }

        [Display(Name ="Nombre")] // Display name hace que se muestre "Nombre" en lugar de "Name"
        [Required(ErrorMessage ="El nombre es requerido")] // Mensaje de error personalizado si el campo es requerido en caso de que no se llene
        public string Name { get; set; }

        [Display(Name = "Productos asociados")]
        public int? ProductCount { get; set; }
    }
}
