using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaPregunta
    {
        [Key]
        public int Id { get; set; }

        public double? Puntuacion { get; set; }

        public virtual Pregunta PreguntaAsociada { get; set; }
        public virtual Realizacion RealizacionCorrespondiente { get; set; }
    }
}
