using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaPreguntaAbierta : RespuestaPregunta
    {
        public string? Respuesta { get; set; }
    }
}
