namespace PomaBrothers_Frontend.Models
{
    public class DeliveryDetail
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int ItemId { get; set; }
        public int InvoiceId { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public virtual Invoice Invoice { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
    }
}
