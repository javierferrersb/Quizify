using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PublicadoActivo : EstadoQuiz
    {
        private PublicadoActivo()
        {
            Identificador = 3;
        }
        public static PublicadoActivo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PublicadoActivo();
            }
            return _instance;
        }
    }
}
