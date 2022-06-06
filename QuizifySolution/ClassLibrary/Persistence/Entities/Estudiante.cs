using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Estudiante : Persona
    {
        public virtual ICollection<Curso> CursosInscritos { get; set; }
        public virtual ICollection<Competencia> Competencias { get; set; }
        public virtual ICollection<RealizaQuiz> QuizzesRealizados { get; set; }
    }
}

