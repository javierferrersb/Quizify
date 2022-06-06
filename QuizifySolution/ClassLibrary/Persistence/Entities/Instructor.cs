using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Instructor : Persona
    {
        public virtual ICollection<Curso> Cursos { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }

        public int numQuizesRestantes { get; set; }
    }
}
