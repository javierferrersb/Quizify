using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quizify.Services;

namespace Quizify.Entities
{
    public partial class BateriaVerdaderoFalso:BateriaPreguntas
    {

        public BateriaVerdaderoFalso()
        {
        }
        public BateriaVerdaderoFalso(string nombre, Quiz quizPerteneciente):base(nombre, quizPerteneciente)
        {
        }

        public override Pregunta CrearPregunta(string enunciado, Instructor autor)
        {
            Pregunta pregunta = new PreguntaVerdaderoFalso(enunciado, autor, this);
            return pregunta;
        }

        public override BateriaPreguntas CopyBatery()
        {
            BateriaVerdaderoFalso bateria = new BateriaVerdaderoFalso();
            bateria.Nombre = this.Nombre;
            return bateria;
        }

    }
}
