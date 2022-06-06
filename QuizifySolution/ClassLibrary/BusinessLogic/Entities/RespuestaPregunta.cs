using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public abstract partial class RespuestaPregunta
    {
        public RespuestaPregunta()
        {

        }

        public RespuestaPregunta(Pregunta preguntaAsociada, Realizacion realizacionCorrespondiente) : this()
        {
            PreguntaAsociada = preguntaAsociada;
            RealizacionCorrespondiente = realizacionCorrespondiente;
        }

        public abstract string? GetOpenAnswer();
        public abstract void SetOpenAnswer(String answer);

        public abstract bool? GetVFAnswer();
        public abstract void SetVFAnswer(bool answer);
        public abstract void SetSeleccionAnswer(int answer);
        public abstract int GetSeleccionAnswer();
        public abstract Dictionary<int, int> GetCorrespondenciaAnswer();
        public abstract void SetCorrespondenciaAnswer(int TerminoIndex, int FraseIndex);
    }
}
