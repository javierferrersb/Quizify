using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for BuscadorPreguntas.xaml
    /// </summary>
    public partial class VistaSeleccionPreguntas : Page
    {
        private IQuizifyService service;
        private ICollectionView availableQuestionsView;
        private List<PreguntaItem> availableQuestions;
        private string personEmail;
        private static string StatementCol = "Nombre";
        private static string TopicCol = "Contenido";
        private static string TypeCol = "Tipo de Pregunta";
        private static string CompetenciaCol = "Competencia";

        public VistaSeleccionPreguntas(IQuizifyService service)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            availableQuestions = new List<PreguntaItem>();
            availableQuestionsView = new ListCollectionView(availableQuestions);
            QuestionsDataGrid.CanUserAddRows = false;

            setQuestionToDataGrid();
            CreateAvailableQuestionsColumns();
        }

        private void setQuestionToDataGrid()
        {
            List<List<String>> list = service.GetAllQuestions();
            foreach (List<String> listOfQuestionInfo in list)
            {
                PreguntaItem item = (new PreguntaItem
                {
                    Id = int.Parse(listOfQuestionInfo.ElementAt(0)),
                    Enunciado = listOfQuestionInfo.ElementAt(1),
                    Quiz = listOfQuestionInfo.ElementAt(3),
                    Contenido = listOfQuestionInfo.ElementAt(4),
                    Type = listOfQuestionInfo.ElementAt(5),
                    Competencia = listOfQuestionInfo.ElementAt(6),
                    QuizId = int.Parse(listOfQuestionInfo.ElementAt(7)),
                });
                availableQuestions.Add(item);
            }
            availableQuestionsView.Filter += new Predicate<object>(QuestionFilter);

            availableQuestionsView.Refresh();

            QuestionsDataGrid.ItemsSource = availableQuestionsView;
        }
        private bool QuestionFilter(Object item)
        {
            return item is PreguntaItem obj &&
                obj.Contenido.ToLower().Contains(TopicTextBox.Text.ToLower()) &&
                obj.Enunciado.ToLower().Contains(StatementTextBox.Text.ToLower()) &&
                obj.Quiz.ToLower().Contains(QuizTextBox.Text.ToLower()) &&
                obj.Type.ToLower().Contains(TypeTextBox.Text.ToLower());
        }

        private void CreateAvailableQuestionsColumns()
        {
            QuestionsDataGrid.Columns.Clear();

            GridManager.AddGridColumn(QuestionsDataGrid, StatementCol, "Enunciado");
            GridManager.AddGridColumn(QuestionsDataGrid, "Quiz", "Quiz");
            GridManager.AddGridColumn(QuestionsDataGrid, TopicCol, "Contenido");
            GridManager.AddGridColumn(QuestionsDataGrid, TypeCol, "Type");
            GridManager.AddGridColumn(QuestionsDataGrid, CompetenciaCol, "Competencia");

            QuestionsDataGrid.MouseDoubleClick += QuestionsDataGrid_MouseDoubleClick;
        }


        private void ClearStatementButton_Click(object sender, RoutedEventArgs e)
        {
            StatementTextBox.Clear();
        }


        private void Filters_TextChanged(object sender, TextChangedEventArgs e)
        {
            availableQuestionsView.Refresh();
        }

        private void actualizar()
        {
            //Idioma.Controles(this);
            //this.Title = Idioma.info["SelectPregTitulo"];
            this.BuscarPreguntaTextBlock.Text = Idioma.info["BuscarPreguntaTextBlock"];
            this.ClearStatementButton.Content = Idioma.info["borrar2"];
            this.ClearTypeButton.Content = Idioma.info["borrar2"];
            this.ClearTopicButton.Content = Idioma.info["borrar2"];
            this.ClearQuizButton.Content = Idioma.info["borrar2"];
            HintAssist.SetHint(StatementTextBox, Idioma.info["statementTextBox"]);
            HintAssist.SetHint(TopicTextBox, Idioma.info["topicTextBox"]);
            HintAssist.SetHint(TypeTextBox, Idioma.info["typeTextBox"]);
            this.AvailableQuestionsTextBox.Text = Idioma.info["AvailableQuestionsTextBox"];

            StatementCol = Idioma.info["Enunciado"];
            TopicCol = Idioma.info["Contenido"];
            TypeCol = Idioma.info["Type"];
            CompetenciaCol = Idioma.info["Competencia"];

        }

        private void QuestionsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            PreguntaItem selectedQuestion = ((PreguntaItem)QuestionsDataGrid.SelectedItem);

            ((VistaPrincipal)this.Tag as VistaPrincipal).HandleQuestionDoubleClick(selectedQuestion);
        }

        private void ClearQuizButton_Click(object sender, RoutedEventArgs e)
        {
            QuizTextBox.Clear();
        }

        private void ClearTopicButton_Click(object sender, RoutedEventArgs e)
        {
            TopicTextBox.Clear();
        }

        private void ClearTypeButton_Click(object sender, RoutedEventArgs e)
        {
            TypeTextBox.Clear();
        }

        private void RefreshListButton_Click(object sender, RoutedEventArgs e)
        {
            availableQuestions.Clear();
            setQuestionToDataGrid();
        }
    }
    public struct PreguntaItem
    {
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public string Quiz { get; set; }
        public int QuizId { get; set; }
        public string Contenido { get; set; }
        public string Type { get; set; }
        public string Competencia { get; set; }
    }

}