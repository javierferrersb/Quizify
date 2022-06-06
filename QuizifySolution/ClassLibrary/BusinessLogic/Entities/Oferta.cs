using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Oferta
    {
        public Oferta() { 
            Facturas = new List<Factura>();
        }

        public Oferta(string nombre, DateTime fechaVigenciaInicio, DateTime fechaVigenciaFinal, int numeroDeQuizes, int almacenamiento, double precio) : this()
        {
            Nombre = nombre;
            FechaVigenciaInicio = fechaVigenciaInicio;
            FechaVigenciaFinal = fechaVigenciaFinal;
            NumeroDeQuizes = numeroDeQuizes;
            Almacenamiento = almacenamiento;
            Precio = precio;
        }

    }
}

