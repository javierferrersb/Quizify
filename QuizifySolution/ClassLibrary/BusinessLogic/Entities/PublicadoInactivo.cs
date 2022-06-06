using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PublicadoInactivo : EstadoQuiz
    {
        private PublicadoInactivo()
        {
            Identificador = 2;
        }
        public static PublicadoInactivo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PublicadoInactivo();
            }
            return _instance;
        }
    }
}
