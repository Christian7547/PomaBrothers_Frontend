namespace PomaBrothers_Frontend.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }
        public string Email { get; set; } = null!;
        public string Ci { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
