using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.DTOs
{
    public class RegistryDTO
    {
        [Display(Name = "Estatus")] //Display name hace que se muestre lo inidcado en lugar de "active"
        public bool Active { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Alta del sistema")]//Display name hace que se muestre lo inidcado en lugar de "HighSystem"
        public DateTime HighSystem { get; set; } = DateTime.Now;
    }
}
