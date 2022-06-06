using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Contenido
    {
        [Key]
        public int Id { get; set; }

        public String Nombre { get; set; }

        public virtual Curso CursoPerteneciente { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}

