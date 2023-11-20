using System.ComponentModel;

namespace PomaBrothers_Frontend.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; } = null!;
        [DisplayName("Apellido Paterno")]
        public string LastName { get; set; } = null!;
        [DisplayName("Apellido Materno")]
        public string? SecondLastName { get; set; }
        [DisplayName("CI")]
        public string Ci { get; set; } = null!;
        [DisplayName("Teléfono")]
        public string Phone { get; set; } = null!;
        [DisplayName("Usuario")]
        public string User { get; set; } = null!;
        [DisplayName("Contraseña")]
        public string Password { get; set; } = null!;
        [DisplayName("Rol")]
        public string Role { get; set; } = null!;
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        [DisplayName("Imagen")]
        public string? UrlImage { get; set; }
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
