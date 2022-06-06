using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class QuizRecuperacion:Quiz
    {
        public QuizRecuperacion() : base()
        {
            QuizRecuperado = null;
        }

        public QuizRecuperacion(Quiz quiz, string nombre, DateTime fechaCreacion, Instructor autor) : base(nombre, quiz.ModoRespuesta, quiz.Tiempo, quiz.Peso, fechaCreacion, autor, quiz.NumeroIntentos, quiz.Contenido, quiz.VerResultado, quiz.NotaQuiz)
        {
            QuizRecuperado = quiz;
            foreach (NotaBateria notaBateria in quiz.NotasBateria)
            {
                this.NotasBateria.Add(notaBateria.copyNotaBateria(quiz));
            }
        }
    }
}

