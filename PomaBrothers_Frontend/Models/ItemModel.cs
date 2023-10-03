namespace PomaBrothers_Frontend.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Marker { get; set; }
<<<<<<< HEAD
        public int CapacityOrSize { get; set; }
        public string MeasurementUnit { get; set; }
=======
        public int? CapacityOrSize { get; set; }
        public string? MeasurementUnit { get; set; }
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
>>>>>>> chris
    }
}
