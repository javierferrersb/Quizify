using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaVerdaderoFalso
    {
        public PreguntaVerdaderoFalso()
        {

        }

        public PreguntaVerdaderoFalso( string enunciado, Instructor autor, BateriaPreguntas bateria) : base( enunciado, autor, bateria)
        {
            AddAnswer();
        }

        public override RespuestaCorrecta CreateCorrectAnswer()
        {
            return new RespuestaCorrectaVF("res",true);
        }

        public RespuestaCorrectaVF GetAnswerVF()
        {
            return (RespuestaCorrectaVF)respuestaCorrecta;
        }

        public override Pregunta CopyQuestion()
        {
            PreguntaVerdaderoFalso pregunta = new PreguntaVerdaderoFalso();
            pregunta.SetStatement(this.GetStatement());
            pregunta.Autor = this.Autor;
            pregunta.respuestaCorrecta = respuestaCorrecta.CopyRespuestaCorrecta(); 
            return pregunta;
        }
    }
}