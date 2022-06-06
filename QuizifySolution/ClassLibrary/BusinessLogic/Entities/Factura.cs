using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Factura
    {
        public Factura() 
        {

        }

        public Factura(DateTime fechaCompra, Instructor instructor, Oferta oferta) : this()
        {
            FechaCompra = fechaCompra;
            Instructor = instructor;
            Oferta = oferta;
        }

    }
}
