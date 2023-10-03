namespace PomaBrothers_Frontend.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public byte DurationWarranty { get; set; }
        public string TypeWarranty { get; set; }
        public int CategoryId { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public int ModelId { get; set; }
        public string UrlImage { get; set; }
        public virtual Category Category { get; set; }
        public virtual ItemModel ItemModel { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
