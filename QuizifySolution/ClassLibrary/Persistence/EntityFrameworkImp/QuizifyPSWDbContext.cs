using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quizify.Entities;

namespace Quizify.Persistence
{
    public class QuizifyPSWDbContext : DbContextPSW
    {
        public QuizifyPSWDbContext() : base("Name=QuizifyPSWDbConnection")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        static QuizifyPSWDbContext()
        {
            Database.SetInitializer<QuizifyPSWDbContext>(new DropCreateDatabaseIfModelChanges<QuizifyPSWDbContext>());
        }



        public IDbSet<Persona> Personas { get; set; }
        public IDbSet<Estudiante> Estudiantes { get; set; }
        public IDbSet<Instructor> Instructores { get; set; }
        public IDbSet<Oferta> Ofertas { get; set; }
        public IDbSet<Factura> Facturas { get; set; }
        public IDbSet<Curso> Cursos { get; set; }
        public IDbSet<Contenido> Contenidos { get; set; }
        public IDbSet<Quiz> Quizzes { get; set; }
        public IDbSet<BateriaPreguntas> BateriasPreguntas { get; set; }
        public IDbSet<Pregunta> Preguntas { get; set; }
        public IDbSet<PreguntaAbierta> PreguntasAbiertas { get; set; }
        public IDbSet<PreguntaCorrespondencia> PreguntasCorrespondencia { get; set; }
        public IDbSet<PreguntaSeleccionMultiple> PreguntasSeleccionMultiple { get; set; }
        public IDbSet<PreguntaParametrizada> PreguntasParametrizadas { get; set; }
        public IDbSet<PreguntaVerdaderoFalso> PreguntasVerdaderoFalso { get; set; }
        public IDbSet<Competencia> Competencias { get; set; }
        public IDbSet<MaterialMultimedia> MaterialesMultimedia { get; set; }
        public IDbSet<NotaBateria> NotasBateria { get; set; }
        public IDbSet<ComboCorrespondencia> CombosCorrespondencia { get; set; }
        public IDbSet<ComboSeleccionMultiple> CombosSeleccionMultiple { get; set; }
        public IDbSet<HuecoParametrizada> HuecosParametrizada { get; set; }
        public IDbSet<RealizaQuiz> QuizesRealizados { get; set; }
        public IDbSet<Realizacion> Realizaciones { get; set; }
        public IDbSet<RespuestaCorrecta> RespuestasCorrectas { get; set; }
        public IDbSet<RespuestaPregunta> RespuestasPreguntas { get; set; }
    }
}
