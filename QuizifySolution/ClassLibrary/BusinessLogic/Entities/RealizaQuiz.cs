using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RealizaQuiz
    {
        public RealizaQuiz()
        {
            Realizaciones = new List<Realizacion>();
        }

        public RealizaQuiz(Quiz quiz, Estudiante estudiante) : this()
        {
            Quiz = quiz;
            Estudiante = estudiante;
        }

        public int GetNumRealizaciones()
        {
            return Realizaciones.Count;
        }

        public Realizacion GetUnfinishedRealizacion()
        {
            foreach(Realizacion realizacion in Realizaciones)
            {
                if (!realizacion.EsTerminado)
                {
                    return realizacion;
                }
            }
            return null;
        }

        public Realizacion GetLastRealizacion()
        {
            Realizacion realizacion = Realizaciones.Last<Realizacion>();
            return realizacion;
        }
    }
}
