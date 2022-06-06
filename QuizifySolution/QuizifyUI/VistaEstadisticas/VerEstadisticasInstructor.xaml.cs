using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para VerEstadisticasInstructor.xaml
    /// </summary>
    public partial class VerEstadisticasInstructor : Window
    {
        private IQuizifyService service;
        private int currentQuestionNum;

        private int quizId;
        private int? cursoId;
        private string StudentEmail;

        private int realizacionId;

        //RealizaQuiz -> id=int
        private ICollection<int> examenesDeEstudiantes;
        //Realizacion -> id=int
        private ICollection<int> todasRespuestasDeTodosEstudiantes;
        //ICollection<ICollection<Preguntas>> -> id=int
        private ICollection<ICollection<int>> listaTodasRespuestasDeTodosEstudiantes;
        //Estudiante -> id=string
        private ICollection<String> estudiantesQueHanHechoQuiz;
        private string preguntaString = "Pregunta ";
        private string of = " de ";
        private string estudiantesZero = "Estudiantes: 0 de ";
        private string estudiantesNormal = "Estudiantes: ";
        private string quizEstudianteString = "Quiz del estudiante ";
        public VerEstadisticasInstructor(int qId, int cId, IQuizifyService ser)
        {
            InitializeComponent();
            actualizar();
            this.service = ser;

            this.quizId = qId;
            this.cursoId = cId;

            this.CursoAsignatura.Content = service.GetQuizName(quizId);
            this.NotaQuiz.Content = "" + service.GetValorQuiz(quizId);

            //Lista de IDs de los exámenes hechos por los estudiantes del curso
            this.estudiantesQueHanHechoQuiz = service.StudentsInACourse((int)cursoId);

            examenesDeEstudiantes = new List<int>();
            //Añadimos los RealizaQuiz de los estudiantes
            for (int i = 0; i < this.estudiantesQueHanHechoQuiz.Count; i++)
            {
                String StudentMail = estudiantesQueHanHechoQuiz.ElementAt(i);
                if (service.IsPersonAStudent(StudentMail))
                {
                    if (!service.GetRealizaQuizFromQuizAndStudentIsNull(quizId, StudentMail))
                    {
                        int ExamenEstudiante = service.GetRealizaQuizIdFromQuizAndStudent(quizId, StudentEmail);
                        if (ExamenEstudiante != -1)
                        {
                            this.examenesDeEstudiantes.Add(ExamenEstudiante);
                        }
                    }
                }
            }


            if (service.QuizTienePreguntasAleatorias(quizId))
            {
                //Realizacion
                this.listaTodasRespuestasDeTodosEstudiantes = service.GetOrderedQuizAnswers(estudiantesQueHanHechoQuiz, quizId);
            }
            else
            {
                this.todasRespuestasDeTodosEstudiantes = respuestasDelQuizDeEstudiantes();
            }

            currentQuestionNum = 0;
            LoadCurrentQuestion();
        }

        public VerEstadisticasInstructor(int qId, string emailPersona, int realId, IQuizifyService service)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.quizId = qId;
            this.StudentEmail = emailPersona;
            this.cursoId = 0;

            this.realizacionId = realId;

            this.NotaQuiz.Content = "" + service.GetPuntuacionDeRealizacionById(realizacionId) + "/" + service.GetValorQuiz(quizId);
            currentQuestionNum = 0;
            this.CursoAsignatura.Content = service.GetQuizName(quizId);
            LoadCurrentQuestionEstudiante();
        }

        private ICollection<int> respuestasDelQuizDeEstudiantes()
        {
            ICollection<int> respuestasEstudiantes = new List<int>();
            foreach (string e in estudiantesQueHanHechoQuiz)
            {
                if (!service.GetRealizaQuizFromQuizAndStudentIsNull(quizId, e))
                {
                    int? quizDeEstudiante = service.GetRealizaQuizIdFromQuizAndStudent(quizId, e);
                    if (quizDeEstudiante != -1)
                    {
                        int realizacionDeEstudiante = service.GetLasRealizacionIdFromRealizaQuiz((int)quizDeEstudiante);
                        respuestasEstudiantes.Add(realizacionDeEstudiante);
                    }
                }
            }
            return respuestasEstudiantes;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionNum++;
            if (this.cursoId == 0)
            {
                LoadCurrentQuestionEstudiante();
            }
            else
            {
                LoadCurrentQuestion();
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionNum--;
            if (this.cursoId == 0)
            {
                LoadCurrentQuestionEstudiante();
            }
            else
            {
                LoadCurrentQuestion();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void LoadCurrentQuestion()
        {
            NumPregunta.Content = preguntaString + (currentQuestionNum + 1) + of + service.GetNumBateriasFromQuiz(quizId);
            bool aleatorio = service.QuizTienePreguntasAleatorias(quizId);

            if ((aleatorio && listaTodasRespuestasDeTodosEstudiantes.Count == 0) || (!aleatorio && todasRespuestasDeTodosEstudiantes.Count == 0))
            {
                NumEstudiantesQueHanHechoElQuiz.Content = estudiantesZero + estudiantesQueHanHechoQuiz.Count;
            }
            else
            {
                ICollection<int> respuestasVFEstudiantes = new List<int>();
                ICollection<int> respuestasAbiertaEstudiantes = new List<int>();
                ICollection<int> respuestasSeleccionEstudiantes = new List<int>();
                ICollection<int> respuestasCorrespondenciaEstudiantes = new List<int>();
                int QuestionToOpenId;
                if (aleatorio)
                {
                    NumEstudiantesQueHanHechoElQuiz.Content = estudiantesNormal + listaTodasRespuestasDeTodosEstudiantes.Count + of + estudiantesQueHanHechoQuiz.Count;
                    QuestionToOpenId = service.GetQuestionToOpen(listaTodasRespuestasDeTodosEstudiantes.ElementAt(0).ElementAt(listaTodasRespuestasDeTodosEstudiantes.ElementAt(0).Count-1), currentQuestionNum);
                }
                else
                {
                    NumEstudiantesQueHanHechoElQuiz.Content = estudiantesNormal + todasRespuestasDeTodosEstudiantes.Count + of + estudiantesQueHanHechoQuiz.Count;
                    QuestionToOpenId = service.GetQuestionToOpen(todasRespuestasDeTodosEstudiantes.ElementAt(0), currentQuestionNum);
                    //Realizacion
                    foreach (int respuestaUnEstudiante in todasRespuestasDeTodosEstudiantes)
                    {
                        //RespuestaPregunta -> Respuesta del estudiante a la pregunta X
                        int respuestaDeUnEstudiante = service.GetRespuestaDeUnEstudiante(respuestaUnEstudiante, currentQuestionNum);
                        if (service.GetQuestionAnswerType(respuestaDeUnEstudiante) == 0)
                        {
                            respuestasAbiertaEstudiantes.Add(respuestaDeUnEstudiante);
                        }
                        else if (service.GetQuestionAnswerType(respuestaDeUnEstudiante) == 1)
                        {
                            respuestasVFEstudiantes.Add(respuestaDeUnEstudiante);
                        }
                        else if (service.GetQuestionAnswerType(respuestaDeUnEstudiante) == 2)
                        {
                            respuestasSeleccionEstudiantes.Add(respuestaDeUnEstudiante);
                        }
                        else if (service.GetQuestionAnswerType(respuestaDeUnEstudiante) == 3)
                        {
                            respuestasCorrespondenciaEstudiantes.Add(respuestaDeUnEstudiante);
                        }
                    }
                    if (service.GetQuestionType(QuestionToOpenId) == 0)
                    {
                        QuestionFrame.Navigate(new ListaDeEstudiantesParaPreguntaAbierta(estudiantesQueHanHechoQuiz, respuestasAbiertaEstudiantes, QuestionToOpenId, quizId, service));
                    }
                    else if (service.GetQuestionType(QuestionToOpenId) == 1)
                    {
                        QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(QuestionToOpenId, respuestasVFEstudiantes, true, service));
                    }
                    else if (service.GetQuestionType(QuestionToOpenId) == 2)
                    {
                        QuestionFrame.Navigate(new PreguntaSeleccionMultiplePage(QuestionToOpenId, respuestasSeleccionEstudiantes, service));
                    }
                    else if (service.GetQuestionType(QuestionToOpenId) == 3)
                    {
                        //QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(QuestionToOpenId, respuestasVFEstudiantes, true, service));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                PuntuacionPregunta.Content = service.GetNotaDeNotaBateriaFromPreguntaAndQuiz(quizId, QuestionToOpenId) + " pts";
            }
            LogicaDeBotones(service.GetNumBateriasFromQuiz(quizId));

        }

        private void LoadCurrentQuestionEstudiante()
        {
            NumPregunta.Content = preguntaString + (currentQuestionNum + 1) + of + service.GetPreguntasEnRealizacaion(realizacionId);
            NumEstudiantesQueHanHechoElQuiz.Content = quizEstudianteString + service.GetNombreEstudianteByEmail(StudentEmail);
            int QuestionToOpenId = service.GetQuestionToOpen(realizacionId, currentQuestionNum);
            int respuestaPreguntaEstudiante = service.GetRespuestaPreguntaFromRealizacion(realizacionId, currentQuestionNum);
            PuntuacionPregunta.Content = service.GetNotaDeNotaBateriaFromPreguntaAndQuiz(quizId, QuestionToOpenId) + "";

            if (service.GetQuestionType(QuestionToOpenId) == 0)
            {
                QuestionFrame.Navigate(new PreguntaAbiertaPage(QuestionToOpenId, service.getRespuestaPreguntaIdByRealizacionAndCurrentQuestionNum(realizacionId, currentQuestionNum), service, false));
            }
            else if (service.GetQuestionType(QuestionToOpenId) == 1)
            {
                QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(QuestionToOpenId, service.getRespuestaPreguntaIdByRealizacionAndCurrentQuestionNum(realizacionId, currentQuestionNum), true, service));
            }
            else if (service.GetQuestionType(QuestionToOpenId) == 2)
            {
                QuestionFrame.Navigate(new PreguntaSeleccionMultiplePage(QuestionToOpenId, service.getRespuestaPreguntaIdByRealizacionAndCurrentQuestionNum(realizacionId, currentQuestionNum), service));
            }
            else if (service.GetQuestionType(QuestionToOpenId) == 3)
            {
                //QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(QuestionToOpenId, respuestasVFEstudiantes, true, service));
            }
            else
            {
                throw new NotImplementedException();
            }
            LogicaDeBotones(service.GetPreguntasEnRealizacaion(realizacionId));
        }

        private void LogicaDeBotones(int maxQuestion)
        {
            if (currentQuestionNum == 0)
            {
                PreviousButton.IsEnabled = false;
            }
            else
            {
                PreviousButton.IsEnabled = true;
            }
            if (currentQuestionNum == maxQuestion - 1)
            {
                NextButton.IsEnabled = false;
            }
            else
            {
                NextButton.IsEnabled = true;
            }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["EstadisticasTitulo"];
            NumEstudiantesQueHanHechoElQuiz.Content = Idioma.info["NumEstudiantesQueHanHechoElQuiz"];
            NumPregunta.Content = Idioma.info["NumPregunta"];
            NotaLabel.Content = Idioma.info["NotaLabel"];
            pregPointsLabel.Content = Idioma.info["pregPointsLabel"];
            PreviousButton.Content = Idioma.info["PreviousButton"];
            NextButton.Content = Idioma.info["NextButton"];
            ExitButton.Content = Idioma.info["ExitButton"];
            preguntaString = Idioma.info["preguntaString"];
            of = Idioma.info["of"];
            estudiantesZero = Idioma.info["estudiantesZero"];
            estudiantesNormal = Idioma.info["estudiantesNormal"];
            quizEstudianteString = Idioma.info["quizEstudianteString"];
        }
    }
}
