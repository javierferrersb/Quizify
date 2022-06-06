using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Realizacion
    {
        [Key]
        public int Id { get; set; }

        public int TiempoRestante { get; set; }
        public double Puntuacion { get; set; }
        public bool EsTerminado { get; set; }
        public int CurrentQuestion { get; set; }

        public virtual RealizaQuiz RealizaQuizPerteneciente { get; set; }
        public virtual ICollection<RespuestaPregunta> RespuestasPreguntas { get; set; }
    }
}
