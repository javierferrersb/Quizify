using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaPreguntaMultiple:RespuestaPregunta
    {
        public RespuestaPreguntaMultiple()
        {
            Respuesta = -1;
        }

        public RespuestaPreguntaMultiple(Pregunta preguntaAsociada, Realizacion realizacionCorrespondiente) : base(preguntaAsociada, realizacionCorrespondiente)
        {
            Respuesta = -1;
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
            return null;
        }
        public override void SetVFAnswer(bool answer)
        {
            
        }
        public override void SetSeleccionAnswer(int answer)
        {
            Respuesta = answer;
        }
        public override int GetSeleccionAnswer()
        {
            return Respuesta;
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
