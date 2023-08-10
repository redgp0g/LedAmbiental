using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models
{
    public class Movimentacao
    {
        [Key]
        public int IDMovimentacao { get; set; }

        [Required(ErrorMessage = "O Caminh�o � obrigat�rio")]
        [Display(Name = "Caminh�o")]
        public string Caminhao { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;
        public string Material { get; set; }
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "O Tipo � obrigat�rio")]
        [Display(Name = "Entrada/Sa�da")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "O Local � obrigat�rio")]
        public string Local { get; set; }


        private readonly Contexto _context;

        public Movimentacao(Contexto context)
        {
            _context = context;
        }

        public Movimentacao() { }
        
    }
}