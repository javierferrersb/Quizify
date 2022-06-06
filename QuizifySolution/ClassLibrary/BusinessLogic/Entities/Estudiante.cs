using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Estudiante
    {
        public Estudiante()
        {
            CursosInscritos = new List<Curso>();
            Competencias = new List<Competencia>();
            QuizzesRealizados = new List<RealizaQuiz>();
        }

        public Estudiante(string nombre, string email, string password) :
            base(nombre, email, password)
        {
            CursosInscritos = new List<Curso>();
            Competencias = new List<Competencia>();
            QuizzesRealizados = new List<RealizaQuiz>();
        }

        public ICollection<Curso> getAllCursos() { return this.CursosInscritos; }
    }
}

