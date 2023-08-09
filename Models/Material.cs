using CadastroFalhas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LedAmbiental.Models
{
    public class Material
    {
        [Key]
        public int IDMaterial { get; set; }
        [ForeignKey("Entrada")]
        public int IDEntrada { get; set; }
        public virtual Entrada Entrada { get; set; }
        public string Nome { get; set; }
        public decimal Quantidade { get; set; }

        private readonly Contexto _context;

        public Material(Contexto context)
        {
            _context = context;
        }

        public Material() { }
        
    }
}