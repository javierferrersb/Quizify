using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Instructor
    {
        public Instructor() 
        {
            Cursos = new List<Curso>();
            Facturas = new List<Factura>();
        }

        public Instructor(string nombre, string email, string password) :
            base(nombre, email, password)
        {
            this.numQuizesRestantes = 0;
            Cursos = new List<Curso>();
            Facturas = new List<Factura>();
        }

    }
}
