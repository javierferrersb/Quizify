using Quizify.Services;
using System;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for CreateQuiz.xaml
    /// </summary>
    public partial class RevisarResultado : Window
    {
        private int quizId;
        private String curso;
        private int cursoId;
        private String subject;
        private int topicId;
        private int currentQuestionNum;
        private string studentEmail;
        private int quizSize;
        private IQuizifyService service;
        private string preguntaString = "Pregunta ";
        private string of = " de ";
        private string sobre = " sobre ";
        public RevisarResultado(IQuizifyService service, int quizId, string personEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.quizId = quizId;
            this.topicId = service.GetTopicFromQuiz(quizId);
            this.cursoId = service.GetCourseOfTopic(topicId);
            this.studentEmail = personEmail;
            this.subject = service.GetTopicName(topicId);
            this.curso = service.GetCourseName(cursoId);
            this.CursoAsignatura.Content = curso + " > " + subject + " > " + service.GetQuizName(quizId);
            this.NotaQuiz.Content = service.GetNotaLastRealizacion(quizId, studentEmail) + " pts";
            this.quizSize = service.GetQuizSize(quizId);
            this.currentQuestionNum = 0;
            LoadCurrentQuestion();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionNum++;
            LoadCurrentQuestion();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionNum--;
            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            NumPregunta.Content = preguntaString + (currentQuestionNum + 1) + of + quizSize;
            int pregId = service.GetPreguntaIdFromLastRealizacion(quizId, studentEmail, currentQuestionNum);
            if (service.GetQuestionType(pregId) == 0)
            {
                this.PuntuacionPregunta.Content = 0 + sobre + service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) + " pts";
                QuestionFrame.Navigate(new PreguntaAbiertaPage(pregId, service.GetIdRespuestaPreguntaFromLastRealizacion(quizId, studentEmail, currentQuestionNum), true, service));
            }
            else if (service.GetQuestionType(pregId) == 1)
            {
                if (service.RespondidoCorrectamenteVF(quizId, studentEmail, currentQuestionNum))
                {
                    if (service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) > 0 && service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) > 0)
                    {
                        this.PuntuacionPregunta.Content = service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) + sobre + service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) + " pts";
                    }
                    else
                    {
                        this.PuntuacionPregunta.Content = 0;
                    }
                }
                else
                {
                    if (service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) >0 && service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) > 0)
                    {
                        this.PuntuacionPregunta.Content = (-1 / 3.0) * service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) + sobre + service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, pregId) + " pts";
                    }
                    else
                    {
                        this.PuntuacionPregunta.Content = 0;
                    }
                }
                QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(pregId, service.GetRespuestaPreguntaIdFromLastRealizacion(quizId, studentEmail, currentQuestionNum), true, service));
            }
            else if (service.GetQuestionType(pregId) == 3)
            {
            }

            if (currentQuestionNum == quizSize - 1)
            {
                NextButton.IsEnabled = false;
            }
            else
            {
                NextButton.IsEnabled = true;
            }
            if (currentQuestionNum == 0)
            {
                PreviousButton.IsEnabled = false;
            } 
            else
            {
                PreviousButton.IsEnabled = true;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["revResultadoTitulo"];
            TuNotaLabel.Content = Idioma.info["TuNotaLabel"];
            NumPregunta.Content = Idioma.info["NumPregunta"];
            pregPointsLabel.Content = Idioma.info["pregPointsLabel"];
            PreviousButton.Content = Idioma.info["PreviousButton"];
            NextButton.Content = Idioma.info["NextButton"];
            ExitButton.Content = Idioma.info["ExitButton"];
            preguntaString = Idioma.info["preguntaString"];
            of = Idioma.info["of"];
            sobre = Idioma.info["sobre"];
        }
    }
}
