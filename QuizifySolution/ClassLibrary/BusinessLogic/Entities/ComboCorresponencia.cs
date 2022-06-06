using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class ComboCorrespondencia
    {
        public ComboCorrespondencia()
        {

        }

        public ComboCorrespondencia(string termino, string frase, PreguntaCorrespondencia preguntaCorrespondiente) : this()
        {
            Termino = termino;
            Frase = frase;
            PreguntaCorrespondiente = preguntaCorrespondiente;
        }

    }
}
