using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Borrador : EstadoQuiz
    {
        private Borrador()
        {
            Identificador = 1;
        }
        public static Borrador GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Borrador();
            }
            return _instance;
        }
    }
}
