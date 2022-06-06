using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class ComboSeleccionMultiple
    {
        [Key]
        public int Id { get; set; }

        public String TextoRespuesta { get; set; }
        public Boolean EsRespuestaCorrecta { get; set; }

        public virtual PreguntaSeleccionMultiple PreguntaCorrespondiente { get; set; }
    }
}