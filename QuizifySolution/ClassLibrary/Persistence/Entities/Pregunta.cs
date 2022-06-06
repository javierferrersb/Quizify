using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Pregunta
    {
        [Key]
        public int Id { get; set; }

        public String Enunciado { get; set; }
        public virtual Instructor Autor { get; set; }
        public virtual Contenido ContenidoPertenece { get; set; }
        public virtual RespuestaCorrecta respuestaCorrecta { get; set; }
        public virtual ICollection<BateriaPreguntas> BateriaPreguntasPertenecientes { get; set; }
        public virtual ICollection<MaterialMultimedia> Materiales { get; set; }
    }
}

