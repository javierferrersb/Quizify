using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaParametrizada : Pregunta
    {
        public virtual ICollection<HuecoParametrizada> Huecos { get; set; }
    }
}