using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaPreguntaVF
    {
        public RespuestaPreguntaVF()
        {

        }

        public RespuestaPreguntaVF(Pregunta preguntaAsociada, Realizacion realizacionCorrespondiente) : base(preguntaAsociada, realizacionCorrespondiente)
        {
            Respuesta = null;
        }

        public override string? GetOpenAnswer()
        {
            return null;
        }
        public override void SetOpenAnswer(String answer)
        {
        }

        public override bool? GetVFAnswer()
        {
            return Respuesta;
        }
        public override void SetVFAnswer(bool answer)
        {
            Respuesta = answer;
        }
        public override void SetSeleccionAnswer(int answer)
        {

        }
        public override int GetSeleccionAnswer()
        {
            return -2;
        }

        public override Dictionary<int, int> GetCorrespondenciaAnswer()
        {
            return null;
        }
        public override void SetCorrespondenciaAnswer(int TerminoIndex, int FraseIndex)
        {

        }
    }
}
