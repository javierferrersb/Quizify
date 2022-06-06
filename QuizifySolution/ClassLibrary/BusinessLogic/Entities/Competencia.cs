using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Competencia
    {
        public Competencia()
        {
            Estudiantes = new List<Estudiante>();
            Cursos = new List<Curso>();
        }

        public Competencia(string nombre, string descripcion) : this()
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

    }
}