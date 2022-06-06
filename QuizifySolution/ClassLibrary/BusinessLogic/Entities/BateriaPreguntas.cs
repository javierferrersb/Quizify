using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public abstract partial class BateriaPreguntas
    {
        public BateriaPreguntas()
        {
            Preguntas = new List<Pregunta>();
            NotasBaterias = new List<NotaBateria>();
        }

        public BateriaPreguntas(string nombre, Quiz quizPerteneciente) : this()
        {
            Preguntas = new List<Pregunta>();
            NotasBaterias = new List<NotaBateria>();
            Nombre = nombre;
            NotaBateria notabateria = new NotaBateria(quizPerteneciente,1,1);
            NotasBaterias.Add(notabateria);
        }
        public void AñadirPregunta(Pregunta p)
        {
            this.Preguntas.Add(p);
        }

        public abstract Pregunta CrearPregunta(string enunciado, Instructor autor);

        public Pregunta GetRandomQuestion()
        {
            Random random = new Random();
            int index = random.Next(Preguntas.Count);
            return Preguntas.ElementAt<Pregunta>(index);
        }

        public abstract BateriaPreguntas CopyBatery();
    }
}
