
using System.ComponentModel.DataAnnotations;

namespace LedAmbiental.Models

{
    public class LoginModel
    {
        [Required(ErrorMessage = "O Login � obrigat�rio")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A Senha � obrigat�ria")]
        [DataType(DataType.Password)]
        public string Senha{ get; set; }
    }
}
