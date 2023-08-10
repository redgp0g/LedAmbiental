using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models
{
    public class Movimentacao
    {
        [Key]
        public int IDMovimentacao { get; set; }

        [Required(ErrorMessage = "O Caminhão é obrigatório")]
        [Display(Name = "Caminhão")]
        public string Caminhao { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;
        public string Material { get; set; }
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "O Tipo é obrigatório")]
        [Display(Name = "Entrada/Saída")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "O Local é obrigatório")]
        public string Local { get; set; }


        private readonly Contexto _context;

        public Movimentacao(Contexto context)
        {
            _context = context;
        }

        public Movimentacao() { }
        
    }
}