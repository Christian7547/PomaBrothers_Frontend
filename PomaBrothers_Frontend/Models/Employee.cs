namespace PomaBrothers_Frontend.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }
        public string Ci { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string? UrlImage { get; set; }
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
