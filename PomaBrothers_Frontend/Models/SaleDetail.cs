namespace PomaBrothers_Frontend.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int IdSale { get; set; }
        public int IdItem { get; set; }
        public decimal Subtotal { get; set; }
        public virtual Item IdItemNavigation { get; set; } = null!;
        public virtual Sale IdSaleNavigation { get; set; } = null!;
    }
}
