namespace PomaBrothers_Frontend.Models.DTOModels
{
    public class SaleDTO
    {
        public decimal Total { get; set; }
        public DateTime RegisterDate { get; set; }
        public List<ProductPurchasedDTO> Products { get; set; }
    }
}
