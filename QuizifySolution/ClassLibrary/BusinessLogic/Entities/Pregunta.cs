using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public abstract partial class Pregunta
    {
        public Pregunta()
        {
            BateriaPreguntasPertenecientes = new List<BateriaPreguntas>();
            Materiales = new List<MaterialMultimedia>();
        }

        public Pregunta( string enunciado, Instructor autor, BateriaPreguntas bateria) : this()
        {
            Enunciado = enunciado;
            Autor = autor;
            ContenidoPertenece = new Contenido();
            BateriaPreguntasPertenecientes.Add(bateria);
        }

        public abstract RespuestaCorrecta CreateCorrectAnswer();

        public void AddAnswer()
        {
            respuestaCorrecta = CreateCorrectAnswer();
        }

        public RespuestaCorrecta GetAnswer()
        {
            return respuestaCorrecta;
        }

        public string GetStatement()
        {
            return Enunciado;
        }

        public void SetStatement(string statement)
        {
            Enunciado = statement;
        }

        public abstract Pregunta CopyQuestion();
    }
}

