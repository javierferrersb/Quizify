using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class BorradorCancelado : EstadoQuiz
    {
        private BorradorCancelado()
        {
            Identificador = 0;
        }
        public static BorradorCancelado GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BorradorCancelado();
            }
            return _instance;
        }
    }
}
