namespace PomaBrothers_Frontend.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateTime RegisterDate { get; set; }
        public decimal Total { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
    }
}
