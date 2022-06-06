using Quizify.Services;
using QuizifyUI.CorrecionQuizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for VistaSeleccionQuiz.xaml
    /// </summary>
    public partial class VistaSeleccionQuiz : Page
    {
        private int topicId;
        private string personEmail;
        private int courseId;
        private IQuizifyService service;
        private string quizNameCol = "Nombre";
        private string quizDateCreateCol = "Fecha de Creación";
        private string quizInitDateCol = "Fecha de Inicio";
        private string quizClosingDateCol = "Fecha de Cierre";
        private string quizNumAttemptsCol = "Número de Intentos";
        private string correctedQuizCol = "Corregido?";
        public VistaSeleccionQuiz(IQuizifyService service, int topicId, int courseId, string personEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.topicId = topicId;
            this.courseId = courseId;
            this.personEmail = personEmail;

            if (PersonIsStudent())
            {
                HideInstructorFunctionality();
                ViewPreviousQuizesButton.Visibility = Visibility.Visible;
                LoadPreviousQuizes();
            }

            SetTextPath();
            ShowQuizzes();
        }

        private void LoadPreviousQuizes()
        {
            NombreContenidoPrevious.Text = service.GetCourseName(courseId) + " > " + service.GetTopicName(topicId) + " > Quizzes realizados";

            LoadPreviousQuizesColumns();

            List<List<string>> previousQuizesData = service.GetPreviousQuizes(personEmail, topicId);

            foreach (List<string> listofQuizInfo in previousQuizesData)
            {
                PreviousQuizItem item = (new PreviousQuizItem
                {
                    Id = int.Parse(listofQuizInfo.ElementAt(0)),
                    Nombre = listofQuizInfo.ElementAt(1),
                    Corregido = listofQuizInfo.ElementAt(2) == "1" ? "Si" : "No",
                    CorregidoBool = listofQuizInfo.ElementAt(2) == "1",
                });
                PreviousQuizzesTable.Items.Add(item);
            }
        }

        private void LoadPreviousQuizesColumns()
        {
            PreviousQuizzesTable.Columns.Clear();
            PreviousQuizzesTable.Items.Clear();

            GridManager.AddGridColumn(PreviousQuizzesTable, quizNameCol, "Nombre");
            GridManager.AddGridColumn(PreviousQuizzesTable, correctedQuizCol, "Corregido");
        }

        private void HideInstructorFunctionality()
        {
            AddQuizButton.Visibility = Visibility.Hidden;
        }

        private bool PersonIsStudent()
        {
            return service.IsPersonAStudent(personEmail);
        }

        private void ShowQuizzes()
        {
            List<List<String>> list = new();
            service.GetQuizzesFromTopic(topicId, list);

            CreateQuizesColumns();

            foreach (List<String> listofQuizInfo in list)
            {
                QuizItem item = (new QuizItem
                {
                    Id = int.Parse(listofQuizInfo.ElementAt(0)),
                    Name = listofQuizInfo.ElementAt(1),
                    CreationDate = listofQuizInfo.ElementAt(2),
                    Author = listofQuizInfo.ElementAt(3),
                    StartDate = listofQuizInfo.ElementAt(4),
                    EndDate = listofQuizInfo.ElementAt(5),
                    NumberOfTries = listofQuizInfo.ElementAt(6),
                    CourseId = int.Parse(listofQuizInfo.ElementAt(7)),
                    Status = int.Parse(listofQuizInfo.ElementAt(8))
                });
                if (!PersonIsStudent() || QuizIsActive(item))
                {
                    QuizzesTable.Items.Add(item);
                }
            }
        }

        private bool QuizIsActive(QuizItem item)
        {
            return item.Status == 2;
        }

        private void CreateQuizesColumns()
        {
            QuizzesTable.Columns.Clear();
            QuizzesTable.Items.Clear();

            GridManager.AddGridColumn(QuizzesTable, quizNameCol, "Name");
            GridManager.AddGridColumn(QuizzesTable, quizDateCreateCol, "CreationDate");
            GridManager.AddGridColumn(QuizzesTable, quizInitDateCol, "StartDate");
            GridManager.AddGridColumn(QuizzesTable, quizClosingDateCol, "EndDate");
            GridManager.AddGridColumn(QuizzesTable, quizNumAttemptsCol, "NumberOfTries");
        }

        public void SetTextPath()
        {
            NombreContenido.Text = service.GetCourseName(courseId) + " > " + service.GetTopicName(topicId);
        }

        private void QuizzesTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            QuizItem selectedQuizId = ((QuizItem)contentSelected.Item);

            ((VistaPrincipal)this.Tag as VistaPrincipal).HandleQuizDoubleQuick(selectedQuizId);
        }

        private void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            ((VistaPrincipal)this.Tag as VistaPrincipal).ShowCreateQuizDialog(topicId);
        }

        private void GoUpButton_Click(object sender, RoutedEventArgs e)
        {
            ((VistaPrincipal)this.Tag).GoUpCurrentTab(courseId);
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            TextAddQuizButton.Text = Idioma.info["TextAddQuizButton"];
            quizNameCol = Idioma.info["quizNameCol"];
            quizDateCreateCol = Idioma.info["quizDateCreateCol"];
            quizInitDateCol = Idioma.info["quizInitDateCol"];
            quizClosingDateCol = Idioma.info["quizClosingDateCol"];
            quizNumAttemptsCol = Idioma.info["quizNumAttemptsCol"];
        }

        private void ViewPreviousQuizesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoBackToQuizzesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private class PreviousQuizItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Corregido { get; set; }
            public bool CorregidoBool { get; set; }
        }

        private void PreviousQuizzesTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            PreviousQuizItem selectedQuizId = ((PreviousQuizItem)contentSelected.Item);

            CorregirQuiz window = new CorregirQuiz(service, selectedQuizId.Id, selectedQuizId.CorregidoBool);
            window.ShowDialog();
        }
    }
}
