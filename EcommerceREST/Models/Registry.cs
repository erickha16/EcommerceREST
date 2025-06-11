namespace EcommerceREST.Models
{
    public class Registry
    {
        public bool Active { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime HighSystem { get; set; } = DateTime.Now;
    }
}
