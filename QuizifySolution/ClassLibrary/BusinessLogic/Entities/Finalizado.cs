using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Finalizado : EstadoQuiz
    {
        private Finalizado()
        {
            Identificador = 4;
        }
        public static Finalizado GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Finalizado();
            }
            return _instance;
        }
    }
}
