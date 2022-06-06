using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class RespuestaPreguntaCorrespondencia : RespuestaPregunta
    {
        public Dictionary<int, int> Respuesta;
    }
}
