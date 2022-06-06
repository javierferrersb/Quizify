using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaPreguntaCorrespondencia : RespuestaPregunta
    {
        public RespuestaPreguntaCorrespondencia() : base()
        {
            Respuesta = new Dictionary<int, int>();
        }
        public RespuestaPreguntaCorrespondencia(Pregunta preguntaAsociada, Realizacion realizacionCorrespondiente) : base(preguntaAsociada, realizacionCorrespondiente)
        {
            Respuesta = new Dictionary<int, int>();
            int numFrases = ((PreguntaCorrespondencia)preguntaAsociada).CombosCorrespondencia.Count;
            for(int i = 0; i < numFrases; i++)
            {
                Respuesta.Add(i, -1);
            }
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

        }
        public override int GetSeleccionAnswer()
        {
            return -2;
        }

        public override Dictionary<int, int> GetCorrespondenciaAnswer()
        {
            return Respuesta;
        }
        public override void SetCorrespondenciaAnswer(int TerminoIndex, int FraseIndex)
        {
            Respuesta[TerminoIndex] = FraseIndex;
        }
    }
}
