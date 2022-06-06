using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for HacerQuiz.xaml
    /// </summary>
    public partial class HacerQuiz : Window
    {

        private System.Timers.Timer timeClock, endExamClock;
        private List<QuestionItem> questions;
        private int quizId;
        private string studentEmail;
        private int currentQuestionNum;
        private int AnswerMode;
        private IQuizifyService service;
        private int realizaQuizId;
        private int currentRealizacionId;
        private int secondsPassed;
        private string preguntaString = "Pregunta ";
        private string of = " de ";
        private string maxMarkString = "      Nota máxima: ";
        private string pesoString = "Peso: ";
        private string puntosString = " puntos";
        public HacerQuiz(IQuizifyService service, int quizId, string personEmail)
        {
            InitializeComponent();
            actualizar();
            questions = new List<QuestionItem>();
            this.service = service;
            this.quizId = quizId;
            this.studentEmail = personEmail;

            SetupStorage();
            AnswerMode = service.GetQuizAnswerMode(quizId);
            QuizName.Text = service.GetQuizName(quizId);
            this.Title = service.GetQuizName(quizId);
            LoadCurrentQuestion();

            SetExpanderContent();

            if (service.GetQuizTime(quizId) != -1)
            {
                secondsPassed = service.GetRemainingTimeRealizacion(currentRealizacionId);

                timeClock = new System.Timers.Timer();
                double interval = 1000;
                timeClock.Interval = interval;
                timeClock.Elapsed += TimePassed;
                timeClock.AutoReset = true;
                timeClock.Enabled = true;

                TimeProgressBar.Maximum = 100;
            }
            else
            {
                TimeProgressBar.Visibility = Visibility.Hidden;
                ShowHideButton.Visibility = Visibility.Hidden;
            }
        }
        private void SetExpanderContent()
        {
            String expanderContent = "";
            int numPregunta = 1;
            foreach (QuestionItem question in questions)
            {
                expanderContent += numPregunta + ". " + question.Puntos + " pts\n";
                numPregunta++;
            }
            QuestionPointsTextBlock.Text = expanderContent;
        }
        private void SetupStorage()
        {
            realizaQuizId = service.GetRealizaQuizIdFromQuizAndStudent(quizId, studentEmail);
            if (realizaQuizId != -1)
            {
                if (IsStudentResumingQuiz(quizId))
                {
                    SetRealizacion(service.GetUnfinishedRealizacionId(quizId, studentEmail));
                }
                else
                {
                    if (service.GetQuizNumberOfAttempts(quizId) != -1)
                    {
                        if (service.GetStudentRealizationsCount(studentEmail, quizId) < service.GetQuizNumberOfAttempts(quizId))
                        {
                            CreateAndSetRealizacion();
                        }
                        else
                        {
                            CloseWindow();
                            return;
                        }
                    }
                    else
                    {
                        CreateAndSetRealizacion();
                    }
                }
            }
            else
            {
                if (service.GetQuizNumberOfAttempts(quizId) == -1 || service.GetQuizNumberOfAttempts(quizId) > 0)
                {
                    realizaQuizId = service.CreateRealizaQuiz(quizId, studentEmail);
                    CreateAndSetRealizacion();
                }
                else
                {
                    CloseWindow();
                    return;
                }
            }
            LoadQuestionsData();
        }

        private void LoadQuestionsData()
        {
            List<List<String>> list = service.GetQuestionsFromRealizacion(currentRealizacionId);

            questions.Clear();

            foreach (List<String> item in list)
            {
                questions.Add(new QuestionItem
                {
                    Id = int.Parse(item[0]),
                    Enunciado = item[1],
                    Type = int.Parse(item[3]),
                    Puntos = double.Parse(item[4])
                });
            }
        }
        private bool IsStudentResumingQuiz(int quizId)
        {
            return service.StudentHasUnfinishedAnsweredTest(studentEmail, quizId);
        }

        private void SetRealizacion(int realizacionId)
        {
            currentRealizacionId = realizacionId;
            currentQuestionNum = service.GetRealizacionCurrentQuestionNum(currentRealizacionId);
        }

        private void CreateAndSetRealizacion()
        {
            if (service.QuizHasRandomQuestions(quizId))
            {
                currentRealizacionId = service.CreateRealizacionRandomOrder(realizaQuizId);
            }
            else
            {
                currentRealizacionId = service.CreateRealizacionInOrder(realizaQuizId);
            }
            currentQuestionNum = service.GetRealizacionCurrentQuestionNum(currentRealizacionId);
        }

        private void CloseWindow()
        {
            Close();
        }

        private void TimePassed(Object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                TimeLeftTextBlock.Text = secondsPassed + "";
                secondsPassed--;
                TimeProgressBar.Value = 100.0 * secondsPassed / (service.GetQuizTime(quizId) * 60);
                if (secondsPassed == -1)
                {
                    timeClock.Enabled = false;
                    timeClock.Close();
                    EndExamTimer();
                }
            }));
        }

        private void EndExam()
        {
            service.MarkRealizacionAsFinished(currentRealizacionId);
            CloseWindow();
        }

        private void EndExamTimer()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                timeClock.Enabled = false;
                timeClock.Close();
                this.Close();
            }));
            EndExam();
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
            QuestionItem questionToOpen = questions.ElementAt(currentQuestionNum);

            if (questionToOpen.Type == 0)
            {
                QuestionFrame.Navigate(new PreguntaAbiertaPage(service, questionToOpen.Id, currentRealizacionId, studentEmail));
            }
            else if (questionToOpen.Type == 1)
            {
                QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(service, questionToOpen.Id, currentRealizacionId, studentEmail));
            }
            else if (questionToOpen.Type == 2)
            {
                QuestionFrame.Navigate(new PreguntaSeleccionMultiplePage(service, questionToOpen.Id, currentRealizacionId, studentEmail));
            }
            else if (questionToOpen.Type == 3)
            {
                QuestionFrame.Navigate(new PreguntaCorrespondenciaPage(service, questionToOpen.Id, currentRealizacionId, studentEmail));
            }

            service.SetRealizacionCurrentQuestionNum(currentRealizacionId, currentQuestionNum);

            QuestionNum.Text = preguntaString + (currentQuestionNum + 1) + of + questions.Count + maxMarkString + service.GetValorQuiz(quizId);
            QuestionStats.Text = pesoString + service.GetPesoNotaBateriaFromPreguntaAndQuiz(quizId, questionToOpen.Id) + puntosString;

            UpdateButtonVisibility();
        }


        private void UpdateButtonVisibility()
        {
            if (currentQuestionNum == questions.Count - 1)
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
                if (AnswerMode == 0)
                {
                    PreviousButton.IsEnabled = true;
                }
            }
        }

        private void EndQuizButton_Click(object sender, RoutedEventArgs e)
        {
            service.MarkRealizacionAsFinished(currentRealizacionId);
            ObtenerPuntuacion();
            if (service.GetQuizTime(quizId) == -1)
            {
                EndExam();
            }
            else
            {
                EndExamTimer();
            }
        }

        private void ObtenerPuntuacion()
        {
            double puntos = service.GetPointsFromQuizRealizacion(currentRealizacionId);
            service.SetPointsToQuizRealizacion(currentRealizacionId, puntos);
        }

        private void PauseQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeProgressBar.IsVisible)
            {
                service.SetRemainingTimeRealizacion(currentRealizacionId, secondsPassed);
                timeClock.Enabled = false;
                timeClock.Close();
            }

            Close();
        }

        private void ShowHideButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeProgressBar.IsVisible)
            {
                TimeProgressBar.Visibility = Visibility.Hidden;
                TimeLeftTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                TimeProgressBar.Visibility = Visibility.Visible;
                TimeLeftTextBlock.Visibility = Visibility.Visible;
            }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["HacerQuizTitulo"];
            QuestionNum.Text = Idioma.info["QuestionNum"];
            ShowHideButton.Content = Idioma.info["ShowHideButton"];
            PreviousButton.Content = Idioma.info["PreviousButton"];
            NextButton.Content = Idioma.info["NextButton"];
            PauseQuizButton.Content = Idioma.info["PauseQuizButton"];
            EndQuizButton.Content = Idioma.info["EndQuizButton"];
            Question.Text = Idioma.info["Question"];
            Question2.Text = Idioma.info["Question"];
            preguntaString = Idioma.info["preguntaString"];
            of = Idioma.info["of"];
            maxMarkString = Idioma.info["maxMarkString"];
            pesoString = Idioma.info["pesoString"];
            puntosString = Idioma.info["puntosString"];
            InfoFinalizacion.Text = Idioma.info["confirmacionString"];
            AcceptLaunchButton.Content = Idioma.info["AcceptLaunchButton"];
        }
    }
}
