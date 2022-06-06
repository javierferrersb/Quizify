using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class MaterialMultimedia
    {
        [Key]
        public int Id { get; set; }

        public Byte[] Archivo { get; set; }

        public virtual Pregunta PreguntaPerteneciente { get; set; }
    }
}

