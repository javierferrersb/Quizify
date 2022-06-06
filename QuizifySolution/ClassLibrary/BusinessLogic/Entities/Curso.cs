using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Curso
    {
        public Curso() 
        { 
            Contenidos = new List<Contenido>();
            Estudiantes = new List<Estudiante>();
            Competencias = new List<Competencia>();
        }

        public Curso(string nombre, int icono, string desc, Instructor inst) : this() 
        {
            Nombre = nombre;
            Icono = icono;
            Descripcion = desc;
            InstructorPerteneciente = inst;
        }

        public Curso CopyCourse(String courseName, Instructor creator)
        {
            Curso curso = new Curso(courseName, this.Icono, this.Descripcion, creator);
            foreach (Competencia competencia in this.Competencias)
            {
                curso.Competencias.Add(competencia);
            }
            foreach (Contenido contenido in this.Contenidos)
            {
                curso.Contenidos.Add(contenido.CopyTopic(curso));
            }
            return curso;
        }

    }
}
