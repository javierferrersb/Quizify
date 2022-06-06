using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Quiz
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ModoRespuesta { get; set; }
        public int Tiempo { get; set; }
        public double Peso { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaCierre { get; set; }
        public virtual Instructor Autor { get; set; }
        public int NumeroIntentos { get; set; }
        public bool VerResultado { get; set; }
        public double NotaQuiz { get; set; }
        public bool PreguntasAleatorias { get; set; }
        public virtual Contenido Contenido { get; set; }
        public virtual ICollection<NotaBateria> NotasBateria { get; set; }
        public virtual ICollection<RealizaQuiz> Realizaciones { get; set; }
        public virtual EstadoQuiz Estado { get; set; }
    }
}
