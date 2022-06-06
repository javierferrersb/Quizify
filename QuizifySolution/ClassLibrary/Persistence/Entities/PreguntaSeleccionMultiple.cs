using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class PreguntaSeleccionMultiple : Pregunta
    {
        public virtual ICollection<ComboSeleccionMultiple> CombosSeleccionMultiple { get; set; }
    }
}