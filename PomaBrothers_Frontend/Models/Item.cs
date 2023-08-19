namespace PomaBrothers_Frontend.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string Marker { get; set; }
        public string? Capacity { get; set; }
        public string Warranty { get; set; }
        public int CategoryId { get; set; }
        public DateTime RegisterDate { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();
        public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
