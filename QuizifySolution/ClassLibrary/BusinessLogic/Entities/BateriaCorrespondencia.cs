using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class BateriaCorrespondencia : BateriaPreguntas
    {

        public BateriaCorrespondencia()
        {
        }
        public BateriaCorrespondencia(string nombre, Quiz quizPerteneciente) : base(nombre, quizPerteneciente)
        {
        }

        public override Pregunta CrearPregunta(string enunciado, Instructor autor)
        {
            Pregunta pregunta = new PreguntaCorrespondencia(enunciado, autor, this);
            return pregunta;
        }

        public override BateriaPreguntas CopyBatery()
        {
            BateriaCorrespondencia bateria = new BateriaCorrespondencia();
            bateria.Nombre = this.Nombre;
            return bateria;
        }
    }
}
