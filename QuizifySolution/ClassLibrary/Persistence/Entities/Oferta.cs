using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Oferta
    {
        [Key]
        public int Id { get; set; }

        public String Nombre { get; set; }
        public DateTime FechaVigenciaInicio { get; set; }
        public DateTime FechaVigenciaFinal { get; set; }
        public int NumeroDeQuizes { get; set; }
        public int Almacenamiento { get; set; }
        public double Precio { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; }
    }
}

