using System.ComponentModel;

namespace PomaBrothers_Frontend.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [DisplayName("Nombre de Empresa")]
        public string BussinesName { get; set; } = null!;
        [DisplayName("Teléfono")]
        public string Phone { get; set; } = null!;
        [DisplayName("Gerente")]
        public string Manager { get; set; } = null!;
        [DisplayName("Dirección")]
        public string Address { get; set; } = null!;
        [DisplayName("CI")]
        public string Ci { get; set; } = null!;
        public DateTime RegisterDate { get; set; } = DateTime.Now;
    }
}
