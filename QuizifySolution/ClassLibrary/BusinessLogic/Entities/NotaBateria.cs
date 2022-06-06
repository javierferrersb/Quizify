using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class NotaBateria
    {
        public NotaBateria()
        {

        }
        public NotaBateria(Quiz quiz, double notaBateria, int dificultad) 
        {
            Quiz = quiz;
            NotaBateriaPreguntas = notaBateria;
            Dificultad = dificultad;
        }

        public NotaBateria copyNotaBateria(Quiz quiz)
        {
            NotaBateria nb = new NotaBateria(quiz,this.NotaBateriaPreguntas, this.Dificultad);
            //nb.Bateria = this.Bateria;
            return nb;
        }
    }
}
