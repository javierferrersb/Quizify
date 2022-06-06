using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    /*Por defecto, las preguntas no permiten que haya varias respuestas correctas*/
    public partial class BateriaSeleccionMultiple : BateriaPreguntas
    {
        public BateriaSeleccionMultiple()
        {
        }
        public BateriaSeleccionMultiple(string nombre, Quiz quizPerteneciente) : base(nombre, quizPerteneciente)
        {

        }


        public override Pregunta CrearPregunta(string enunciado, Instructor autor)
        {
            Pregunta pregunta = new PreguntaSeleccionMultiple(enunciado, autor, this);
            return pregunta;
        }

        public override BateriaPreguntas CopyBatery()
        {
            BateriaSeleccionMultiple bateria = new BateriaSeleccionMultiple();
            bateria.Nombre = this.Nombre;
            return bateria;
        }
    }
}
