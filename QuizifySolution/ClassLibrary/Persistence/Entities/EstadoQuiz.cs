using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizify.Entities
{
    public abstract partial class EstadoQuiz
    {
        [Key]
        public int Identificador { get; set; }

    }
}
