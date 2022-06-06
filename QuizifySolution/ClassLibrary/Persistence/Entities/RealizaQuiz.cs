using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RealizaQuiz
    {
        [Key]
        public int Id { get; set; }
        
        public virtual Quiz Quiz { get; set; }
        public virtual Estudiante Estudiante { get; set; }

        public virtual ICollection<Realizacion> Realizaciones { get; set; }
    }
}
