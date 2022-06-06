using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Persona
    {
        [Key]
        public String Email { get; set; }

        public String Nombre { get; set; }
        public String Password { get; set; }
    }
}
