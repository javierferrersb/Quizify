using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaCorrectaParametrizada:RespuestaCorrecta
    {
        public RespuestaCorrectaParametrizada()
        {

        }

        public RespuestaCorrectaParametrizada(String nombre) : base(nombre)
        {

        }
        public override RespuestaCorrecta CopyRespuestaCorrecta()
        {
            throw new NotImplementedException();
        }
    }
}
