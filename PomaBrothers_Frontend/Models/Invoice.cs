namespace PomaBrothers_Frontend.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public decimal Total { get; set; }
        public int DeliveryDetailId { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
    }
}
