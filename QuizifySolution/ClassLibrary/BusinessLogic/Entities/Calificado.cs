using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Calificado : EstadoQuiz
    {
        private Calificado()
        {
            Identificador = 5;
        }
        public static Calificado GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Calificado();
            }
            return _instance;
        }
    }
}
