using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PomaBrothers_Frontend.Models
{
    public class Section
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ModelId { get; set; }
        public int ModelQuantity { get; set; }
        public string Name { get; set; } = null!;
        public virtual ItemModel ItemModel { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
    }
}
