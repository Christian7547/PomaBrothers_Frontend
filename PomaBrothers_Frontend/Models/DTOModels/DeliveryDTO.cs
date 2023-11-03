using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PomaBrothers_Frontend.Models.DTOModels
{
    public class DeliveryDTO
    {
        public int SupplierId { get; set; }
        public double Total { get; set; }
        public List<Item> Items { get; set; }
        public List<decimal> PurchasePrices { get; set; }
        public int WarehouseId { get; set; }

        //public static DeliveryDTO FromJson(JObject jsonObject)
        //{


        //    return new DeliveryDTO
        //    {
        //        SupplierId = (int)jsonObject["supplierId"],
        //        Total = (double)jsonObject["total"],
        //        Items = jsonObject["items"].ToObject<List<Item>>(),
        //        PurchasePrices = jsonObject["purchasePrices"].ToObject<List<decimal>>(),
        //        WarehouseId = (int)jsonObject["warehouseId"]
        //    };
        //}
    }
}
