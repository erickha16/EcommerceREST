using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.DTOs
    
    //DTO para crear una nueva marca
{
    public class BrandCreateDTO
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage ="La imagen es requerida")]
        public IFormFile File { get; set; } // Para recibir el archivo de imagen
    }
}

