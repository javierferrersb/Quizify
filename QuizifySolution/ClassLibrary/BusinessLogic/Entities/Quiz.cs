using System;
using System.Collections.Generic;

namespace Quizify.Entities
{
    public partial class Quiz
    {
        public Quiz()
        {
            Realizaciones = new List<RealizaQuiz>();
            NotasBateria = new List<NotaBateria>();
        }

        public Quiz(string nombre, int modoRespuesta, int tiempo, double peso,
            DateTime fechaCreacion, Instructor autor, int numeroIntentos,
            Contenido contenido, bool verResultado, Double notaquiz) : this()
        {
            Nombre = nombre;
            ModoRespuesta = modoRespuesta;
            Tiempo = tiempo;
            Peso = peso;
            FechaCreacion = fechaCreacion;
            Autor = autor;
            NumeroIntentos = numeroIntentos;
            Contenido = contenido;
            VerResultado = verResultado;
            PreguntasAleatorias = false;
            NotaQuiz = notaquiz;
            Estado = Borrador.GetInstance();
        }
        public List<BateriaPreguntas> GetBateriasPreguntas()
        {
            List<BateriaPreguntas> bp = new List<BateriaPreguntas>();
            foreach (NotaBateria nb in NotasBateria)
            {
                bp.Add(nb.Bateria);
            }
            return bp;
        }

        public int GetNumBateriasPreguntas()
        {
            return NotasBateria.Count;
        }

        public void AddBateriaPreguntas(BateriaPreguntas bateria)
        {
            NotaBateria nb = new NotaBateria(this, 0, 0);
            nb.Bateria = bateria;
            NotasBateria.Add(nb);
        }

        public Instructor GetAuthor()
        {
            return Autor;
        }

        public int GetAnswerMode()
        {
            return ModoRespuesta;
        }

        public void SetAnswerMode(int mode)
        {
            ModoRespuesta = mode;
        }

        public DateTime? GetStartingDate()
        {
            return FechaInicio;
        }

        public void SetStartingDate(DateTime? date)
        {
            FechaInicio = date;
        }

        public DateTime? GetClosingDate()
        {
            return FechaCierre;
        }

        public void SetClosingDate(DateTime? date)
        {
            FechaCierre = date;
        }

        public string GetName()
        {
            return Nombre;
        }

        public void SetName(string name)
        {
            Nombre = name;
        }

        public bool GetViewResult()
        {
            return VerResultado;
        }

        public void SetViewResult(bool viewResult)
        {
            VerResultado = viewResult;
        }

        public int GetNumberOfAttempts()
        {
            return NumeroIntentos;
        }

        public void SetNumberOfAttempts(int numberOfTries)
        {
            NumeroIntentos = numberOfTries;
        }

        public int GetTime()
        {
            return Tiempo;
        }

        public void SetTime(int time)
        {
            Tiempo = time;
        }

        public void SetRandomQuestions(bool order)
        {
            PreguntasAleatorias = order;
        }

        public Quiz copyQuiz(String quizName, Contenido topic)
        {
            Quiz quiz = new Quiz(quizName, this.GetAnswerMode(), this.GetTime(), this.Peso, DateTime.Now.Date, this.Autor, this.GetNumberOfAttempts(), topic, this.GetViewResult(), this.NotaQuiz);
            quiz.SetRandomQuestions(this.PreguntasAleatorias);
            /*foreach (NotaBateria notaBateria in this.NotasBateria)
            {
                quiz.NotasBateria.Add(notaBateria.copyNotaBateria(quiz));
            }*/
            return quiz;
        }

        public void Lanzar()
        {
            if (Estado is Borrador || Estado is BorradorCancelado)
            {
                Estado = PublicadoInactivo.GetInstance();
            }
        }

        public void Activar()
        {
            if (Estado is PublicadoInactivo)
            {
                Estado = PublicadoActivo.GetInstance();
            }
        }

        public void Finalizar()
        {
            if (Estado is PublicadoActivo)
            {
                Estado = Finalizado.GetInstance();
            }
        }
        public void Calificar()
        {
            if (Estado is Finalizado)
            {
                Estado = Calificado.GetInstance();
            }
        }

        public void Cancelar()
        {
            if (Estado is PublicadoInactivo)
            {
                Estado = BorradorCancelado.GetInstance();
            }
        }
    }
}
