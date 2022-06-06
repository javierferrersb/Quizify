using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaAbierta
    {
        public PreguntaAbierta()
        {

        }

        public PreguntaAbierta( string enunciado, Instructor autor, BateriaPreguntas bateria) : base(enunciado, autor, bateria) 
        {
            AddAnswer();
        }

        public override RespuestaCorrecta CreateCorrectAnswer()
        {
            return new RespuestaCorrectaAbierta(); 
        }

        public override Pregunta CopyQuestion()
        {
            PreguntaAbierta pregunta = new PreguntaAbierta();
            pregunta.SetStatement(this.GetStatement());
            pregunta.Autor = this.Autor;
            pregunta.respuestaCorrecta = respuestaCorrecta.CopyRespuestaCorrecta();
            return pregunta;
        }
    }
}
