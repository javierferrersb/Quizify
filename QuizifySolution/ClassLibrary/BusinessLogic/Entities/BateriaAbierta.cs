using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class BateriaAbierta : BateriaPreguntas
    {

        public BateriaAbierta()
        {
        }

        public BateriaAbierta(string nombre, Quiz quizPerteneciente) : base(nombre, quizPerteneciente)
        {
        }

        public override Pregunta CrearPregunta(string enunciado, Instructor autor)
        {
            Pregunta pregunta = new PreguntaAbierta(enunciado, autor, this);
            return pregunta;
        }

        public override BateriaPreguntas CopyBatery()
        {
            BateriaAbierta bateria = new BateriaAbierta();
            bateria.Nombre = this.Nombre;
            return bateria;
        }
    }
}
