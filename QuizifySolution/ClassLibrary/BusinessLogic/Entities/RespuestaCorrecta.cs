using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public abstract partial class RespuestaCorrecta
    {
        public RespuestaCorrecta()
        {

        }

        public RespuestaCorrecta(String nombre) : this()
        {
            this.Nombre = nombre;
        }
        public abstract RespuestaCorrecta CopyRespuestaCorrecta();
    }
}
