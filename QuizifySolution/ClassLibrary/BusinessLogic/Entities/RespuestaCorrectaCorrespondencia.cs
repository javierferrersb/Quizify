using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaCorrectaCorrespondencia:RespuestaCorrecta
    {
        public RespuestaCorrectaCorrespondencia()
        {
            ParesCorrectosTerminoFrase = new Dictionary<int, int>();
        }

        public RespuestaCorrectaCorrespondencia(String nombre, Dictionary<int, int> paresCorrectosTerminoFrase) : base(nombre)
        {
            ParesCorrectosTerminoFrase = paresCorrectosTerminoFrase;
        }
        public override RespuestaCorrecta CopyRespuestaCorrecta()
        {
            RespuestaCorrectaCorrespondencia nuevaRespuesta = new RespuestaCorrectaCorrespondencia();
            nuevaRespuesta.Nombre = this.Nombre;
            for(int i = 0; i < ParesCorrectosTerminoFrase.Count; i++)
            {
                nuevaRespuesta.ParesCorrectosTerminoFrase.Add(i, ParesCorrectosTerminoFrase[i]);
            }
            return nuevaRespuesta;
        }
    }
}
