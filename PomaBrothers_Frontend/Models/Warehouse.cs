namespace PomaBrothers_Frontend.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
