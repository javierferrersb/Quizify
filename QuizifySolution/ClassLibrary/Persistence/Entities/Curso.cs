using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Curso
    {
        [Key]
        public int Id { get; set; }

        public String Nombre { get; set; }
        public int Icono { get; set; }
        public String Descripcion { get; set; }

        public virtual Instructor InstructorPerteneciente { get; set; }
        public virtual ICollection<Contenido> Contenidos { get; set; }
        public virtual ICollection<Estudiante> Estudiantes { get; set; }
        public virtual ICollection<Competencia> Competencias { get; set; }
    }
}
