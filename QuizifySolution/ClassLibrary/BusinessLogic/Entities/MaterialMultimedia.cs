using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class MaterialMultimedia
    {
        public MaterialMultimedia() { 

        }

        public MaterialMultimedia(byte[] archivo, Pregunta preguntaPerteneciente) : this()
        {
            Archivo = archivo;
            PreguntaPerteneciente = preguntaPerteneciente;
        }

    }
}

