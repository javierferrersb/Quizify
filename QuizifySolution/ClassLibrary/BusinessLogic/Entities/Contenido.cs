using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Contenido
    {
        public Contenido() 
        {
            Quizzes = new List<Quiz>();
        }

        public Contenido(string nombre, Curso cursoPerteneciente) : this()
        {
            Nombre = nombre;
            CursoPerteneciente = cursoPerteneciente;
        }

        public List<Quiz> GetQuizzes()
        {
            return (List<Quiz>)Quizzes;
        }

        public Contenido CopyTopic(Curso curso)
        {
            Contenido contenido = new Contenido(this.Nombre, curso);
            /*foreach(Quiz quiz in this.GetQuizzes())
            {
                contenido.Quizzes.Add(quiz.copyQuiz(quiz.GetName(), contenido));
            }*/
            List<Quiz> createdQuizzes = new List<Quiz>();
            List<BateriaPreguntas> createdBaterias = new List<BateriaPreguntas>();
            List<BateriaPreguntas> bateriasCopiadas = new List<BateriaPreguntas>();
            List<Quiz> quizesOriginales = this.GetQuizzes();
            List<Pregunta> preguntasCopiadas = new List<Pregunta>();
            Quiz createdQuiz;
            foreach (Quiz quiz in quizesOriginales)
            {
                createdQuiz = quiz.copyQuiz(quiz.GetName(), contenido);
                contenido.Quizzes.Add(createdQuiz);
                createdQuizzes.Add(createdQuiz);
                foreach(NotaBateria nb in quiz.NotasBateria)
                {
                    if (!bateriasCopiadas.Contains(nb.Bateria))
                    {
                        bateriasCopiadas.Add(nb.Bateria);
                        createdBaterias.Add(nb.Bateria.CopyBatery());
                    }
                }
            }
            int posQuiz;
            int posBateria;
            BateriaPreguntas createdBateria;
            Pregunta createdPregunta;
            NotaBateria createdNotaBateria;
            for(int i = 0; i < bateriasCopiadas.Count(); i++)
            {
                foreach(NotaBateria nbateria in bateriasCopiadas.ElementAt<BateriaPreguntas>(i).NotasBaterias)
                {
                    posQuiz = quizesOriginales.IndexOf(nbateria.Quiz); 
                    createdQuiz = createdQuizzes.ElementAt<Quiz>(posQuiz);
                    createdBateria = createdBaterias.ElementAt<BateriaPreguntas>(i);
                    createdNotaBateria = nbateria.copyNotaBateria(createdQuiz);
                    createdNotaBateria.Bateria = createdBateria;
                    createdBateria.NotasBaterias.Add(createdNotaBateria);
                    createdQuiz.NotasBateria.Add(createdNotaBateria);
                }
                foreach(Pregunta pregunta in bateriasCopiadas.ElementAt<BateriaPreguntas>(i).Preguntas)
                {
                    if (!preguntasCopiadas.Contains(pregunta))
                    {
                        preguntasCopiadas.Add(pregunta);
                        createdPregunta = pregunta.CopyQuestion();
                        foreach(BateriaPreguntas bateria in pregunta.BateriaPreguntasPertenecientes)
                        {
                            posBateria = bateriasCopiadas.IndexOf(bateria);
                            createdBateria = createdBaterias.ElementAt<BateriaPreguntas>(i);
                            createdBateria.AñadirPregunta(createdPregunta);
                            createdPregunta.BateriaPreguntasPertenecientes.Add(createdBateria);
                        }
                    }
                }
            }
            return contenido;
        }
    }
}

