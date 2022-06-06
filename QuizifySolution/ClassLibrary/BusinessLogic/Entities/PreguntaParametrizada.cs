using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaParametrizada
    {
        public PreguntaParametrizada()
        {

        }

        public PreguntaParametrizada(string enunciado, Instructor autor, BateriaPreguntas bateria) : base( enunciado, autor, bateria)
        {
            Huecos = new List<HuecoParametrizada>();
        }

        public override RespuestaCorrecta CreateCorrectAnswer()
        {
            return new RespuestaCorrectaParametrizada();
        }

        public override Pregunta CopyQuestion()
        {
            PreguntaParametrizada pregunta = new PreguntaParametrizada();
            pregunta.SetStatement(this.GetStatement());
            pregunta.Autor = this.Autor;
            return pregunta;
        }
    }
}