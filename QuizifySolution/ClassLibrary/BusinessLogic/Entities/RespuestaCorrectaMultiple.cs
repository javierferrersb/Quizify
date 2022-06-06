using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaCorrectaMultiple:RespuestaCorrecta
    {
        public RespuestaCorrectaMultiple()
        {

        }
        public RespuestaCorrectaMultiple(String nombre) : base(nombre)
        {
        }
        public override RespuestaCorrecta CopyRespuestaCorrecta()
        {
            throw new NotImplementedException();
        }
    }
}
