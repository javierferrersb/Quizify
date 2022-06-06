using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Competencia
    {
        [Key]
        public int Id { get; set; }

        public String Nombre { get; set; }
        public String Descripcion { get; set; }

        public virtual ICollection<Estudiante> Estudiantes { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }
    }
}