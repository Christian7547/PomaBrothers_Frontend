namespace PomaBrothers_Frontend.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string BussinesName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Manager { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IdSupplier { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
    }
}
