using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizifyUI.CorrecionQuizes
{
    /// <summary>
    /// Interaction logic for CorregirQuiz.xaml
    /// </summary>
    public partial class CorregirQuiz : Window
    {
        private int quizId;
        private static readonly Regex _numberRegex = new Regex("[^0-9]+"); //regex that matches only integer numbers
        private IQuizifyService service;
        private List<RealizacionItem> listaOriginal;
        private ICollectionView lista;
        private int currentQuestion;
        private bool IsEditingMode;
        private bool IsStudentMode;
        private string StudentEmail;

        public CorregirQuiz(IQuizifyService service, int quizId)
        {
            InitializeComponent();
            this.service = service;
            this.quizId = quizId;

            IsStudentMode = false;

            currentQuestion = 0;
            QuizNameTextBlock.Text = service.GetQuizName(quizId);
            this.Title = service.GetQuizName(quizId);
            SetListItems();
            SetQuizMode();


            LoadData();

            if (IsEditingMode)
            {
                CheckIfAllQuizzesAreDone();
            }

            UpdateLanguage();
        }
        public CorregirQuiz(IQuizifyService service, int realizaQuizId, bool isCorrected)
        {
            InitializeComponent();
            this.service = service;
            this.quizId = service.GetQuizIdFromRealizaQuizId(realizaQuizId);
            this.StudentEmail = service.GetStudentFromRealizaQuiz(realizaQuizId);

            IsStudentMode = true;

            currentQuestion = 0;
            QuizNameTextBlock.Text = service.GetQuizName(quizId);

            SetListItems();
            SetStudentMode();

            LoadStudentData();
            UpdateLanguage();
        }

        private void LoadStudentData()
        {
            listaOriginal.Clear();

            List<List<string>> realizationsData = service.GetRealizationsFromQuiz(quizId, false).Where(x => x[1] == StudentEmail).ToList();

            foreach (List<string> item in realizationsData)
            {
                RealizacionItem realizacionItem = new()
                {
                    Id = int.Parse(item[0]),
                    StudentEmail = item[1],
                    StudentLetter = (int.Parse(item[3]) + 1).ToString(),
                    StudentName = Idioma.info["RealizacionNumero"] + (int.Parse(item[3]) + 1),
                    CurrentyAnswering = item[4] == "0",
                };
                if (realizacionItem.CurrentyAnswering)
                {
                    realizacionItem.Description = Idioma.info["CurrentlyAnsweringRealizacion"];
                }
                List<List<string>> questionsData = service.GetRespuestasPreguntasFromRealizacion(realizacionItem.Id);
                List<RespuestaPreguntaItem> questions = new List<RespuestaPreguntaItem>();
                double Grade = 0.0;
                foreach (List<string> question in questionsData)
                {
                    RespuestaPreguntaItem respuestaPreguntaItem = (new RespuestaPreguntaItem()
                    {
                        Id = int.Parse(question[0]),
                        QuestionId = int.Parse(question[1]),
                        Statement = question[2],
                        QuestionType = int.Parse(question[3]),
                        Points = service.GetPointsFromRealizationQuestion(int.Parse(question[0]))
                    });
                    questions.Add(respuestaPreguntaItem);
                    if (respuestaPreguntaItem.Points.HasValue) { Grade += respuestaPreguntaItem.Points.Value; }
                }
                if (IsEditingMode)
                {
                    if (Grade.ToString().Count() > 7)
                    {
                        realizacionItem.Description = Idioma.info["GradeTextNumber"] + Grade.ToString().Substring(0, 7);
                    }
                    else
                    {
                        realizacionItem.Description = Idioma.info["GradeTextNumber"] + Grade.ToString();
                    }
                }
                else
                {
                    realizacionItem.Description = Idioma.info["NotGradedText"];
                }
                realizacionItem.IsGraded = false;
                realizacionItem.Questions = questions;
                listaOriginal.Add(realizacionItem);
            }
            lista.Refresh();
        }

        private void SetListItems()
        {
            listaOriginal = new List<RealizacionItem>();
            lista = new ListCollectionView(listaOriginal);
            RealizationsListView.ItemsSource = lista;
        }
        private void SetStudentMode()
        {
            if (service.GetQuizStatus(quizId) == 4)
            {
                IsEditingMode = true;
            }
            UpdateButton.Visibility = Visibility.Hidden;
            RealizationCurrentlyIndicator.Visibility = Visibility.Hidden;
            ViewingModeIndicator.Visibility = Visibility.Hidden;
            SavedIndicator.Visibility = Visibility.Hidden;
            NotSavedIndicator.Visibility = Visibility.Hidden;
            RefreshRealziationsButton.Visibility = Visibility.Hidden;
            MarkQuizButton.Visibility = Visibility.Hidden;
        }

        private void SetQuizMode()
        {
            int quizStatus = service.GetQuizStatus(quizId);
            switch (quizStatus)
            {
                case 2:
                    SetViewingMode();
                    break;
                case 3:
                    SetCorrectionMode();
                    break;
                case 4:
                    SetViewingMode();
                    break;
            }

        }

        private void LoadQuestion()
        {
            RespuestaPreguntaItem currentRespuesta = listaOriginal[RealizationsListView.SelectedIndex].Questions[currentQuestion];

            if (!IsEditingMode)
            {
                if (!IsStudentMode)
                {
                    if (listaOriginal[RealizationsListView.SelectedIndex].CurrentyAnswering)
                    {
                        RealizationCurrentlyIndicator.Visibility = Visibility.Visible;
                        UpdateButton.Visibility = Visibility.Visible;
                        ViewingModeIndicator.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        RealizationCurrentlyIndicator.Visibility = Visibility.Hidden;
                        UpdateButton.Visibility = Visibility.Hidden;
                        ViewingModeIndicator.Visibility = Visibility.Visible;
                    }
                    QuestionmarkTextBox.TextChanged -= QuestionmarkTextBox_TextChanged;
                    QuestionmarkTextBox.Text = currentRespuesta.Points.ToString();
                    QuestionmarkTextBox.TextChanged += QuestionmarkTextBox_TextChanged;
                }
            }
            else
            {
                if (!IsStudentMode)
                {
                    SavedIndicator.Visibility = Visibility.Visible;
                    NotSavedIndicator.Visibility = Visibility.Hidden;
                    QuestionmarkTextBox.IsEnabled = true;
                }
                else
                {
                    SavedIndicator.Visibility = Visibility.Hidden;
                }
                QuestionmarkTextBox.TextChanged -= QuestionmarkTextBox_TextChanged;
                QuestionmarkTextBox.Text = currentRespuesta.Points.ToString();
                QuestionmarkTextBox.TextChanged += QuestionmarkTextBox_TextChanged;
            }
            TextFieldAssist.SetSuffixText(QuestionmarkTextBox, "/" + service.GetNotaDeNotaBateriaFromPreguntaAndQuiz(quizId, listaOriginal[RealizationsListView.SelectedIndex].Questions[currentQuestion].QuestionId));
            UpdateQuestionButtonVisibility();
            QuestionMarkCard.Visibility = Visibility.Visible;


            if (currentRespuesta.QuestionType == 0)
            {
                QuestionFrame.Navigate(new PreguntaAbiertaRevision(service, currentRespuesta.Id, currentRespuesta.Statement, service.GetKeyWordsFromOpenQuestion(currentRespuesta.QuestionId), IsStudentMode));
            }
            else if (currentRespuesta.QuestionType == 1)
            {
                QuestionFrame.Navigate(new PreguntaVFRevision(service, currentRespuesta.Id, currentRespuesta.Statement, service.GetVFCorrectAnswer(currentRespuesta.QuestionId), IsStudentMode && !IsEditingMode));
            }
            else if (currentRespuesta.QuestionType == 2)
            {
                QuestionFrame.Navigate(new PreguntaMultipleRevision(currentRespuesta.Id, currentRespuesta.Statement, service.GetSeleccionStudentAnswerRespuestaMultiple(currentRespuesta.Id, currentRespuesta.QuestionId),service.GetSeleccionCorrectAnswer(currentRespuesta.QuestionId), service.GetSeleccionTextOptions(currentRespuesta.QuestionId), IsStudentMode && !IsEditingMode));
            }
            if (!IsStudentMode) { CheckIfQuizIsDone(); }
        }

        private void SetCorrectionMode()
        {
            IsEditingMode = true;
            MarkQuizButton.Visibility = Visibility.Visible;
            MarkQuestionButton.Visibility = Visibility.Visible;
        }

        private void SetViewingMode()
        {
            IsEditingMode = false;
            QuestionmarkTextBox.IsEnabled = false;
            ViewingModeIndicator.Visibility = Visibility.Visible;
            MarkQuizButton.Visibility = Visibility.Hidden;
        }

        private void LoadData()
        {
            listaOriginal.Clear();

            List<List<string>> realizationsData;
            if (IsEditingMode)
            {
                realizationsData = service.GetRealizationsFromQuiz(quizId, true);
            }
            else
            {
                realizationsData = service.GetRealizationsFromQuiz(quizId, false);
            }

            foreach (List<string> item in realizationsData)
            {
                RealizacionItem realizacionItem = new()
                {
                    Id = int.Parse(item[0]),
                    StudentEmail = item[1],
                    StudentName = item[2],
                    StudentLetter = item[2].Substring(0, 1),
                    Description = Idioma.info["RealizacionNumero"] + (int.Parse(item[3]) + 1),
                    CurrentyAnswering = item[4] == "0",
                };
                if (realizacionItem.CurrentyAnswering)
                {
                    realizacionItem.Description = Idioma.info["CurrentlyAnsweringRealizacion"];
                }
                List<List<string>> questionsData = service.GetRespuestasPreguntasFromRealizacion(realizacionItem.Id);
                List<RespuestaPreguntaItem> questions = new List<RespuestaPreguntaItem>();
                bool IsGraded = true;
                foreach (List<string> question in questionsData)
                {
                    RespuestaPreguntaItem respuestaPreguntaItem = (new RespuestaPreguntaItem()
                    {
                        Id = int.Parse(question[0]),
                        QuestionId = int.Parse(question[1]),
                        Statement = question[2],
                        QuestionType = int.Parse(question[3]),
                        Points = service.GetPointsFromRealizationQuestion(int.Parse(question[0]))
                    });
                    if (respuestaPreguntaItem.Points == null) { IsGraded = false; }
                    questions.Add(respuestaPreguntaItem);
                }
                realizacionItem.IsGraded = IsGraded;
                realizacionItem.Questions = questions;
                listaOriginal.Add(realizacionItem);
            }

            lista.Refresh();
        }


        private void RealizationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadQuestion();
        }

        private static bool IsTextAllowed(string text, Regex regex)
        {
            return !regex.IsMatch(text);
        }

        private void QuestionNumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, _numberRegex);
        }

        private void QuestionmarkTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void PreviousQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestion--;
            LoadQuestion();
            UpdateQuestionButtonVisibility();
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestion++;
            LoadQuestion();
            UpdateQuestionButtonVisibility();
        }

        private void UpdateQuestionButtonVisibility()
        {
            if (currentQuestion == 0)
            {
                PreviousQuestionButton.IsEnabled = false;
            }
            else
            {
                PreviousQuestionButton.IsEnabled = true;
            }
            if (currentQuestion == listaOriginal[0].Questions.Count - 1)
            {
                NextQuestionButton.IsEnabled = false;
            }
            else
            {
                NextQuestionButton.IsEnabled = true;
            }
            QuestionNumberTextBox.Text = (currentQuestion + 1).ToString();
            QuestionNumberTextBox.IsEnabled = true;
        }

        private void QuestionmarkTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                MarkQuestionButton_Click(sender, e);
            }
        }

        private void QuestionNumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                if (QuestionNumberTextBox.Text != "" && int.Parse(QuestionNumberTextBox.Text) <= listaOriginal[0].Questions.Count
                    && int.Parse(QuestionNumberTextBox.Text) > 0)
                {
                    currentQuestion = int.Parse(QuestionNumberTextBox.Text) - 1;
                    UpdateQuestionButtonVisibility();
                    QuestionmarkTextBox.Focus();
                    QuestionmarkTextBox.SelectAll();
                    LoadQuestion();
                }
                else
                {
                    QuestionNumberTextBox.Text = (currentQuestion + 1).ToString();
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadQuestion();
        }

        private void RefreshRealziationsButton_Click(object sender, RoutedEventArgs e)
        {
            RealizationsListView.SelectionChanged -= RealizationsListView_SelectionChanged;
            LoadData();
            QuestionNumberTextBox.IsEnabled = false;
            NextQuestionButton.IsEnabled = false;
            PreviousQuestionButton.IsEnabled = false;
            QuestionFrame.Content = null;
            QuestionMarkCard.Visibility = Visibility.Hidden;
            RealizationsListView.SelectionChanged += RealizationsListView_SelectionChanged;
        }

        private void QuestionmarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SavedIndicator.Visibility = Visibility.Hidden;
            NotSavedIndicator.Visibility = Visibility.Visible;
            MarkQuestionButton.IsEnabled = true;
        }

        private void MarkQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            double grade;
            if (double.TryParse(QuestionmarkTextBox.Text, out grade))
            {
                if (grade > service.GetNotaDeNotaBateriaFromPreguntaAndQuiz(quizId, listaOriginal[RealizationsListView.SelectedIndex].Questions[currentQuestion].QuestionId))
                {
                    MessageBox.Show(Idioma.info["OverMaximumGradeMessage"]);
                }
                else
                {
                    service.SetPointsFromRealizationQuestion(listaOriginal[RealizationsListView.SelectedIndex].Questions[currentQuestion].Id, grade);
                    listaOriginal[RealizationsListView.SelectedIndex].Questions[currentQuestion].Points = grade;
                    SavedIndicator.Visibility = Visibility.Visible;
                    NotSavedIndicator.Visibility = Visibility.Hidden;
                    CheckIfQuizIsDone();
                }
            }
            else
            {
                MessageBox.Show(Idioma.info["InvalidMarkFormatMessage"]);
            }
        }

        private void CheckIfQuizIsDone()
        {
            List<RespuestaPreguntaItem> listToCheck = listaOriginal[RealizationsListView.SelectedIndex].Questions;
            bool isDone = true;
            foreach (RespuestaPreguntaItem item in listToCheck)
            {
                if (item.Points == null)
                {
                    isDone = false;
                }
            }
            if (isDone)
            {
                listaOriginal[RealizationsListView.SelectedIndex].IsGraded = true;
                lista.Refresh();
            }
            CheckIfAllQuizzesAreDone();
        }

        private void MarkQuizButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> realizacionId = new List<int>();
            foreach (RealizacionItem ri in listaOriginal)
            {
                realizacionId.Add(ri.Id);
            }
            service.SetQuizAsCorrected(quizId, realizacionId);
            this.Close();
        }
        private void CheckIfAllQuizzesAreDone()
        {
            bool isDone = true;
            foreach (RealizacionItem item in listaOriginal)
            {
                List<RespuestaPreguntaItem> listToCheck = item.Questions;
                foreach (RespuestaPreguntaItem item2 in listToCheck)
                {
                    if (item2.Points == null)
                    {
                        isDone = false;
                    }
                }
            }
            if (isDone == true)
            {
                MarkQuizButton.IsEnabled = true;
            }
        }
        private void UpdateLanguage()
        {
            RealizationsTextBlock.Text = Idioma.info["RealizationsColumnTextBlock"];
            QuestionTextBlock.Text = Idioma.info["QuestionHeaderTextBlock"];
            MarkQuizButton.Content = Idioma.info["MarkQuizToStudentsButton"];
            MarkTextBlock.Text = Idioma.info["MarkTextBlock"];
            SavedIndicatorText.Text = Idioma.info["SavedIndicatorText"];
            NotSavedIndicatorText.Text = Idioma.info["NotSavedIndicatorText"];
            RealizationCurrentlyIndicatorText.Text = Idioma.info["RealizationCurrentlyIndicatorText"];
            ViewingModeIndicatorText.Text = Idioma.info["ViewingModeIndicatorText"];
            MarkQuestionButton.Content = Idioma.info["MarkQuestionButton"];
        }
    }
    public class RealizacionItem
    {
        public int Id { get; set; }
        public string StudentEmail { get; set; }
        public string StudentLetter { get; set; }
        public string StudentName { get; set; }
        public string Description { get; set; }
        public bool IsGraded { get; set; }
        public bool CurrentyAnswering { get; set; }
        public List<RespuestaPreguntaItem> Questions { get; set; }
        public double Grade { get; set; }
    }
    public class RespuestaPreguntaItem
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int QuestionType { get; set; }
        public string Statement { get; set; }
        public double? Points { get; set; }
    }
}
