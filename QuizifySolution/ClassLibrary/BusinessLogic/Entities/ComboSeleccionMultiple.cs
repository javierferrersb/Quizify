using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class ComboSeleccionMultiple
    {
        public ComboSeleccionMultiple()
        {

        }

        public ComboSeleccionMultiple(string textoRespuesta, bool esRespuestaCorrecta, PreguntaSeleccionMultiple preguntaCorrespondiente) : this()
        {
            TextoRespuesta = textoRespuesta;
            EsRespuestaCorrecta = esRespuestaCorrecta;
            PreguntaCorrespondiente = preguntaCorrespondiente;
        }

    }
}