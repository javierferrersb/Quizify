using System;
using System.Collections.Generic;
using System.Linq;

namespace Quizify.Entities
{
    public partial class RespuestaCorrectaAbierta : RespuestaCorrecta
    {
        public RespuestaCorrectaAbierta() : base()
        {
            PalabrasClave = "";
        }

        public List<String> GetOpenAnswer()
        {
            List<String> toReturn = PalabrasClave.Split(';').ToList();
            toReturn.RemoveAll(x => x == "");
            return toReturn;
        }

        public void AddOpenAnswer(String answer)
        {
            PalabrasClave = PalabrasClave + ";" + answer;
        }

        public void DeleteOpenAnswer(String answer)
        {
            PalabrasClave = PalabrasClave.Replace(";" + answer, "");
        }
        public override RespuestaCorrecta CopyRespuestaCorrecta()
        {
            RespuestaCorrectaAbierta nuevaRespuesta = new RespuestaCorrectaAbierta();
            nuevaRespuesta.Nombre = Nombre;
            nuevaRespuesta.PalabrasClave = this.PalabrasClave;
            return nuevaRespuesta;
        }
    }
}
