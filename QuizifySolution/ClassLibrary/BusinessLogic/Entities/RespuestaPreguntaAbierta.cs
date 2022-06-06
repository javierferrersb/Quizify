using System;
using System.Collections.Generic;

namespace Quizify.Entities
{
    public partial class RespuestaPreguntaAbierta
    {
        public RespuestaPreguntaAbierta()
        {

        }

        public RespuestaPreguntaAbierta(Pregunta preguntaAsociada, Realizacion realizacionCorrespondiente) : base(preguntaAsociada, realizacionCorrespondiente)
        {
            Respuesta = "";
        }

        public override string? GetOpenAnswer()
        {
            return Respuesta;
        }
        public override void SetOpenAnswer(String answer)
        {
            Respuesta = answer;
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
            return null;
        }
        public override void SetCorrespondenciaAnswer(int TerminoIndex, int FraseIndex)
        {

        }
    }
}
