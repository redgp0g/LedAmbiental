using CadastroFalhas.Models;
using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models
{
    public class Material
    {
        [Key]
        public int IDMaterial { get; set; }
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