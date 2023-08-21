
using Microsoft.EntityFrameworkCore;

namespace LedAmbiental.Models

{
    [Keyless]
    public class ApplicationUser
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome{ get; set; }
        public string Funcao { get; set; }

        public ApplicationUser()
        {

        }
    }
}
