
using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models

{
    public class LoginModel
    {
        [Required(ErrorMessage = "O Login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha{ get; set; }
    }
}
