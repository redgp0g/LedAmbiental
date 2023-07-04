using CadastroFalhas.Models;
using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models
{
    public class Entrada
    {
        [Key]
        public int IDEntrada { get; set; }
        [Required]
        [Display(Name = "Nome do Caminhão")]
        public string NomeCaminhao { get; set; }
        [Required]
        public DateTime Data { get; set; } = DateTime.Now;


        private readonly Contexto _context;

        public Entrada(Contexto context)
        {
            _context = context;
        }

        public Entrada() { }
        
    }
}