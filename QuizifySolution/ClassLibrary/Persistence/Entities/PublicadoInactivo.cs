using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PublicadoInactivo : EstadoQuiz
    {
        private static PublicadoInactivo _instance;
    }
}
