using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Realizacion
    {
        public Realizacion()
        {
            RespuestasPreguntas = new List<RespuestaPregunta>();
        }

        public Realizacion(int tiempoRestante, RealizaQuiz realizaQuizPerteneciente) : this()
        {
            TiempoRestante = tiempoRestante;
            RealizaQuizPerteneciente = realizaQuizPerteneciente;
            CurrentQuestion = 0;
        }
    }
}
