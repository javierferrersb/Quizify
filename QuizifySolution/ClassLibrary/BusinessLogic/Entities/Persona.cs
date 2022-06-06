using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class Persona
    {
        public Persona()
        {

        }

        public Persona(string nombre, string email, string password) : this()
        {
            Nombre = nombre;
            Email = email;
            Password = password;
        }

    }
}
