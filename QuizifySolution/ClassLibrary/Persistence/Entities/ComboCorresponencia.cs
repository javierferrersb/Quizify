using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class ComboCorrespondencia
    {
        [Key]
        public int Id { get; set; }

        public String Termino { get; set; }
        public String Frase { get; set; }

        public virtual PreguntaCorrespondencia PreguntaCorrespondiente { get; set; }
    }
}
