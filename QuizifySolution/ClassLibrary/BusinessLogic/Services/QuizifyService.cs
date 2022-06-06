using Quizify.Entities;
using Quizify.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Quizify.Services
{
    public class QuizifyService : IQuizifyService
    {
        private readonly IDAL dal;
        private QuizifyPSWDbContext dbContext;

        public QuizifyService()
        {
            dbContext = new QuizifyPSWDbContext();
            dal = new EntityFrameworkDAL(dbContext);
            Borrador borrador = Borrador.GetInstance();
            PublicadoInactivo pi = PublicadoInactivo.GetInstance();
            PublicadoActivo pa = PublicadoActivo.GetInstance();
            Calificado c = Calificado.GetInstance();
            Borrador b = Borrador.GetInstance();
        }
        public async Task<bool> StartDBTask()
        {
            try
            {
                Quiz quiz = dal.GetById<Quiz>(0);
            }
            catch (NullReferenceException e)
            {

            }
            return true;
        }
        public List<List<String>> GetPreviousQuizes(string studentEmail, int topicId)
        {
            Estudiante estudiante = GetEstudianteByEmail(studentEmail);
            List<RealizaQuiz> quizzesRealizadosOriginal = dal.GetAll<RealizaQuiz>().ToList();
            List<RealizaQuiz> quizzesRealizados = quizzesRealizadosOriginal.Where(x => x.Estudiante.Email == studentEmail && x.Quiz.Contenido.Id == topicId).ToList();
            List<List<string>> lista = new List<List<string>>();
            foreach (RealizaQuiz realizaQuiz in quizzesRealizados)
            {
                List<string> quizInfo = new List<string>();
                quizInfo.Add(realizaQuiz.Id.ToString());
                quizInfo.Add(realizaQuiz.Quiz.Nombre);
                quizInfo.Add(GetQuizStatus(realizaQuiz.Quiz.Id) == 4 ? "1" : "0");

                if (GetQuizStatus(realizaQuiz.Quiz.Id) == 4 || realizaQuiz.Quiz.VerResultado)
                {
                    lista.Add(quizInfo);
                }
            }
            return lista;
        }

        public int GetQuizIdFromRealizaQuizId(int realizaQuizId)
        {
            RealizaQuiz realizaQuiz = dal.GetById<RealizaQuiz>(realizaQuizId);
            return realizaQuiz.Quiz.Id;
        }

        public void SignUp(int userType, string name, string email, string password)
        {
            if (userType == 0)
            {
                SignUpStudent(name, email, password);
            }
            else if (userType == 1)
            {
                SignUpInstructor(name, email, password);
            }
        }
        private void SignUpStudent(string name, string email, string password)
        {
            Persona persona = new Estudiante(name, email, password);
            try
            {
                dal.Insert(persona);
                Commit();
            }
            catch (Exception e)
            {
                throw new ServiceException("Excepcion!");
            }
        }
        private void SignUpInstructor(string name, string email, string password)
        {
            Persona persona = new Instructor(name, email, password);
            try
            {
                dal.Insert(persona);
                Commit();
            }
            catch (Exception e)
            {
                throw new ServiceException("Excepcion!");
            }
        }

        public void RemoveBatteryFromQuiz(int quizId, int batteryId, int position)
        {
            Quiz quiz = GetQuizById(quizId);
            BateriaPreguntas bateria = GetBatteryById(batteryId);
            NotaBateria notaBateriaToDelete = quiz.NotasBateria.ElementAt(position);
            quiz.NotasBateria.Remove(notaBateriaToDelete);
            bateria.NotasBaterias.Remove(notaBateriaToDelete);
            UpdateEntity(quiz);
            UpdateEntity(bateria);
        }

        public Quiz GetQuizById(int quizId)
        {
            return dal.GetById<Quiz>(quizId);
        }

        public double GetNotaQuizById(int quizId)
        {
            return dal.GetById<Quiz>(quizId).NotaQuiz;
        }


        public Contenido GetContenidoById(int contenidoId)
        {
            return dal.GetById<Contenido>(contenidoId);
        }

        public RespuestaPreguntaAbierta GetRespuestaPreguntaAbiertaById(int rpa)
        {
            return dal.GetById<RespuestaPreguntaAbierta>(rpa);
        }

        public string GetStatmentRespuestaPreguntaAbiertaById(int rpa)
        {
            return dal.GetById<RespuestaPreguntaAbierta>(rpa).PreguntaAsociada.GetStatement();
        }

        public string GetRespuestOfRespuestaPreguntaAbiertaById(int rpa)
        {
            RespuestaPregunta rp = dal.GetById<RespuestaPregunta>(rpa);
            dbContext.Entry<RespuestaPregunta>(rp).Reload();
            return ((RespuestaPreguntaAbierta)rp).Respuesta;
        }

        public int GetIdPreguntaAsociadaRespuestaPreguntaAbiertaById(int rpa)
        {
            return dal.GetById<RespuestaPreguntaAbierta>(rpa).PreguntaAsociada.Id;
        }


        public string GetNameBatteryById(int batteryId)
        {
            return dal.GetById<BateriaPreguntas>(batteryId).Nombre;
        }

        public string getCursoByQuizId(int quizId)
        {
            return dal.GetById<Quiz>(quizId).Contenido.CursoPerteneciente.Nombre;
        }

        public int getCountQuestionOfBattery(int batteryId)
        {
            return GetBatteryById(batteryId).Preguntas.Count;
        }

        public string getContenidoByQuizId(int quizId)
        {
            return dal.GetById<Quiz>(quizId).Contenido.Nombre;
        }

        public string GetNameQuizById(int quizId)
        {
            return dal.GetById<Quiz>(quizId).Nombre;
        }

        public void RemoveAllData()
        {
            dal.RemoveAllData();
        }

        public void Commit()
        {
            dal.Commit();
        }

        public bool InscribeStudentToCourse(string studentEmail, int courseId)
        {
            Estudiante estudiante = dal.GetById<Estudiante>(studentEmail);
            Curso curso = dal.GetById<Curso>(courseId);
            if (curso == null)
            {
                return false;
            }
            else
            {
                if (estudiante.CursosInscritos.Contains(curso))
                {
                    return false;
                }
                estudiante.CursosInscritos.Add(curso);
                UpdateEntity(estudiante);
                return true;
            }
        }

        public double getNotaRestoQuizes(int contenidoId)
        {
            double sumaTotal = 0;
            foreach (Quiz quiz in GetContenidoById(contenidoId).GetQuizzes())
            {
                sumaTotal += quiz.NotaQuiz;
            }
            return sumaTotal;
        }

        public List<List<String>> GetQuestionsFromInstructor(string instructorEmail, int questionType)
        {
            List<List<String>> list = new List<List<String>>();
            List<Pregunta> questions = dal.GetAll<Pregunta>().ToList();
            List<Contenido> contenidos = new List<Contenido>();
            List<Curso> cursos = new List<Curso>();
            List<Pregunta> validQuestions = new List<Pregunta>();

            for (int i = 0; i < questions.Count; i++)
            {
                bool questionBelongsToInstructor = false;
                Pregunta currentQuestion = questions.ElementAt(i);
                Curso cursoPerteneciente = null;
                Contenido contenidoPerteneciente = null;
                for (int j = 0; j < currentQuestion.BateriaPreguntasPertenecientes.Count && !questionBelongsToInstructor; j++)
                {
                    BateriaPreguntas currentBateria = currentQuestion.BateriaPreguntasPertenecientes.ElementAt(j);
                    for (int k = 0; k < currentBateria.NotasBaterias.Count && !questionBelongsToInstructor; k++)
                    {
                        Quiz currentQuiz = currentBateria.NotasBaterias.ElementAt(k).Quiz;
                        if (currentQuiz.Contenido.CursoPerteneciente.InstructorPerteneciente.Email == instructorEmail)
                        {
                            if ((questionType == 0 && currentQuestion is PreguntaAbierta)
                                || (questionType == 1 && currentQuestion is PreguntaVerdaderoFalso)
                                || (questionType == 2 && currentQuestion is PreguntaSeleccionMultiple)
                                || (questionType == 3 && currentQuestion is PreguntaCorrespondencia))
                            {
                                questionBelongsToInstructor = true;
                                cursoPerteneciente = currentQuiz.Contenido.CursoPerteneciente;
                                contenidoPerteneciente = currentQuiz.Contenido;
                            }
                        }
                    }
                }
                if (questionBelongsToInstructor)
                {
                    validQuestions.Add(currentQuestion);
                    cursos.Add(cursoPerteneciente);
                    contenidos.Add(contenidoPerteneciente);
                }
            }

            for (int i = 0; i < validQuestions.Count; i++)
            {
                Pregunta pregunta = validQuestions.ElementAt(i);

                List<String> questionInfo = new List<string>();
                questionInfo.Add(pregunta.Id.ToString());
                questionInfo.Add(pregunta.Enunciado);
                questionInfo.Add(pregunta.Autor.Nombre);
                questionInfo.Add(cursos.ElementAt(i).Nombre);
                questionInfo.Add(contenidos.ElementAt(i).Nombre);
                list.Add(questionInfo);
            }
            return list;
        }

        public List<List<String>> GetQuestionsFromBateria(int bateriaId)
        {
            BateriaPreguntas bateriaPreguntas = dal.GetById<BateriaPreguntas>(bateriaId);
            List<List<String>> list = new List<List<String>>();
            foreach (Pregunta pregunta in bateriaPreguntas.Preguntas)
            {
                List<String> questionInfo = new List<string>();
                questionInfo.Add(pregunta.Id.ToString());
                questionInfo.Add(pregunta.Enunciado);
                questionInfo.Add(pregunta.Autor.Nombre);
                list.Add(questionInfo);
            }
            return list;
        }

        public int GetQuestionTypeFromBateria(int bateriaId)
        {
            BateriaPreguntas bateria = dal.GetById<BateriaPreguntas>(bateriaId);

            if (bateria is BateriaAbierta)
            {
                return 0;
            }
            else if (bateria is BateriaVerdaderoFalso)
            {
                return 1;
            }
            else if (bateria is BateriaSeleccionMultiple)
            {
                return 2;
            }
            else if (bateria is BateriaCorrespondencia)
            {
                return 3;
            }
            return -1;
        }
        public string GetStudentFromRealizaQuiz(int realizaQuizId)
        {
            RealizaQuiz realizaQuiz = dal.GetById<RealizaQuiz>(realizaQuizId);
            return realizaQuiz.Estudiante.Email;
        }
        public string GetCourseDescription(int courseId)
        {
            Curso curso = dal.GetById<Curso>(courseId);
            return curso.Descripcion;
        }

        public bool IsQuestionInBateria(int questionId, int bateriaId)
        {
            Pregunta pregunta = dal.GetById<Pregunta>(questionId);
            BateriaPreguntas bateriaPreguntas = dal.GetById<BateriaPreguntas>(bateriaId);

            return bateriaPreguntas.Preguntas.Contains(pregunta);
        }

        public int GetCourseIcon(int courseId)
        {
            Curso curso = dal.GetById<Curso>(courseId);
            return curso.Icono;
        }

        public void UpdateCourseInfo(int courseId, string name, string description, int icon)
        {
            Curso curso = dal.GetById<Curso>(courseId);
            curso.Nombre = name;
            curso.Descripcion = description;
            curso.Icono = icon;
            UpdateEntity(curso);
        }

        public Persona GetPersonByEmail(string email)
        {
            Persona persona = dal.GetById<Persona>(email);
            return persona;
        }

        public Pregunta GetPreguntaById(int id)
        {
            Pregunta pregunta = dal.GetById<Pregunta>(id);
            return pregunta;
        }
        public string GetEnunciadoPreguntaById(int id)
        {
            Pregunta pregunta = dal.GetById<Pregunta>(id);
            return pregunta.Enunciado;
        }
        public Contenido GetTopicById(int topicId)
        {
            Contenido contenido = dal.GetById<Contenido>(topicId);
            return contenido;
        }

        public Instructor GetInstructorByEmail(string email)
        {
            Instructor instructor = dal.GetById<Instructor>(email);
            return instructor;
        }

        public Estudiante GetEstudianteByEmail(string email)
        {
            return dal.GetById<Estudiante>(email);
        }
        public string GetNombreEstudianteByEmail(string email)
        {
            return dal.GetById<Estudiante>(email).Nombre;
        }

        public RealizaQuiz GetRealizaQuizById(int RealizaQuizId)
        {
            return dal.GetById<RealizaQuiz>(RealizaQuizId);
        }
        public int GetLastRealizaQuizById(int RealizaQuizId)
        {
            return dal.GetById<RealizaQuiz>(RealizaQuizId).GetLastRealizacion().Id;
        }
        public bool GetTerminadoLastRealizaQuizById(int RealizaQuizId)
        {
            return dal.GetById<RealizaQuiz>(RealizaQuizId).GetLastRealizacion().EsTerminado;
        }

        public Realizacion GetRealizacionById(int RealizacionId)
        {
            return dal.GetById<Realizacion>(RealizacionId);
        }
        public int GetCountRespuestasRealizacionById(int RealizacionId)
        {
            return dal.GetById<Realizacion>(RealizacionId).RespuestasPreguntas.Count;
        }

        public int GetIdRespuestaRealizacionById(int RealizacionId, int i)
        {
            return dal.GetById<Realizacion>(RealizacionId).RespuestasPreguntas.ElementAt(i).Id;
        }
        public int GetIdPreguntaAsociadaRespuestaRealizacionById(int RealizacionId, int i)
        {
            return dal.GetById<Realizacion>(RealizacionId).RespuestasPreguntas.ElementAt(i).PreguntaAsociada.Id;
        }

        public Curso getCursoById(int id)
        {
            Curso curso = dal.GetById<Curso>(id);
            return curso;
        }

        public Quiz getQuizById(int id)
        {
            Quiz quiz = dal.GetById<Quiz>(id);
            return quiz;
        }

        public bool GetPerson(string email, string password)
        {
            Persona persona = GetPersonByEmail(email);
            if (persona == null)
            {
                throw new ServiceException("There is no person associated with this email");
            }
            else if (persona.Password != password)
            {
                throw new ServiceException("The email and password do not match");
            }
            else
            {
                return true;
            }
        }

        public void FillExampleData()
        {
            RemoveAllData();

            Instructor instructor1 = new Instructor("Pepe", "pepe@pepe.com", "pepe");
            dal.Insert<Instructor>(instructor1);

            Curso curso1 = new Curso("DDS", 1, "Descripcion del curso", instructor1);
            dal.Insert<Curso>(curso1);

            Contenido contendido1 = new Contenido("Patrones Arquitectónicos", curso1);
            dal.Insert<Contenido>(contendido1);

            Estudiante estudiante1 = new Estudiante("Manolo", "manolo@manolo.com", "manolo");
            dal.Insert<Estudiante>(estudiante1);

            Estudiante estudiante2 = new Estudiante("Miguel", "miguel@miguel.com", "miguel");
            dal.Insert<Estudiante>(estudiante2);

            curso1.Estudiantes.Add(estudiante1);
            curso1.Estudiantes.Add(estudiante2);

            Commit();

        }

        public void setQuizes(string instructorId, int nQuiz)
        {
            GetInstructorByEmail(instructorId).numQuizesRestantes += nQuiz;
            Commit();
        }

        public void deleteQuizes(Instructor i)
        {
            i.numQuizesRestantes--;
            Commit();
        }

        public int getNumQuizesRestantes(string instructorId)
        {
            Instructor i = dal.GetById<Instructor>(instructorId);
            return i.numQuizesRestantes;
        }


        public int CreateQuiz(string nombre, int modoRespuesta, int tiempo, double peso, DateTime fechaCreacion, int numeroIntentos, int contenidoId, bool verResultado, double notaquiz)
        {
            Contenido contenido = dal.GetById<Contenido>(contenidoId);
            Quiz quiz = new Quiz(nombre, modoRespuesta, tiempo, peso, fechaCreacion, contenido.CursoPerteneciente.InstructorPerteneciente, numeroIntentos, contenido, verResultado, notaquiz);
            dal.Insert<Quiz>(quiz);
            Commit();

            deleteQuizes(contenido.CursoPerteneciente.InstructorPerteneciente);

            return quiz.Id;
        }

        public void SetQuizWeight(int quizId, double mark)
        {
            Quiz quiz = getQuizById(quizId);
            quiz.NotaQuiz = mark;
            Commit();
        }


        private List<Curso> GetCursosByInstructor(Instructor i)
        {
            List<Curso> cursos = new List<Curso>();
            foreach (Curso c in i.Cursos)
            {
                cursos.Add(c);
            }
            return cursos;
        }

        private List<Contenido> GetContenidoByCurso(Curso curso)
        {
            List<Contenido> contenido = new List<Contenido>();
            foreach (Contenido c in curso.Contenidos)
            {
                contenido.Add(c);
            }
            return contenido;
        }

        private List<Quiz> GetQuizesByContenido(Contenido contenido)
        {
            List<Quiz> quizes = new List<Quiz>();
            foreach (Quiz q in contenido.Quizzes)
            {
                quizes.Add(q);
            }
            return quizes;
        }

        public void SetNotaBateriaWeight(int nbId, int dificultad, double peso)
        {
            NotaBateria nb = GetNotaBateriaById(nbId);
            nb.Dificultad = dificultad;
            nb.NotaBateriaPreguntas = peso;
            Commit();
        }
        public int CreateBateria(int type, int quizId, string name)
        {
            if (type == 0)
            {
                return CreateBateriaAbierta(quizId, name);
            }
            else if (type == 1)
            {
                return CreateBateriaVerdaderoFalso(quizId, name);
            }
            else if (type == 2)
            {
                return CreateBateriaSeleccion(quizId, name);
            }
            else if (type == 3)
            {
                return CreateBateriaCorrespondencia(quizId, name);
            }
            else
            {
                throw new ServiceException("Invalid type");
            }
        }

        private int CreateBateriaVerdaderoFalso(int quizId, string name)
        {
            Quiz quizPerteneciente = dal.GetById<Quiz>(quizId);
            BateriaVerdaderoFalso bateria = new BateriaVerdaderoFalso(name, quizPerteneciente);
            dal.Insert<BateriaPreguntas>(bateria);
            Commit();
            return bateria.Id;
        }

        private int CreateBateriaAbierta(int quizId, string name)
        {
            Quiz quizPerteneciente = dal.GetById<Quiz>(quizId);
            BateriaAbierta bateria = new BateriaAbierta(name, quizPerteneciente);
            dal.Insert<BateriaPreguntas>(bateria);
            Commit();
            return bateria.Id;
        }
        private int CreateBateriaSeleccion(int quizId, string name)
        {
            Quiz quizPerteneciente = dal.GetById<Quiz>(quizId);
            BateriaSeleccionMultiple bateria = new BateriaSeleccionMultiple(name, quizPerteneciente);
            dal.Insert<BateriaPreguntas>(bateria);
            Commit();
            return bateria.Id;
        }
        private int CreateBateriaCorrespondencia(int quizId, string name)
        {
            Quiz quizPerteneciente = dal.GetById<Quiz>(quizId);
            BateriaCorrespondencia bateria = new BateriaCorrespondencia(name, quizPerteneciente);
            dal.Insert<BateriaPreguntas>(bateria);
            Commit();
            return bateria.Id;
        }

        public int CreateRealizaQuiz(int quizId, string studentEmail)
        {
            Quiz quiz = GetQuizById(quizId);
            Estudiante estudiante = (Estudiante)GetPersonByEmail(studentEmail);
            RealizaQuiz realizaQuiz = new RealizaQuiz(quiz, estudiante);
            dal.Insert(realizaQuiz);
            Commit();
            //quiz.Realizaciones.Add(realizaQuiz);
            //estudiante.QuizzesRealizados.Add(realizaQuiz);
            return realizaQuiz.Id;
        }

        public RealizaQuiz GetRealizaQuizFromQuizAndStudent(Quiz quiz, Estudiante student)
        {
            List<RealizaQuiz> realizaQuizzes = dal.GetAll<RealizaQuiz>().ToList();
            foreach (RealizaQuiz realizaQuiz in realizaQuizzes)
            {
                if (realizaQuiz.Quiz.Equals(quiz) && realizaQuiz.Estudiante.Equals(student))
                {
                    return realizaQuiz;
                }
            }
            return null;
        }

        public int GetRealizaQuizFromQuizAndStudent(int quiz, string student)
        {
            List<RealizaQuiz> realizaQuizzes = dal.GetAll<RealizaQuiz>().ToList();
            foreach (RealizaQuiz realizaQuiz in realizaQuizzes)
            {
                if (realizaQuiz.Quiz.Id.Equals(quiz) && realizaQuiz.Estudiante.Email.Equals(student))
                {
                    return realizaQuiz.Id;
                }
            }
            return -1;
        }
        public bool GetRealizaQuizFromQuizAndStudentIsNull(int quizId, string studentEmail)
        {
            return GetRealizaQuizFromQuizAndStudent(GetQuizById(quizId), GetEstudianteByEmail(studentEmail)) == null;
        }

        public int GetIdRealizaQuizFromQuizAndStudent(int quiz, string student)
        {
            List<RealizaQuiz> realizaQuizzes = dal.GetAll<RealizaQuiz>().ToList();
            foreach (RealizaQuiz realizaQuiz in realizaQuizzes)
            {
                if (realizaQuiz.Quiz.Id.Equals(quiz) && realizaQuiz.Estudiante.Email.Equals(student))
                {
                    return realizaQuiz.Id;
                }
            }
            return 0;
        }

        private List<BateriaPreguntas> GetAllBatteries()
        {
            List<BateriaPreguntas> list = dal.GetAll<BateriaPreguntas>().ToList();
            return list;
        }

        public int CreateRealizacionInOrder(int realizaQuizId)
        {
            RealizaQuiz realizaQuiz = dal.GetById<RealizaQuiz>(realizaQuizId);
            Realizacion realizacion = new Realizacion(realizaQuiz.Quiz.Tiempo * 60, realizaQuiz);
            dal.Insert(realizacion);
            foreach (BateriaPreguntas bateria in realizaQuiz.Quiz.GetBateriasPreguntas())
            {
                Pregunta pregunta = bateria.GetRandomQuestion();
                RespuestaPregunta rp = null;

                if (bateria is BateriaAbierta)
                {
                    rp = new RespuestaPreguntaAbierta(pregunta, realizacion);
                }
                else if (bateria is BateriaVerdaderoFalso)
                {
                    rp = new RespuestaPreguntaVF(pregunta, realizacion);
                }
                else if (bateria is BateriaSeleccionMultiple)
                {
                    rp = new RespuestaPreguntaMultiple(pregunta, realizacion);
                }
                else if (bateria is BateriaCorrespondencia)
                {
                    rp = new RespuestaPreguntaCorrespondencia(pregunta, realizacion);
                }
                dal.Insert(rp);
            }
            Commit();
            return realizacion.Id;
        }

        public int CreateRealizacionRandomOrder(int realizaQuizId)
        {
            RealizaQuiz realizaQuiz = dal.GetById<RealizaQuiz>(realizaQuizId);
            Realizacion realizacion = new Realizacion(realizaQuiz.Quiz.Tiempo * 60, realizaQuiz);
            dal.Insert(realizacion);
            List<BateriaPreguntas> baterias = realizaQuiz.Quiz.GetBateriasPreguntas();
            Shuffle(baterias);
            foreach (BateriaPreguntas bateria in baterias)
            {
                Pregunta pregunta = bateria.GetRandomQuestion();
                RespuestaPregunta rp = null;

                if (bateria is BateriaAbierta)
                {
                    rp = new RespuestaPreguntaAbierta(pregunta, realizacion);
                }
                else if (bateria is BateriaVerdaderoFalso)
                {
                    rp = new RespuestaPreguntaVF(pregunta, realizacion);
                }
                else if (bateria is BateriaSeleccionMultiple)
                {
                    rp = new RespuestaPreguntaMultiple(pregunta, realizacion);
                }
                else if (bateria is BateriaCorrespondencia)
                {
                    rp = new RespuestaPreguntaCorrespondencia(pregunta, realizacion);
                }
                dal.Insert(rp);
            }
            Commit();

            return realizacion.Id;
        }


        private void Shuffle(IList<BateriaPreguntas> list)
        {
            Random rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                BateriaPreguntas value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void UpdateEntity<T>(T modifiedEntity) where T : class
        {
            dal.Update<T>(modifiedEntity);
        }

        public List<List<String>> GetAllQuizzes(String personEmail)
        {
            Persona p = dal.GetById<Persona>(personEmail);
            List<List<String>> list = new List<List<String>>();
            if (p is Instructor)
            {
                Instructor instructor = (Instructor)p;
                foreach (Curso curso in instructor.Cursos)
                {
                    foreach (Contenido topic in curso.Contenidos)
                    {
                        GetQuizzesFromTopic(topic.Id, list);

                    }
                }
            }
            else
            {
                Estudiante estudiante = (Estudiante)p;
                foreach (Curso curso in estudiante.CursosInscritos)
                {
                    foreach (Contenido topic in curso.Contenidos)
                    {
                        GetQuizzesFromTopic(topic.Id, list);

                    }
                };
            }
            return list;
        }

        public void GetQuizzesFromTopic(int topicId, List<List<String>> list)
        {
            Contenido contenido = GetTopicById(topicId);
            dbContext.Entry<Contenido>(contenido).Reload();

            foreach (Quiz quiz in contenido.Quizzes)
            {

                List<String> quizInfo = new List<String>();
                quizInfo.Add(quiz.Id.ToString());
                quizInfo.Add(quiz.Nombre);
                quizInfo.Add(quiz.FechaCreacion.Value.ToShortDateString());
                quizInfo.Add("");
                if (quiz.FechaInicio.HasValue)
                {
                    quizInfo.Add(quiz.FechaInicio.Value.ToShortDateString());
                }
                else
                {
                    quizInfo.Add("");
                }
                if (quiz.FechaCierre.HasValue)
                {
                    quizInfo.Add(quiz.FechaCierre.Value.ToShortDateString());
                }
                else
                {
                    quizInfo.Add("");
                }
                if (quiz.FechaInicio.HasValue && quiz.FechaCierre.HasValue)
                {
                    if (quiz.NumeroIntentos == -1)
                    {
                        quizInfo.Add("Ilimitados");
                    }
                    else
                    {
                        quizInfo.Add(quiz.NumeroIntentos.ToString());
                    }
                }
                else
                {
                    quizInfo.Add("");

                }
                quizInfo.Add(quiz.Contenido.CursoPerteneciente.Id.ToString());
                quizInfo.Add(GetQuizStatus(quiz.Id).ToString());
                list.Add(quizInfo);
            }
        }

        public void GetStudentsAttributes(ICollection<String> students, List<List<String>> l)
        {
            foreach (String est in students)
            {
                List<String> attributes = new List<String>();
                attributes.Add(this.GetEstudianteByEmail(est).Nombre);
                attributes.Add(this.GetEstudianteByEmail(est).Email);
                l.Add(attributes);
            }
        }

        public List<List<String>> GetTopicsFromCourse(int courseId)
        {
            List<List<String>> list = new List<List<String>>();
            Curso curso = GetCursoById(courseId);

            foreach (Contenido contenido in curso.Contenidos)
            {
                List<String> topicInfo = new List<String>();
                topicInfo.Add(contenido.Id.ToString());
                topicInfo.Add(contenido.Nombre);
                topicInfo.Add(contenido.CursoPerteneciente.Id.ToString());
                list.Add(topicInfo);
            }
            return list;
        }

        public int CreateNewCourse(String instructorEmail, String name, int icon, String description)
        {
            Instructor instructor = GetInstructorByEmail(instructorEmail);
            Curso curso = new Curso(name, icon, description, instructor);
            dal.Insert<Curso>(curso);
            Commit();
            return curso.Id;
        }


        public int CopyCoursePrototype(String nombreCurso, int cursoId, string EmailCreator)
        {
            Curso curso = this.GetCourseById(cursoId);
            Curso createdCourse = curso.CopyCourse(nombreCurso, this.GetInstructorByEmail(EmailCreator));
            dal.Insert<Curso>(createdCourse);
            Commit();
            return createdCourse.Id;
        }



        public int CopyQuizNormal(String quizName, int quizId, int topicId)
        {
            Quiz quiz = this.GetQuizById(quizId);
            Quiz createdQuiz = quiz.copyQuiz(quizName, this.GetTopicById(topicId));
            foreach (NotaBateria nb in quiz.NotasBateria)
            {
                NotaBateria nuevaNota = nb.copyNotaBateria(createdQuiz);
                BateriaPreguntas nuevaBateria = nb.Bateria.CopyBatery();
                foreach (Pregunta pregunta in nb.Bateria.Preguntas)
                {
                    nuevaBateria.AñadirPregunta(pregunta.CopyQuestion());
                }
                nuevaNota.Bateria = nuevaBateria;
                createdQuiz.NotasBateria.Add(nuevaNota);
            }
            dal.Insert<Quiz>(createdQuiz);
            Commit();
            return createdQuiz.Id;
        }

        public BateriaPreguntas CopyBatery(BateriaPreguntas bateria)
        {
            BateriaPreguntas copia;
            if (bateria is BateriaAbierta) copia = new BateriaAbierta();
            else if (bateria is BateriaCorrespondencia) copia = new BateriaCorrespondencia();
            else if (bateria is BateriaVerdaderoFalso) copia = new BateriaVerdaderoFalso();
            else if (bateria is BateriaSeleccionMultiple) copia = new BateriaSeleccionMultiple();
            else if (bateria is BateriaParametrizada) copia = new BateriaParametrizada();
            else return null;
            foreach (Pregunta pregunta in bateria.Preguntas)
            {
                copia.AñadirPregunta(pregunta);
            }
            return copia;
        }

        public Curso GetCursoById(int id)
        {
            return dal.GetById<Curso>(id);
        }

        public RespuestaPregunta GetRespuestaPreguntaById(int ResPregId)
        {
            return dal.GetById<RespuestaPregunta>(ResPregId);
        }

        public Pregunta GetQuestionById(int id)
        {
            return dal.GetById<Pregunta>(id);
        }


        public BateriaPreguntas GetBatteryById(int id)
        {
            return dal.GetById<BateriaPreguntas>(id);
        }

        public void SetBatteryName(int id, string name)
        {
            GetBatteryById(id).Nombre = name;
            Commit();
        }

        public void AddBatteryToQuiz(int batteryId, int quizId)
        {
            GetBatteryById(batteryId).NotasBaterias.Add(new NotaBateria(GetQuizById(quizId), 0, 0));
            Commit();
        }


        public void AddTopicToCourse(int courseId, string newTopicName)
        {
            Curso cursoPerteneciente = GetCursoById(courseId);
            Contenido newTopic = new Contenido(newTopicName, cursoPerteneciente);
            dal.Insert(newTopic);
            Commit();
        }
        public string GetCourseName(int courseId)
        {
            Curso curso = GetCursoById(courseId);
            return curso.Nombre;
        }
        public string GetTopicName(int topicId)
        {
            Contenido contenido = GetTopicById(topicId);
            return contenido.Nombre;
        }

        public bool IsPersonAStudent(string personEmail)
        {
            Persona p = GetPersonByEmail(personEmail);
            return p is Estudiante;
        }
        public string GetPersonName(string personEmail)
        {
            Persona p = GetPersonByEmail(personEmail);
            return p.Nombre;
        }
        public int GetCourseOfTopic(int topicId)
        {
            Contenido contenido = GetTopicById(topicId);
            return contenido.CursoPerteneciente.Id;
        }
        public string GetQuizName(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.Nombre;
        }

        public int GetTopicFromQuiz(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.Contenido.Id;
        }
        public Curso GetCourseById(int courseId)
        {
            return dal.GetById<Curso>(courseId);
        }
        public List<List<String>> GetCoursesFromPerson(string personEmail)
        {
            List<List<String>> list = new List<List<String>>();
            Persona p = GetPersonByEmail(personEmail);
            List<Curso> cursos;
            if (p is Instructor instructor)
            {
                cursos = (List<Curso>)instructor.Cursos;
            }
            else
            {
                cursos = (List<Curso>)((Estudiante)p).CursosInscritos;
            }
            foreach (Curso curso in cursos)
            {
                List<String> courseInfo = new List<String>();
                courseInfo.Add(curso.Id.ToString());
                courseInfo.Add(curso.Nombre);
                courseInfo.Add(curso.Descripcion);
                list.Add(courseInfo);
            }
            return list;
        }

        public bool StudentHasUnfinishedAnsweredTest(string personEmail, int quizId)
        {
            Estudiante estudiante = (Estudiante)GetPersonByEmail(personEmail);
            Quiz quiz = GetQuizById(quizId);
            RealizaQuiz realizaQuiz = GetRealizaQuizFromQuizAndStudent(quiz, estudiante);
            if (realizaQuiz != null)
            {
                Realizacion realizacion = realizaQuiz.GetUnfinishedRealizacion();
                if (realizacion != null)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetQuizNumberOfAttempts(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.GetNumberOfAttempts();
        }

        public int GetStudentRealizationsCount(string personEmail, int quizId)
        {
            Estudiante estudiante = (Estudiante)GetPersonByEmail(personEmail);
            Quiz quiz = GetQuizById(quizId);
            RealizaQuiz realizaQuiz = GetRealizaQuizFromQuizAndStudent(quiz, estudiante);
            if (realizaQuiz != null)
            {
                return realizaQuiz.Realizaciones.Count;

            }
            return 0;
        }

        public void InsertQuestionToBattery(int bateriaId, int preguntaId)
        {
            BateriaPreguntas bateriaPreguntas = dal.GetById<BateriaPreguntas>(bateriaId);
            Pregunta pregunta = dal.GetById<Pregunta>(preguntaId);
            bateriaPreguntas.AñadirPregunta(pregunta);
            Commit();
        }

        public void RemoveQuestionFromBattery(int bateriaId, int preguntaId)
        {
            BateriaPreguntas bateriaPreguntas = dal.GetById<BateriaPreguntas>(bateriaId);
            Pregunta pregunta = dal.GetById<Pregunta>(preguntaId);
            bateriaPreguntas.Preguntas.Remove(pregunta);
            Commit();
        }

        public bool GetQuizViewResult(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.VerResultado;
        }

        public List<String> GetTopicsFromCourse(Curso curso)
        {
            List<String> c = new List<String>();
            foreach (Contenido contenido in GetContenidoByCurso(curso))
            {
                c.Add(contenido.Nombre);
            }
            return c;
        }

        public List<String> GetCoursesFromTeacher(string email)
        {
            List<String> c = new List<String>();
            foreach (Curso cursos in GetCursosByInstructor(GetInstructorByEmail(email)))
            {
                c.Add(cursos.Nombre);
            }
            return c;
        }

        private Curso GetCourseByInstructorAndName(string email, string nombreCurso)
        {
            foreach (Curso curso in GetCursosByInstructor(GetInstructorByEmail(email)))
            {
                if (nombreCurso != null && curso.Nombre == nombreCurso)
                {
                    return curso;
                }
            }
            return null;
        }

        private Contenido GetTopicFromTeacherCourseNameAndTopicName(string email, string nombreCurso, string nombreContenido)
        {
            foreach (Contenido conte in GetContenidoByCurso(GetCourseByInstructorAndName(email, nombreCurso)))
            {
                if (nombreContenido != null && conte.Nombre == nombreContenido)
                {
                    return conte;
                }
            }
            return null;
        }

        public List<String> GetQuizzesFromInstructorCourseAndTopic(string email, string nombreCurso, string nombreContenido)
        {
            List<String> c = new List<String>();
            foreach (Quiz quiz in GetQuizesByContenido(GetTopicFromTeacherCourseNameAndTopicName(email, nombreCurso, nombreContenido)))
            {
                c.Add(quiz.Nombre);
            }
            return c;
        }

        public int GetQuizFromInstructorCourseNameTopicNameAndQuizName(string email, string nombreCurso, string nombreContenido, string nombreQuiz)
        {
            foreach (Quiz quiz in GetQuizesByContenido(GetTopicFromTeacherCourseNameAndTopicName(email, nombreCurso, nombreContenido)))
            {
                if (quiz.Nombre.Equals(nombreQuiz))
                {
                    return quiz.Id;
                }
            }
            return 0;
        }
        private string GetBatteryTypeString(BateriaPreguntas bp)
        {
            if (bp is BateriaVerdaderoFalso)
            {
                return "Verdadero/Falso";
            }
            else if (bp is BateriaAbierta)
            {
                return "Abierta";
            }
            else if (bp is BateriaSeleccionMultiple)
            {
                return "Selección Múltiple";
            }
            else if (bp is BateriaCorrespondencia)
            {
                return "Correspondencia";
            }
            else
            {
                return null;
            }
        }

        public List<List<String>> GetBatteryStringList()
        {
            List<List<String>> list = new List<List<String>>();

            foreach (BateriaPreguntas bp in GetAllBatteries())
            {
                foreach (NotaBateria nb in bp.NotasBaterias)
                {

                    List<String> questionInfo = new List<string>();
                    questionInfo.Add(bp.Id.ToString());
                    questionInfo.Add(nb.Quiz.Id.ToString());
                    questionInfo.Add(bp.Nombre);
                    questionInfo.Add(GetBatteryTypeString(bp));
                    questionInfo.Add(nb.Quiz.Nombre);
                    questionInfo.Add(nb.Quiz.Contenido.CursoPerteneciente.Nombre);
                    list.Add(questionInfo);
                }
            }

            return list;

        }
        public double GetNotaLastRealizacion(int quizId, string studentEmail)
        {
            RealizaQuiz rq = this.GetRealizaQuizFromQuizAndStudent(this.GetQuizById(quizId), (Estudiante)this.GetPersonByEmail(studentEmail));
            return rq.GetLastRealizacion().Puntuacion;
        }
        public int GetQuizSize(int quizId)
        {
            Quiz quiz = this.GetQuizById(quizId);
            return quiz.NotasBateria.Count();
        }
        public Pregunta GetPreguntaFromLastRealizacion(int quizId, string studentEmail, int posicion)
        {
            RealizaQuiz rq = this.GetRealizaQuizFromQuizAndStudent(this.GetQuizById(quizId), (Estudiante)this.GetPersonByEmail(studentEmail));
            return rq.GetLastRealizacion().RespuestasPreguntas.ElementAt<RespuestaPregunta>(posicion).PreguntaAsociada;
        }
        public int GetPreguntaIdFromLastRealizacion(int quizId, string studentEmail, int posicion)
        {
            Pregunta p = this.GetPreguntaFromLastRealizacion(quizId, studentEmail, posicion);
            return p.Id;
        }
        public RespuestaPregunta GetRespuestaPreguntaFromLastRealizacion(int quizId, string studentEmail, int posicion)
        {
            RealizaQuiz rq = this.GetRealizaQuizFromQuizAndStudent(this.GetQuizById(quizId), (Estudiante)this.GetPersonByEmail(studentEmail));
            return rq.GetLastRealizacion().RespuestasPreguntas.ElementAt<RespuestaPregunta>(posicion);
        }

        public int GetIdRespuestaPreguntaFromLastRealizacion(int quizId, string studentEmail, int posicion)
        {
            RealizaQuiz rq = this.GetRealizaQuizFromQuizAndStudent(this.GetQuizById(quizId), (Estudiante)this.GetPersonByEmail(studentEmail));
            return rq.GetLastRealizacion().RespuestasPreguntas.ElementAt<RespuestaPregunta>(posicion).Id;
        }


        public bool RespondidoCorrectamenteVF(int quizId, string studentEmail, int posicion)
        {
            RespuestaPreguntaVF rp = (RespuestaPreguntaVF)this.GetRespuestaPreguntaFromLastRealizacion(quizId, studentEmail, posicion);
            RespuestaCorrectaVF rc = (RespuestaCorrectaVF)this.GetPreguntaFromLastRealizacion(quizId, studentEmail, posicion).respuestaCorrecta;
            return rp.Respuesta == rc.Resultado;
        }

        public void LaunchQuiz(int quizId, DateTime openingDate, DateTime closingDate, bool viewResult, bool randomOrder, int numTries, int time, int answerMode)
        {
            Quiz quiz = GetQuizById(quizId);
            quiz.FechaInicio = openingDate;
            quiz.FechaCierre = closingDate;
            quiz.VerResultado = viewResult;
            quiz.PreguntasAleatorias = randomOrder;
            quiz.NumeroIntentos = numTries;
            quiz.Tiempo = time;
            quiz.ModoRespuesta = answerMode;
            quiz.Lanzar();
            UpdateAllQuizzesStatus();
            UpdateEntity(quiz);
        }

        public NotaBateria GetNotaBateriaById(int notabateriaId)
        {
            return dal.GetById<NotaBateria>(notabateriaId);
        }

        public double GetNotaNotaBateriaById(int notabateriaId)
        {
            return dal.GetById<NotaBateria>(notabateriaId).NotaBateriaPreguntas;
        }

        public int GetDificultadNotaBateriaById(int notabateriaId)
        {
            return dal.GetById<NotaBateria>(notabateriaId).Dificultad;
        }

        public NotaBateria GetNotaBateriaFromPreguntaAndQuiz(int quizId, int pId)
        {
            Quiz q = getQuizById(quizId);
            Pregunta p = GetQuestionById(pId);
            foreach (NotaBateria bp in q.NotasBateria)
            {
                if (bp.Bateria.Preguntas.Contains(p))
                {
                    return bp;
                }
            }
            return null;
        }
        public double GetPesoNotaBateriaFromPreguntaAndQuiz(int quizId, int pId)
        {
            Quiz q = getQuizById(quizId);
            Pregunta p = GetQuestionById(pId);
            foreach (NotaBateria bp in q.NotasBateria)
            {
                if (bp.Bateria.Preguntas.Contains(p))
                {
                    return bp.NotaBateriaPreguntas;
                }
            }
            return 0;
        }

        public int GetNotaBateriaIdFromBatteryAndQuiz(int quizId, int batteryId)
        {
            foreach (NotaBateria nb in getQuizById(quizId).NotasBateria)
            {
                if (nb.Bateria.Id.Equals(batteryId))
                {
                    return nb.Id;
                }
            }
            return -1;
        }
        public List<String> StudentsInACourse(int courseId)
        {
            List<String> studentsIDs = new List<String>();
            Curso c = this.GetCourseById(courseId);
            foreach (Estudiante est in c.Estudiantes)
            {
                studentsIDs.Add(est.Email);
            }
            return studentsIDs;
        }
        public void SetQuizName(int quizId, string quizName)
        {
            Quiz quiz = GetQuizById(quizId);
            quiz.SetName(quizName);
            UpdateEntity(quiz);
        }
        public List<List<String>> GetBatteriesFromQuiz(int quizId)
        {
            List<List<String>> list = new List<List<String>>();

            Quiz quiz = GetQuizById(quizId);

            foreach (NotaBateria nb in quiz.NotasBateria)
            {
                BateriaPreguntas bp = nb.Bateria;
                int type = -1;

                if (bp is BateriaAbierta)
                {
                    type = 0;
                }
                else if (bp is BateriaVerdaderoFalso)
                {
                    type = 1;
                }
                else if (bp is BateriaSeleccionMultiple)
                {
                    type = 2;
                }
                else if (bp is BateriaCorrespondencia)
                {
                    type = 3;
                }

                List<String> bateriaInfo = new List<string>();
                bateriaInfo.Add(bp.Id.ToString());
                bateriaInfo.Add(bp.Nombre);
                bateriaInfo.Add(bp.Preguntas.Count.ToString());
                bateriaInfo.Add(type.ToString());

                list.Add(bateriaInfo);
            }

            return list;
        }
        public string GetQuestionStatement(int questionId)
        {
            Pregunta pregunta = GetPreguntaById(questionId);
            return pregunta.Enunciado;
        }

        public void SetQuestionStatement(int questionId, string statement)
        {
            Pregunta pregunta = GetPreguntaById(questionId);
            pregunta.Enunciado = statement;
            UpdateEntity(pregunta);
        }
        private int GetRespuestaPregunta(int questionId, int realizacionId)
        {
            Pregunta pregunta = GetPreguntaById(questionId);
            Realizacion realizacion = dal.GetById<Realizacion>(realizacionId);
            foreach (RespuestaPregunta rp in realizacion.RespuestasPreguntas)
            {
                if (rp.PreguntaAsociada.Id == pregunta.Id)
                {
                    return rp.Id;
                }
            }
            return -1;
        }
        public void SetOpenStudentQuestionAnswer(int currentRealizacionId, int questionId, string answer)
        {
            RespuestaPreguntaAbierta rpa = dal.GetById<RespuestaPreguntaAbierta>(GetRespuestaPregunta(questionId, currentRealizacionId));
            rpa.Respuesta = answer;
            UpdateEntity(rpa);
        }

        public int GetQuestionIdFromBattery(int batteryId, int questionPosition)
        {
            BateriaPreguntas bp = dal.GetById<BateriaPreguntas>(batteryId);
            return bp.Preguntas.ElementAt<Pregunta>(questionPosition).Id;
        }

        public bool? GetVFStudentAnswer(int currentRealizacionId, int questionId)
        {
            Realizacion realizacion = dal.GetById<Realizacion>(currentRealizacionId);
            foreach (RespuestaPregunta respuesta in realizacion.RespuestasPreguntas)
            {
                if (respuesta.PreguntaAsociada.Id == questionId)
                {
                    return GetVFStudentAnswer(respuesta.Id);
                }
            }
            return null;
        }

        public bool? GetVFStudentAnswer(int respondePreguntaId)
        {
            RespuestaPregunta respuesta = GetRespuestaPreguntaById(respondePreguntaId);
            dbContext.Entry<RespuestaPregunta>(respuesta).Reload();
            bool? answer = respuesta.GetVFAnswer();
            if (answer != null)
            {
                return answer;
            }
            else
            {
                return null;
            }
        }

        public bool GetVFCorrectAnswer(int questionId)
        {
            PreguntaVerdaderoFalso pregunta = dal.GetById<PreguntaVerdaderoFalso>(questionId);
            return pregunta.GetAnswerVF().GetCorrectAnswer();
        }

        public void SetVFCorrectAnswer(int questionId, bool correctAnswer)
        {
            PreguntaVerdaderoFalso pregunta = dal.GetById<PreguntaVerdaderoFalso>(questionId);
            pregunta.GetAnswerVF().SetCorrectAnswer(correctAnswer);
            UpdateEntity<PreguntaVerdaderoFalso>(pregunta);
        }

        public void SetVFStudentQuestionAnswer(int currentRealizacionId, int questionId, bool answer)
        {
            RespuestaPreguntaVF rpa = dal.GetById<RespuestaPreguntaVF>(GetRespuestaPregunta(questionId, currentRealizacionId));
            rpa.Respuesta = answer;
            UpdateEntity(rpa);
        }

        public void CreateQuestionInBateria(int bateriaId, string statement, string instructorEmail)
        {
            BateriaPreguntas bateria = dal.GetById<BateriaPreguntas>(bateriaId);
            Instructor instructor = GetInstructorByEmail(instructorEmail);
            Pregunta newQuestion = bateria.CrearPregunta(statement, instructor);
            dal.Insert<Pregunta>(newQuestion);
            Commit();
        }

        public string GetInstructorEmailFromQuiz(int quizId)
        {
            Quiz quiz = dal.GetById<Quiz>(quizId);
            return quiz.Autor.Email;
        }

        public double GetValorQuiz(int quizId)
        {
            double suma = 0;
            foreach (NotaBateria nb in GetQuizById(quizId).NotasBateria)
            {
                suma += nb.NotaBateriaPreguntas;
            }
            return suma;
        }
        public int GetQuizAnswerMode(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.ModoRespuesta;
        }

        public List<string> GetKeyWordsFromOpenQuestion(int questionId)
        {
            Pregunta pregunta = (PreguntaAbierta)GetPreguntaById(questionId);
            RespuestaCorrectaAbierta respuesta = (RespuestaCorrectaAbierta)pregunta.respuestaCorrecta;
            dbContext.Entry(respuesta).Reload();
            List<string> keywords = new List<string>();
            foreach (string word in respuesta.GetOpenAnswer())
            {
                keywords.Add(word);
            }
            return keywords;
        }

        public void AddKeyWordToOpenQuestion(int questionId, string keyWord)
        {
            PreguntaAbierta pregunta = (PreguntaAbierta)GetPreguntaById(questionId);
            RespuestaCorrectaAbierta respuesta = (RespuestaCorrectaAbierta)pregunta.respuestaCorrecta;
            dbContext.Entry(respuesta).Reload();
            respuesta.AddOpenAnswer(keyWord);
            UpdateEntity<RespuestaCorrectaAbierta>(respuesta);
            UpdateEntity<PreguntaAbierta>(pregunta);
        }

        public void RemoveKeyWordFromOpenQuestion(int questionId, string keyWord)
        {
            PreguntaAbierta pregunta = (PreguntaAbierta)GetPreguntaById(questionId);
            RespuestaCorrectaAbierta respuesta = (RespuestaCorrectaAbierta)pregunta.respuestaCorrecta;
            dbContext.Entry(respuesta).Reload();
            respuesta.DeleteOpenAnswer(keyWord);
            UpdateEntity<RespuestaCorrectaAbierta>(respuesta);
            UpdateEntity<PreguntaAbierta>(pregunta);
        }

        public int GetRealizaQuizIdFromQuizAndStudent(int quizId, string studentEmail)
        {
            Quiz quiz = GetQuizById(quizId);
            Estudiante estudiante = (Estudiante)GetPersonByEmail(studentEmail);
            RealizaQuiz rq = GetRealizaQuizFromQuizAndStudent(quiz, estudiante);
            if (rq != null)
            {
                return rq.Id;
            }
            else
            {
                return -1;
            }
        }
        public int GetUnfinishedRealizacionId(int quizId, string studentEmail)
        {
            int realizaQuizId = GetRealizaQuizIdFromQuizAndStudent(quizId, studentEmail);
            RealizaQuiz realizaQuiz = GetRealizaQuizById(realizaQuizId);
            if (realizaQuiz != null)
            {
                Realizacion realizacion = realizaQuiz.GetUnfinishedRealizacion();
                if (realizacion != null)
                {
                    return realizacion.Id;
                }
            }
            return -1;
        }
        public int GetRealizacionCurrentQuestionNum(int realizacionId)
        {
            Realizacion realizacion = GetRealizacionById(realizacionId);
            return realizacion.CurrentQuestion;
        }

        public void SetRealizacionCurrentQuestionNum(int realizacionId, int currentQuestionNum)
        {
            Realizacion realizacion = GetRealizacionById(realizacionId);
            realizacion.CurrentQuestion = currentQuestionNum;
            UpdateEntity(realizacion);
        }


        public bool QuizHasRandomQuestions(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.PreguntasAleatorias;
        }
        public void MarkRealizacionAsFinished(int realizacionId)
        {
            Realizacion realizacion = dal.GetById<Realizacion>(realizacionId);
            realizacion.EsTerminado = true;
            CorrectAutomaticallyQuestionsFromRealization(realizacion);
            UpdateEntity(realizacion);
        }
        private void CorrectAutomaticallyQuestionsFromRealization(Realizacion realizacion)
        {
            foreach (RespuestaPregunta respuesta in realizacion.RespuestasPreguntas)
            {
                if (respuesta is RespuestaPreguntaVF)
                {
                    RespuestaPreguntaVF respuestaPreguntaVF = (RespuestaPreguntaVF)respuesta;
                    if (respuestaPreguntaVF.GetVFAnswer() == ((RespuestaCorrectaVF)respuestaPreguntaVF.PreguntaAsociada.respuestaCorrecta).Resultado)
                    {
                        respuestaPreguntaVF.Puntuacion = GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaVF.PreguntaAsociada.Id);
                    }
                    else if (!respuestaPreguntaVF.GetVFAnswer().HasValue)
                    {
                        respuestaPreguntaVF.Puntuacion = 0;
                    }
                    else
                    {
                        respuestaPreguntaVF.Puntuacion = -GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaVF.PreguntaAsociada.Id) / 3.0;
                    }
                    UpdateEntity(respuestaPreguntaVF);
                }
                else if (respuesta is RespuestaPreguntaMultiple)
                {
                    RespuestaPreguntaMultiple respuestaPreguntaMultiple = (RespuestaPreguntaMultiple)respuesta;
                    if (respuestaPreguntaMultiple.GetSeleccionAnswer() == -1)
                    {
                        respuestaPreguntaMultiple.Puntuacion = 0;
                    }
                    else if (((PreguntaSeleccionMultiple)respuestaPreguntaMultiple.PreguntaAsociada).CombosSeleccionMultiple.ElementAt(respuestaPreguntaMultiple.GetSeleccionAnswer()).EsRespuestaCorrecta == true)
                    {
                        respuestaPreguntaMultiple.Puntuacion = GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaMultiple.PreguntaAsociada.Id);
                    }
                    else
                    {
                        respuestaPreguntaMultiple.Puntuacion = -GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaMultiple.PreguntaAsociada.Id) / 3.0;
                    }
                }
                else if (respuesta is RespuestaPreguntaCorrespondencia)
                {
                    RespuestaPreguntaCorrespondencia respuestaPreguntaCorrespondencia = (RespuestaPreguntaCorrespondencia)respuesta;
                    Dictionary<int, int> correspondenciasStudent = respuestaPreguntaCorrespondencia.GetCorrespondenciaAnswer();
                    Dictionary<int, int> correspondenciasCorrectas = ((RespuestaCorrectaCorrespondencia)respuestaPreguntaCorrespondencia.PreguntaAsociada.respuestaCorrecta).ParesCorrectosTerminoFrase;
                    respuestaPreguntaCorrespondencia.Puntuacion = 0;
                    for (int i = 0; i < correspondenciasStudent.Count; i++)
                    {
                        if (correspondenciasStudent[i] != -1)
                        {
                            if (correspondenciasStudent[i] == correspondenciasCorrectas[i])
                            {
                                respuestaPreguntaCorrespondencia.Puntuacion += GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaCorrespondencia.PreguntaAsociada.Id) / correspondenciasStudent.Count;
                            }
                            else
                            {
                                respuestaPreguntaCorrespondencia.Puntuacion -= (GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaCorrespondencia.PreguntaAsociada.Id) / correspondenciasStudent.Count) / 3.0;
                            }
                        }
                    }
                }
                else
                {
                    RespuestaPreguntaAbierta respuestaPreguntaAbierta = (RespuestaPreguntaAbierta)respuesta;
                    List<String> keyWords = ((RespuestaCorrectaAbierta)respuestaPreguntaAbierta.PreguntaAsociada.respuestaCorrecta).GetOpenAnswer();
                    if (keyWords.Count != 0)
                    {
                        double nota = 0.0;
                        foreach (string keyWord in keyWords)
                        {
                            if (respuestaPreguntaAbierta.GetOpenAnswer().ToLower().Contains(keyWord.ToLower()))
                            {
                                nota += GetNotaDeNotaBateriaFromPreguntaAndQuiz(realizacion.RealizaQuizPerteneciente.Quiz.Id, respuestaPreguntaAbierta.PreguntaAsociada.Id) / keyWords.Count;
                            }
                        }
                        respuestaPreguntaAbierta.Puntuacion = nota;
                        UpdateEntity(respuestaPreguntaAbierta);
                    }
                }
            }
        }
        public void SetRemainingTimeRealizacion(int realizacionId, int secondsPassed)
        {
            Realizacion realizacion = dal.GetById<Realizacion>(realizacionId);
            realizacion.TiempoRestante = secondsPassed;
        }

        public int GetRemainingTimeRealizacion(int realizacionId)
        {
            Realizacion realizacion = dal.GetById<Realizacion>(realizacionId);
            return realizacion.TiempoRestante;
        }

        public List<List<String>> GetQuestionsFromRealizacion(int realizacionId)
        {
            Realizacion realizacion = dal.GetById<Realizacion>(realizacionId);

            Quiz quiz = realizacion.RealizaQuizPerteneciente.Quiz;

            List<List<String>> list = new List<List<String>>();
            foreach (RespuestaPregunta rp in realizacion.RespuestasPreguntas)
            {
                Pregunta pregunta = rp.PreguntaAsociada;
                NotaBateria nb = GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, pregunta.Id);
                int type = -1;
                if (pregunta is PreguntaAbierta)
                {
                    type = 0;
                }
                else if (pregunta is PreguntaVerdaderoFalso)
                {
                    type = 1;
                }
                else if (pregunta is PreguntaSeleccionMultiple)
                {
                    type = 2;
                }
                else if (pregunta is PreguntaCorrespondencia)
                {
                    type = 3;
                }
                List<String> questionInfo = new List<string>();
                questionInfo.Add(pregunta.Id.ToString());
                questionInfo.Add(pregunta.Enunciado);
                questionInfo.Add("");
                questionInfo.Add(type.ToString());
                questionInfo.Add(nb.NotaBateriaPreguntas.ToString());
                list.Add(questionInfo);
            }
            return list;
        }
        public int GetQuizTime(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            return quiz.Tiempo;
        }
        public double GetPointsFromQuizRealizacion(int realizacionId)
        {
            Realizacion currentRealizacion = dal.GetById<Realizacion>(realizacionId);
            Quiz quiz = currentRealizacion.RealizaQuizPerteneciente.Quiz;
            double puntos = 0;
            foreach (RespuestaPregunta rp in currentRealizacion.RespuestasPreguntas)
            {
                if (rp is RespuestaPreguntaAbierta)
                {
                }
                else if (rp is RespuestaPreguntaVF)
                {
                    RespuestaPreguntaVF resEstudian = (RespuestaPreguntaVF)rp;
                    RespuestaCorrectaVF resCorrecta = (RespuestaCorrectaVF)rp.PreguntaAsociada.respuestaCorrecta;
                    if (resCorrecta.Resultado == resEstudian.Respuesta)
                    {
                        if (GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas != null)
                        {
                            puntos += GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas;
                        }
                        else
                        {
                            puntos += 0;
                        }

                    }
                    else
                    {
                        if (GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas != null)
                        {
                            puntos -= (GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas * (1 / 3.0));
                        }
                        else
                        {
                            puntos += 0;
                        }
                    }
                }
                else if (rp is RespuestaPreguntaMultiple)
                {
                    RespuestaPreguntaMultiple resEstudian = (RespuestaPreguntaMultiple)rp;
                    if (resEstudian.GetSeleccionAnswer() == -1)
                    {
                        puntos += 0;
                    }
                    else if (((PreguntaSeleccionMultiple)resEstudian.PreguntaAsociada).CombosSeleccionMultiple.ElementAt(resEstudian.GetSeleccionAnswer()).EsRespuestaCorrecta == true)
                    {
                        puntos += GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas;
                    }
                    else
                    {
                        puntos -= (GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas * (1 / 3.0));
                    }
                }
                else if (rp is RespuestaPreguntaCorrespondencia)
                {
                    RespuestaPreguntaCorrespondencia resEstudian = (RespuestaPreguntaCorrespondencia)rp;
                    Dictionary<int, int> correspondenciasStudent = resEstudian.GetCorrespondenciaAnswer();
                    Dictionary<int, int> correspondenciasCorrectas = ((RespuestaCorrectaCorrespondencia)resEstudian.PreguntaAsociada.respuestaCorrecta).ParesCorrectosTerminoFrase;
                    for (int i = 0; i < correspondenciasStudent.Count; i++)
                    {
                        if (correspondenciasStudent[i] != -1)
                        {
                            if (correspondenciasStudent[i] == correspondenciasCorrectas[i])
                            {
                                puntos += (GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas / correspondenciasStudent.Count);
                            }
                            else
                            {
                                puntos -= (GetNotaBateriaFromPreguntaAndQuiz(quiz.Id, rp.PreguntaAsociada.Id).NotaBateriaPreguntas / correspondenciasStudent.Count) / 3.0;
                            }
                        }
                    }
                }
            }
            if (puntos < 0)
            {
                puntos = 0;
            }
            return puntos;
        }
        public void SetPointsToQuizRealizacion(int realizacionId, double points)
        {
            Realizacion currentRealizacion = dal.GetById<Realizacion>(realizacionId);
            currentRealizacion.Puntuacion = points;
            UpdateEntity(currentRealizacion);
        }
        public string GetOpenStudentAnswer(int currentRealizaiconId, int questionId)
        {
            Realizacion realizacion = dal.GetById<Realizacion>(currentRealizaiconId);
            foreach (RespuestaPregunta respuesta in realizacion.RespuestasPreguntas)
            {
                if (respuesta.PreguntaAsociada.Id == questionId)
                {
                    dbContext.Entry<RespuestaPregunta>(respuesta).Reload();
                    string answer = respuesta.GetOpenAnswer();
                    if (answer != null)
                    {
                        return answer;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }
        public int GetQuizStatus(int quizId)
        {
            UpdateAllQuizzesStatus();

            Quiz quiz = dal.GetById<Quiz>(quizId);
            EstadoQuiz estado = quiz.Estado;
            if (estado is BorradorCancelado)
            {
                return -1;
            }
            else if (estado is Borrador)
            {
                return 0;
            }
            else if (estado is PublicadoInactivo)
            {
                return 1;
            }
            else if (estado is PublicadoActivo)
            {
                return 2;
            }
            else if (estado is Finalizado)
            {
                return 3;
            }
            else if (estado is Calificado)
            {
                return 4;
            }
            return -4;
        }

        public DateTime? GetQuizOpeningDate(int quizId)
        {
            Quiz quiz = dal.GetById<Quiz>(quizId);
            return quiz.FechaInicio;
        }

        public DateTime? GetQuizClosingDate(int quizId)
        {
            Quiz quiz = dal.GetById<Quiz>(quizId);
            return quiz.FechaCierre;
        }

        public void UnlaunchQuiz(int quizId)
        {
            Quiz quiz = GetQuizById(quizId);
            quiz.FechaInicio = null;
            quiz.FechaCierre = null;
            quiz.Cancelar();
            UpdateEntity(quiz);
        }
        public void SetQuizViewAnsers(int quizId, bool viewAnswers)
        {
            Quiz quiz = GetQuizById(quizId);
            quiz.VerResultado = viewAnswers;
            UpdateEntity(quiz);
        }

        public string getStatementByPregunta(int preguntaId)
        {
            Pregunta p = GetPreguntaById(preguntaId);
            return p.GetStatement();
        }

        public bool? getVFAnswerByRespuestaPregunta(int respuestaPreguntaId)
        {
            RespuestaPregunta rp = GetRespuestaPreguntaById(respuestaPreguntaId);
            return rp.GetVFAnswer();
        }

        public int getRespuestaPreguntaIdByRealizacionAndCurrentQuestionNum(int realizacionId, int questionNum)
        {
            Realizacion r = GetRealizacionById(realizacionId);
            return r.RespuestasPreguntas.ElementAt(questionNum).Id;
        }

        public int GetRespuestaPreguntaIdFromLastRealizacion(int quizId, string studentEmail, int questionNum)
        {
            RespuestaPregunta rp = GetRespuestaPreguntaFromLastRealizacion(quizId, studentEmail, questionNum);
            return rp.Id;
        }

        public double GetPuntuacionDeRealizacionById(int realizacionId)
        {
            Realizacion r = GetRealizacionById(realizacionId);
            return r.Puntuacion;
        }

        public int GetLasRealizacionIdFromRealizaQuiz(int quizEstudiante)
        {
            return GetRealizaQuizById(quizEstudiante).GetLastRealizacion().Id;
        }

        public ICollection<ICollection<int>> GetOrderedQuizAnswers(ICollection<string> estudiantesQueHanHechoQuiz, int quizId)
        {
            ICollection<ICollection<int>> respuestasOrdenadas = new List<ICollection<int>>();
            foreach (string e in estudiantesQueHanHechoQuiz)
            {
                if (GetRealizaQuizFromQuizAndStudent(GetQuizById(quizId), GetEstudianteByEmail(e)) != null)
                {
                    int? quizDeEstudiante = GetRealizaQuizFromQuizAndStudent(GetQuizById(quizId), GetEstudianteByEmail(e)).Id;
                    if (quizDeEstudiante != null)
                    {
                        //Ultima realizacion de estudiante
                        int realizacionDeEstudiante = GetRealizaQuizById((int)quizDeEstudiante).GetLastRealizacion().Id;

                        //Ordenar RespuestaPregunta
                        ICollection<int> realizacionEstudiante = new List<int>();
                        int minID = Int32.MaxValue;
                        int mayorQue = -1;
                        int numPreguntasRecolocadas = 0;
                        while (numPreguntasRecolocadas < GetRealizacionById(realizacionDeEstudiante).RespuestasPreguntas.Count)
                        {
                            minID = Int32.MaxValue;
                            int auxRespuestaPregunta;
                            int indiceRespuestaPregunta = 0;
                            int indiceActual = 0;
                            for (int i = 0; i < GetRealizacionById(realizacionDeEstudiante).RespuestasPreguntas.Count; i++)
                            {
                                int idPregunta = GetRealizacionById(realizacionDeEstudiante).RespuestasPreguntas.ElementAt(i).PreguntaAsociada.Id;
                                if (idPregunta < minID && idPregunta > mayorQue)
                                {
                                    indiceActual = indiceRespuestaPregunta;
                                    minID = idPregunta;
                                }
                                indiceRespuestaPregunta++;
                            }

                            auxRespuestaPregunta = GetRealizacionById(realizacionDeEstudiante).RespuestasPreguntas.ElementAt(indiceActual).Id;
                            mayorQue = minID;
                            if (GetQuestionType(minID) == 0)
                            {
                                realizacionEstudiante.Add(auxRespuestaPregunta);
                            }
                            else if (GetQuestionType(minID) == 1)
                            {
                                realizacionEstudiante.Add(auxRespuestaPregunta);
                            }
                            numPreguntasRecolocadas++;
                        }
                        realizacionEstudiante.Add((int)quizDeEstudiante);
                        respuestasOrdenadas.Add(realizacionEstudiante);
                    }
                }
            }
            return respuestasOrdenadas;
        }

        public int GetQuestionAnswerType(int respuestaPreguntaId)
        {
            RespuestaPregunta rp = dal.GetById<RespuestaPregunta>(respuestaPreguntaId);

            if (rp is RespuestaPreguntaAbierta)
            {
                return 0;
            }
            else if (rp is RespuestaPreguntaVF)
            {
                return 1;
            }
            else if (rp is RespuestaPreguntaMultiple)
            {
                return 2;
            }
            else if (rp is RespuestaPreguntaCorrespondencia)
            {
                return 3;
            }
            return -1;
        }


        public bool QuizTienePreguntasAleatorias(int quizId)
        {
            return GetQuizById(quizId).PreguntasAleatorias;
        }

        public int GetQuestionToOpen(int realizacionId, int currentQuestion)
        {
            return GetRealizacionById(realizacionId).RespuestasPreguntas.ElementAt(currentQuestion).PreguntaAsociada.Id;
        }

        public int GetRespuestaPreguntaFromRealizacion(int realizacionId, int currentQuestion)
        {
            return GetRealizacionById(realizacionId).RespuestasPreguntas.ElementAt(currentQuestion).Id;
        }

        public double GetNotaDeNotaBateriaFromPreguntaAndQuiz(int quizId, int QuestionToOpenId)
        {
            return GetNotaBateriaFromPreguntaAndQuiz(quizId, QuestionToOpenId).NotaBateriaPreguntas;
        }

        public int GetRespuestaDeUnEstudiante(int respuestaUnEstudiante, int currentQuestion)
        {
            return GetRealizacionById(respuestaUnEstudiante).RespuestasPreguntas.ElementAt(currentQuestion).Id;
        }

        public bool RespuestaIsNotNull(int respuestaPreguntaId)
        {
            if (GetQuestionAnswerType(respuestaPreguntaId) == 0)
            {
                RespuestaPreguntaAbierta rp = (RespuestaPreguntaAbierta)GetRespuestaPreguntaById(respuestaPreguntaId);
                return rp.Respuesta != null;
            }
            else if (GetQuestionAnswerType(respuestaPreguntaId) == 1)
            {
                RespuestaPreguntaVF rp = (RespuestaPreguntaVF)GetRespuestaPreguntaById(respuestaPreguntaId);
                return rp.Respuesta != null;
            }
            else if (GetQuestionAnswerType(respuestaPreguntaId) == 2)
            {
                RespuestaPreguntaMultiple rp = (RespuestaPreguntaMultiple)GetRespuestaPreguntaById(respuestaPreguntaId);
                return rp.Respuesta != null;
            }
            else
            {
                return true;
            }

        }

        public int GetPreguntasEnRealizacaion(int realizacionId)
        {
            return GetRealizacionById(realizacionId).RespuestasPreguntas.Count;
        }

        public int GetNumBateriasFromQuiz(int quizId)
        {
            return GetQuizById(quizId).GetNumBateriasPreguntas();
        }

        public void AddItemABateria(int quizId, int ItemId)
        {
            getQuizById(quizId).AddBateriaPreguntas(GetBatteryById(ItemId));
        }

        public string? GetOpenAnswerRespuestaPreguntaAbierta(int rpa)
        {
            return GetRespuestaPreguntaAbiertaById(rpa).GetOpenAnswer();
        }

        public List<string> AddContenidoByCurso(string personEmail, string cursoSelected)
        {
            return GetTopicsFromCourse(GetCourseByInstructorAndName(personEmail, cursoSelected));
        }

        public int GetQuestionType(int questionId)
        {
            Pregunta p = dal.GetById<Pregunta>(questionId);

            if (p is PreguntaAbierta)
            {
                return 0;
            }
            else if (p is PreguntaVerdaderoFalso)
            {
                return 1;
            }
            else if (p is PreguntaSeleccionMultiple)
            {
                return 2;
            }
            else if (p is PreguntaCorrespondencia)
            {
                return 3;
            }
            return -1;
        }
        public int GetQuizNumberOfTries(int quizId)
        {
            Quiz quiz = dal.GetById<Quiz>(quizId);
            return quiz.NumeroIntentos;
        }
        public List<List<String>> GetAllQuestions()
        {
            List<List<String>> list = new List<List<String>>();
            foreach (BateriaPreguntas bp in GetAllBatteries())
            {
                foreach (NotaBateria nb in bp.NotasBaterias)
                {
                    foreach (Pregunta pregunta in bp.Preguntas)
                    {
                        List<String> questionInfo = new List<string>();
                        questionInfo.Add(pregunta.Id.ToString());
                        questionInfo.Add(pregunta.Enunciado);
                        questionInfo.Add("");
                        questionInfo.Add(nb.Quiz.Nombre);
                        questionInfo.Add(nb.Quiz.Contenido.Nombre);
                        questionInfo.Add(GetBatteryTypeString(bp));
                        if (pregunta.ContenidoPertenece == null)
                        {
                            questionInfo.Add("");
                        }
                        else
                        {
                            questionInfo.Add(pregunta.ContenidoPertenece.Nombre);
                        }
                        questionInfo.Add(nb.Quiz.Id + "");
                        list.Add(questionInfo);
                    }
                }
            }
            return list;
        }

        public List<List<String>> GetRealizationsFromQuiz(int quizId, bool endAllTests)
        {
            List<List<String>> list = new List<List<String>>();
            Quiz quiz = GetQuizById(quizId);
            dbContext.Entry(quiz).Reload();
            foreach (RealizaQuiz realizaQuiz in quiz.Realizaciones)
            {
                dbContext.Entry<RealizaQuiz>(realizaQuiz).Reload();
                int numberOfRealizationsPerStudent = 0;
                foreach (Realizacion realizacion in realizaQuiz.Realizaciones)
                {
                    if (endAllTests)
                    {
                        if (!realizacion.EsTerminado)
                        {
                            MarkRealizacionAsFinished(realizacion.Id);
                        }
                    }
                    dbContext.Entry<Realizacion>(realizacion).Reload();
                    List<String> realizacionInfo = new List<string>();
                    realizacionInfo.Add(realizacion.Id.ToString());
                    realizacionInfo.Add(realizaQuiz.Estudiante.Email);
                    realizacionInfo.Add(realizaQuiz.Estudiante.Nombre);
                    realizacionInfo.Add(numberOfRealizationsPerStudent.ToString());
                    realizacionInfo.Add(realizacion.EsTerminado ? "1" : "0");
                    numberOfRealizationsPerStudent++;
                    list.Add(realizacionInfo);
                }
            }
            return list;
        }

        public List<List<String>> GetRespuestasPreguntasFromRealizacion(int realizationId)
        {
            Realizacion realizacion = GetRealizacionById(realizationId);
            dbContext.Entry<Realizacion>(realizacion).Reload();
            List<List<String>> list = new List<List<String>>();
            foreach (RespuestaPregunta respuestaPregunta in realizacion.RespuestasPreguntas)
            {
                dbContext.Entry<RespuestaPregunta>(respuestaPregunta).Reload();
                List<String> questionInfo = new List<string>();
                questionInfo.Add(respuestaPregunta.Id.ToString());
                questionInfo.Add(respuestaPregunta.PreguntaAsociada.Id.ToString());
                questionInfo.Add(respuestaPregunta.PreguntaAsociada.Enunciado);
                questionInfo.Add(GetQuestionType(respuestaPregunta.PreguntaAsociada.Id).ToString());
                list.Add(questionInfo);
            }
            return list;
        }

        public double? GetPointsFromRealizationQuestion(int respuestaPreguntaId)
        {
            double? points = GetRespuestaPreguntaById(respuestaPreguntaId).Puntuacion;
            return points;
        }

        public void SetPointsFromRealizationQuestion(int respuestaPreguntaId, double points)
        {
            RespuestaPregunta respuestaPregunta = GetRespuestaPreguntaById(respuestaPreguntaId);
            respuestaPregunta.Puntuacion = points;
            UpdateEntity(respuestaPregunta);
        }
        public void SetQuizAsCorrected(int quizId, List<int> realizacionIds)
        {
            foreach (int id in realizacionIds)
            {
                Realizacion r = GetRealizacionById(id);
                r.Puntuacion = 0.0;
                foreach (RespuestaPregunta rp in r.RespuestasPreguntas)
                {
                    r.Puntuacion += (double)rp.Puntuacion;
                }
                UpdateEntity(r);
            }
            Quiz quiz = GetQuizById(quizId);
            quiz.Calificar();
            UpdateEntity(quiz);
        }
        public void SetSeleccionCorrectAnswer(int questionId, bool op1, bool op2, bool op3, bool op4)
        {
            PreguntaSeleccionMultiple pregunta = (PreguntaSeleccionMultiple)GetQuestionById(questionId);
            pregunta.CombosSeleccionMultiple.ElementAt(0).EsRespuestaCorrecta = op1;
            pregunta.CombosSeleccionMultiple.ElementAt(1).EsRespuestaCorrecta = op2;
            pregunta.CombosSeleccionMultiple.ElementAt(2).EsRespuestaCorrecta = op3;
            pregunta.CombosSeleccionMultiple.ElementAt(3).EsRespuestaCorrecta = op4;
            UpdateEntity(pregunta);
        }

        public void SetSeleccionTextOptions(int questionId, string op1, string op2, string op3, string op4)
        {
            PreguntaSeleccionMultiple pregunta = (PreguntaSeleccionMultiple)GetQuestionById(questionId);
            pregunta.CombosSeleccionMultiple.ElementAt(0).TextoRespuesta = op1;
            pregunta.CombosSeleccionMultiple.ElementAt(1).TextoRespuesta = op2;
            pregunta.CombosSeleccionMultiple.ElementAt(2).TextoRespuesta = op3;
            pregunta.CombosSeleccionMultiple.ElementAt(3).TextoRespuesta = op4;
            UpdateEntity(pregunta);
        }
        public List<string> GetSeleccionTextOptions(int questionId)
        {
            PreguntaSeleccionMultiple pregunta = (PreguntaSeleccionMultiple)GetQuestionById(questionId);
            List<string> options = new List<string>();
            foreach (ComboSeleccionMultiple combo in pregunta.CombosSeleccionMultiple)
            {
                options.Add(combo.TextoRespuesta);
            }
            return options;
        }

        public bool IsQuizActiveFromPregunta(int preguntaId, int quizId)
        {
            Quiz quiz = getQuizById(quizId);
            if (quiz == null)
            {
                return false;
            }
            if (quiz.FechaCierre <= DateTime.Today.Date || quiz.FechaCierre == null)
            {
                return true;
            }
            return false;
        }

        private List<Contenido> GetAllTopics()
        {
            List<Contenido> aux = dal.GetAll<Contenido>().ToList();
            return aux;
        }

        public List<List<string>> GetAllTopicsString()
        {
            List<List<string>> res = new List<List<string>>();
            foreach (Contenido contenido in GetAllTopics())
            {
                List<string> contenidoInfo = new List<string>();
                contenidoInfo.Add(contenido.Id + "");
                contenidoInfo.Add(contenido.Nombre);
                res.Add(contenidoInfo);
            }
            return res;
        }

        public void SetTopicToQuestion(int contenidoId, int preguntaId)
        {
            Contenido c = GetContenidoById(contenidoId);
            Pregunta p = GetPreguntaById(preguntaId);
            p.ContenidoPertenece = c;
            UpdateEntity(p);
        }

        public void SetSeleccionStudentAnswer(int currentRealizacionId, int questionId, int comboIndex)
        {
            RespuestaPreguntaMultiple rpm = dal.GetById<RespuestaPreguntaMultiple>(GetRespuestaPregunta(questionId, currentRealizacionId));
            rpm.SetSeleccionAnswer(comboIndex);
            UpdateEntity(rpm);
        }

        public int GetSeleccionStudentAnswer(int currentRealizacionId, int questionId)
        {
            RespuestaPreguntaMultiple rpm = dal.GetById<RespuestaPreguntaMultiple>(GetRespuestaPregunta(questionId, currentRealizacionId));
            dbContext.Entry<RespuestaPreguntaMultiple>(rpm).Reload();
            return rpm.GetSeleccionAnswer();
        }

        public int GetSeleccionStudentAnswerRespuestaMultiple(int respuestaId, int questionId)
        {
            RespuestaPreguntaMultiple rpm = dal.GetById<RespuestaPreguntaMultiple>(respuestaId);
            return rpm.GetSeleccionAnswer();
        }
        public int GetSelectionAnswerFromAnswerId(int respuestaPreguntaId)
        {
            RespuestaPregunta rp = GetRespuestaPreguntaById(respuestaPreguntaId);
            return rp.GetSeleccionAnswer();
        }
        public List<bool> GetSeleccionCorrectAnswer(int questionId)
        {
            List<bool> answers = new List<bool>();
            PreguntaSeleccionMultiple question = (PreguntaSeleccionMultiple)GetQuestionById(questionId);
            foreach (ComboSeleccionMultiple c in question.CombosSeleccionMultiple)
            {
                answers.Add(c.EsRespuestaCorrecta);
            }
            return answers;
        }

        public List<string> GetCorrespondenciaFrases(int questionId)
        {
            PreguntaCorrespondencia question = (PreguntaCorrespondencia)GetQuestionById(questionId);
            List<string> frases = question.GetFrases();
            return frases;
        }
        public List<string> GetCorrespondenciaTerminos(int questionId)
        {
            PreguntaCorrespondencia question = (PreguntaCorrespondencia)GetQuestionById(questionId);
            List<string> terminos = question.GetTerminos();
            return terminos;
        }

        public void UpdateAllQuizzesStatus()
        {
            List<Quiz> quizzes = dal.GetAll<Quiz>().ToList();
            foreach (Quiz quiz in quizzes)
            {
                if (quiz.Estado is PublicadoInactivo
                    && quiz.FechaInicio < DateTime.Now)
                {
                    quiz.Activar();
                }
                if (quiz.Estado is PublicadoActivo
                    && quiz.FechaCierre < DateTime.Now)
                {
                    quiz.Finalizar();
                }
                UpdateEntity(quiz);
            }
        }
        public void FinishQuizImmediately(int quizId)
        {
            Quiz quiz = getQuizById(quizId);
            quiz.Finalizar();
            UpdateEntity(quiz);
        }
        public void SetCorrespondenciaFraseTermino(int questionId, int terminoIndex, int fraseIndex)
        {
            RespuestaCorrectaCorrespondencia respuesta = (RespuestaCorrectaCorrespondencia)((PreguntaCorrespondencia)GetQuestionById(questionId)).respuestaCorrecta;
            respuesta.ParesCorrectosTerminoFrase[terminoIndex] = fraseIndex;
            UpdateEntity(respuesta);
        }
        public void SetCorrespondenciaFrasesTerminosText(int questionId, string[] frases, string[] terminos)
        {
            PreguntaCorrespondencia pregunta = (PreguntaCorrespondencia)GetQuestionById(questionId);
            pregunta.SetFrases(frases);
            pregunta.SetTerminos(terminos);
            UpdateEntity(pregunta);
        }
        public void SetCorrespondenciaStudentAnswer(int currentRealizacionId, int questionId, int terminoIndex, int fraseIndex)
        {
            RespuestaPreguntaCorrespondencia rpc = dal.GetById<RespuestaPreguntaCorrespondencia>(GetRespuestaPregunta(questionId, currentRealizacionId));
            rpc.SetCorrespondenciaAnswer(terminoIndex, fraseIndex);
            UpdateEntity(rpc);
        }

        public String GetNumberOfRealizaciones(int quizId, int cursoId)
        {
            Quiz q = getQuizById(quizId);
            Curso c = GetCursoById(cursoId);
            if (q.GetNumberOfAttempts() == -1)
            {
                return "Ilimitados";
            }
            int numRealizacionesDeQuiz = 0;
            foreach (Estudiante e in c.Estudiantes)
            {
                RealizaQuiz rq = GetRealizaQuizFromQuizAndStudent(q, e);
                if (rq != null)
                {
                    foreach (Realizacion r in rq.Realizaciones)
                    {
                        if (r.EsTerminado)
                        {
                            numRealizacionesDeQuiz++;
                        }
                    }
                }
            }
            return numRealizacionesDeQuiz + " de " + (c.Estudiantes.Count * q.NumeroIntentos);
        }

        public double GetAverageMarkOfQuiz(int quizId, int cursoId)
        {
            Quiz q = getQuizById(quizId);
            Curso c = GetCursoById(cursoId);
            int numEstudiantes = c.Estudiantes.Count;
            double notas = 0.0;
            foreach (Estudiante e in c.Estudiantes)
            {
                RealizaQuiz rq = GetRealizaQuizFromQuizAndStudent(q, e);
                if (rq != null)
                {
                    Realizacion r = rq.GetLastRealizacion();
                    notas += r.Puntuacion;
                }
            }
            double media = (notas / numEstudiantes);
            return media;
        }

        public double GetStandardDeviation(int quizId, int cursoId)
        {
            List<double> notas = new List<double>();
            Quiz q = getQuizById(quizId);
            Curso c = GetCursoById(cursoId);
            foreach (Estudiante e in c.Estudiantes)
            {
                RealizaQuiz rq = GetRealizaQuizFromQuizAndStudent(q, e);
                if (rq != null)
                {
                    Realizacion r = rq.GetLastRealizacion();
                    notas.Add(r.Puntuacion);
                }
                else
                {
                    notas.Add(0.0);
                }
            }

            double result = 0.0;
            if (notas.Any())
            {
                double average = notas.Average();
                double sum = notas.Sum(d => Math.Pow(d - average, 2));
                result = Math.Sqrt((sum) / notas.Count());
            }
            return result;
        }

        public ICollection<double> GetAllMarks(int quizId, int cursoId)
        {
            ICollection<double> notas = new List<double>();
            ICollection<Estudiante> estudiantes = getCursoById(cursoId).Estudiantes;
            Quiz q = getQuizById(quizId);
            double notaMax = 0.0;
            foreach (NotaBateria nb in q.NotasBateria)
            {
                notaMax += nb.NotaBateriaPreguntas;
            }
            foreach (Estudiante e in estudiantes)
            {
                RealizaQuiz rq = GetRealizaQuizFromQuizAndStudent(q, e);
                if (rq != null)
                {
                    double notaPonderada = 10.0 * rq.GetLastRealizacion().Puntuacion / notaMax;
                    notas.Add(notaPonderada);
                }
                else
                {
                    notas.Add(0);
                }
            }
            return notas;
        }

        public double GetNotaMediaDePregunta(ICollection<int> respuestasAbiertas)
        {
            ICollection<double> respuestas = new List<double>();
            foreach (int res in respuestasAbiertas)
            {
                RespuestaPreguntaAbierta rpa = GetRespuestaPreguntaAbiertaById(res);
                if (rpa.Puntuacion != null) { respuestas.Add((double)rpa.Puntuacion); }
                else { return -10.0; }

            }
            return respuestas.Average();
        }

        public Dictionary<int, int> GetCorrespondenciaCorrectAnswer(int questionId)
        {
            PreguntaCorrespondencia question = (PreguntaCorrespondencia)GetQuestionById(questionId);
            RespuestaCorrectaCorrespondencia rpc = (RespuestaCorrectaCorrespondencia)question.respuestaCorrecta;
            return rpc.ParesCorrectosTerminoFrase;
        }
        public Dictionary<int, int> GetCorrespondenciaStudentAnswer(int currentRealizacionId, int questionId)
        {
            RespuestaPreguntaCorrespondencia rpc = dal.GetById<RespuestaPreguntaCorrespondencia>(GetRespuestaPregunta(questionId, currentRealizacionId));
            return rpc.GetCorrespondenciaAnswer();
        }
        public Dictionary<int, int> getCorrespondenciaAnswerByRespuestaPregunta(int respuestaPreguntaId)
        {
            RespuestaPreguntaCorrespondencia rp = dal.GetById<RespuestaPreguntaCorrespondencia>(respuestaPreguntaId);
            return rp.GetCorrespondenciaAnswer();
        }
        public void SetCorrespondenciaNumberPhrases(int questionId, int num)
        {
            PreguntaCorrespondencia question = (PreguntaCorrespondencia)GetQuestionById(questionId);
            question.changeNumFrases(num);
            UpdateEntity(question);
        }
        public void initPreguntaCorrespondencia(int questionId, string frase, string termino)
        {
            PreguntaCorrespondencia question = (PreguntaCorrespondencia)GetQuestionById(questionId);
            question.initPreguntaCorrespondencia(frase, termino);
            UpdateEntity(question);
        }
        public void CrearQuizDeRecuperacion(int quizId, string nombre, string autorEmail, DateTime fechaCreacion)
        {
            Quiz quiz = this.GetQuizById(quizId);
            QuizRecuperacion QuizRec = new QuizRecuperacion(quiz, nombre, fechaCreacion, this.GetInstructorByEmail(autorEmail));
            dal.Insert<Quiz>(QuizRec);
            Commit();
        }
    }
}
