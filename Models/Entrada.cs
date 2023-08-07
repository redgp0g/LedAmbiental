using CadastroFalhas.Models;
using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models
{
    public class Entrada
    {
        [Key]
        public int IDEntrada { get; set; }
        [Required(ErrorMessage = "O Caminhão é obrigatório")]
        [Display(Name = "Caminhão")]
        public string NomeCaminhao { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        public virtual IEnumerable<Material>? Materials { get; set; }

        private readonly Contexto _context;

        public Entrada(Contexto context)
        {
            _context = context;
        }

        public Entrada() { }
        
    }
}