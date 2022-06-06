using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaCorrectaVF : RespuestaCorrecta
    {
        public RespuestaCorrectaVF()
        {

        }
        public RespuestaCorrectaVF(String nombre, bool resultado) : base(nombre)
        {
            Resultado = resultado;
        }

        public bool GetCorrectAnswer()
        {
            return Resultado;
        }
        public void SetCorrectAnswer(bool newAnswer)
        {
            Resultado=newAnswer;
        }
        public override RespuestaCorrecta CopyRespuestaCorrecta()
        {
            RespuestaCorrectaVF nuevaRespuesta = new RespuestaCorrectaVF(Nombre, Resultado);
            return nuevaRespuesta;
        }
    }
}
