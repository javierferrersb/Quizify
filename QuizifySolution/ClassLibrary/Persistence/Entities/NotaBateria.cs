using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class NotaBateria
    {
        [Key]
        public int Id { get; set; }
        public int Dificultad { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual BateriaPreguntas Bateria { get; set; }
        public double NotaBateriaPreguntas { get; set; }
    }
}
