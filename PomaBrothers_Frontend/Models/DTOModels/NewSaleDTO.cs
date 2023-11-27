namespace PomaBrothers_Frontend.Models.DTOModels
{
    public class NewSaleDTO
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public List<Item> ItemsId { get; set; }
        public decimal Total { get; set; }

    }
}
