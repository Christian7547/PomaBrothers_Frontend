namespace PomaBrothers_Frontend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
