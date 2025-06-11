using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.DTOs
//DTO para mostrar la información de una marca
{
    public class BrandDTO : RegistryDTO 
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public string LogoUrl { get; set; }
    }
}
