using Quizify.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for BuscadorPreguntas.xaml
    /// </summary>
    public partial class SeleccionarPregunta : Window
    {
        private IQuizifyService service;
        private ICollectionView availableQuestionsView;
        private List<Question> availableQuestions;
        private static string StatementCol = "Enunciado";
        private static string TypeCol = "Tipo de Pregunta";
        private static string AuthorCol = "Autor";

        public SeleccionarPregunta(IQuizifyService service)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            availableQuestions = new List<Question>();
            availableQuestionsView = new ListCollectionView(availableQuestions);
            QuestionsDataGrid.CanUserAddRows = false;
            CreateAvailableQuestionsColumns(QuestionsDataGrid);
            setQuestionToDataGrid();
        }

        private void setQuestionToDataGrid()
        {
            List<Question> items = new List<Question>();
            List<List<String>> list = service.GetAllQuestions();
            foreach (List<String> listOfQuestionInfo in list)
            {
                Question item = (new Question
                {
                    Id = int.Parse(listOfQuestionInfo.ElementAt(0)),
                    Enunciado = listOfQuestionInfo.ElementAt(1),
                    Autor = listOfQuestionInfo.ElementAt(2),
                    Type = listOfQuestionInfo.ElementAt(5)
                });
                availableQuestions.Add(item);
            }
            availableQuestionsView.Filter += new Predicate<object>(QuestionFilter);

            availableQuestionsView.Refresh();

            QuestionsDataGrid.ItemsSource = availableQuestionsView;
        }


        private static void CreateAvailableQuestionsColumns(DataGrid datagrid)
        {
            datagrid.Columns.Clear();
            DataGridTextColumn enunciadoCol = new();
            DataGridTextColumn authorCol = new();
            DataGridTextColumn typeCol = new();

            enunciadoCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            authorCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            typeCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            enunciadoCol.Header = StatementCol;
            authorCol.Header = AuthorCol;
            typeCol.Header = TypeCol;

            enunciadoCol.IsReadOnly = true;
            authorCol.IsReadOnly = true;
            typeCol.IsReadOnly = true;

            enunciadoCol.Binding = new Binding("Enunciado");
            authorCol.Binding = new Binding("Autor");
            typeCol.Binding = new Binding("Type");

            // QuizzesTable.Columns.Add(idCol);
            datagrid.Columns.Add(enunciadoCol);
            datagrid.Columns.Add(authorCol);
            datagrid.Columns.Add(typeCol);
        }

        private bool QuestionFilter(Object item)
        {
            return item is Question obj &&
                obj.Enunciado.ToLower().Contains(StatementTextBox.Text.ToLower()) &&
                obj.Autor.ToLower().Contains(AuthorTextBox.Text.ToLower()) &&
                obj.Type.ToLower().Contains(TypeTextBox.Text.ToLower());
        }

        private void ClearAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorTextBox.Clear();
        }

        private void ClearStatementButton_Click(object sender, RoutedEventArgs e)
        {
            StatementTextBox.Clear();
        }

        private void ClearTypeButton_Click(object sender, RoutedEventArgs e)
        {
            TypeTextBox.Clear();
        }

        private void Filters_TextChanged(object sender, TextChangedEventArgs e)
        {
            availableQuestionsView.Refresh();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void actualizar()
        {
            this.Title = Idioma.info["FindPregTitulo"];
            selectTextQ.Text = Idioma.info["selectTextQ"];
            ClearStatementButton.Content = Idioma.info["ClearStatementButton"];
            ClearTypeButton.Content = Idioma.info["ClearAuthorButton"];
            ClearAuthorButton.Content = Idioma.info["ClearAuthorButton"];
            AvailableQuestionsTextBox.Text = Idioma.info["AvailableQuestionsTextBox"];
            CloseButton.Content = Idioma.info["CloseButton"];
            MaterialDesignThemes.Wpf.HintAssist.SetHint(StatementTextBox, Idioma.info["statementTextBox"]);
            MaterialDesignThemes.Wpf.HintAssist.SetHint(TypeTextBox, Idioma.info["typeTextBox"]);
            MaterialDesignThemes.Wpf.HintAssist.SetHint(AuthorTextBox, Idioma.info["AuthorTextBox"]);

            StatementCol = Idioma.info["Enunciado"];
            AuthorCol = Idioma.info["Autor"];
            TypeCol = Idioma.info["Type"];
        }

        private void QuestionsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            Question selectedQuestion = ((Question)QuestionsDataGrid.SelectedItem);

            Close();
        }

        
    }
    public class Question
    {
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public string Autor { get; set; }
        public string Type { get; set; }
    }

}