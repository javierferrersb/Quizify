using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Factura
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaCompra { get; set; }

        public virtual Instructor Instructor { get; set; }
        public virtual Oferta Oferta { get; set; }
    }
}
