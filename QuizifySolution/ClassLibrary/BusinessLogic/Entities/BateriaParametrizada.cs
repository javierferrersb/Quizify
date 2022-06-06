using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class BateriaParametrizada : BateriaPreguntas
    {

        public BateriaParametrizada()
        {
        }
        public BateriaParametrizada(string nombre, Quiz quizPerteneciente) : base(nombre, quizPerteneciente)
        {
        }

        public override Pregunta CrearPregunta(string enunciado, Instructor autor)
        {
            Pregunta pregunta = new PreguntaParametrizada(enunciado, autor, this);
            return pregunta;
        }

        public override BateriaPreguntas CopyBatery()
        {
            BateriaParametrizada bateria = new BateriaParametrizada();
            bateria.Nombre = this.Nombre;
            return bateria;
        }
    }
}
