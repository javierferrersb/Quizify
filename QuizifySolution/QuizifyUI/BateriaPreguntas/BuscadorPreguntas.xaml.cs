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
    public partial class BuscadorPreguntas : Window
    {
        private IQuizifyService service;
        private int bateriaId;
        private ICollectionView availableQuestionsView;
        private ICollectionView addedQuestionsView;
        private string personEmail;
        private List<QuestionItem> availableQuestions;
        private List<QuestionItem> addedQuestions;
        
        public BuscadorPreguntas(IQuizifyService service, string personEmail, int bateriaId)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.bateriaId = bateriaId;
            this.personEmail = personEmail;

            availableQuestions = new List<QuestionItem>();
            addedQuestions = new List<QuestionItem>();
            addedQuestionsView = new ListCollectionView(addedQuestions);
            availableQuestionsView = new ListCollectionView(availableQuestions);


            FillAvailableQuestionsTable();
        }

        private void FillAvailableQuestionsTable()
        {
            List<List<String>> list = service.GetQuestionsFromInstructor(personEmail, service.GetQuestionTypeFromBateria(bateriaId));

            CreateAvailableQuestionsColumns(QuestionsDataGrid);
            CreateAvailableQuestionsColumns(AddedQuestionsDataGrid);
            QuestionsDataGrid.MouseDoubleClick += QuestionsDataGrid_MouseDoubleClick;
            AddedQuestionsDataGrid.MouseDoubleClick += AddedQuestionsDataGrid_MouseDoubleClick;


            foreach (List<String> listOfQuestionInfo in list)
            {
                QuestionItem item = (new QuestionItem
                {
                    Id = int.Parse(listOfQuestionInfo.ElementAt(0)),
                    Enunciado = listOfQuestionInfo.ElementAt(1),
                    Autor = listOfQuestionInfo.ElementAt(2),
                    Curso = listOfQuestionInfo.ElementAt(3),
                    Contenido = listOfQuestionInfo.ElementAt(4)
                });
                if (!service.IsQuestionInBateria(item.Id, bateriaId))
                {
                    availableQuestions.Add(item);
                }
                else
                {
                    addedQuestions.Add(item);
                }
            }
            availableQuestionsView.Filter += new Predicate<object>(QuestionFilter);

            addedQuestionsView.Refresh();
            availableQuestionsView.Refresh();

            QuestionsDataGrid.ItemsSource = availableQuestionsView;
            AddedQuestionsDataGrid.ItemsSource = addedQuestionsView;
        }
        private bool QuestionFilter(Object item)
        {
            return item is QuestionItem obj && 
                obj.Contenido.ToLower().Contains(TopicTextBox.Text.ToLower()) && 
                obj.Curso.ToLower().Contains(CourseTextBox.Text.ToLower()) && 
                obj.Autor.ToLower().Contains(AuthorTextBox.Text.ToLower()) && 
                obj.Enunciado.ToLower().Contains(StatementTextBox.Text.ToLower());
        }
        private static void CreateAvailableQuestionsColumns(DataGrid datagrid)
        {
            datagrid.Columns.Clear();
            datagrid.Items.Clear();
            System.Windows.Controls.DataGridTextColumn enunciadoCol = new();
            System.Windows.Controls.DataGridTextColumn authorCol = new();
            System.Windows.Controls.DataGridTextColumn courseCol = new();
            System.Windows.Controls.DataGridTextColumn contenidoCol = new();

            enunciadoCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            authorCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            courseCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            contenidoCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            enunciadoCol.Header = Idioma.info["enunciadoColHeader"];
            authorCol.Header = Idioma.info["authorColHeader"];
            courseCol.Header = Idioma.info["courseColHeader"];
            contenidoCol.Header = Idioma.info["contenidoColHeader"];

            enunciadoCol.IsReadOnly = true;
            authorCol.IsReadOnly = true;
            courseCol.IsReadOnly = true;
            contenidoCol.IsReadOnly = true;

            enunciadoCol.Binding = new Binding("Enunciado");
            authorCol.Binding = new Binding("Autor");
            courseCol.Binding = new Binding("Curso");
            contenidoCol.Binding = new Binding("Contenido");

            datagrid.Columns.Add(enunciadoCol); 
            datagrid.Columns.Add(courseCol);
            datagrid.Columns.Add(contenidoCol);
            datagrid.Columns.Add(authorCol);
        }

        private void QuestionsDataGrid_MouseDoubleClick(Object sender, RoutedEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            QuestionItem selectedQuestion = ((QuestionItem)contentSelected.Item);

            MoveQuestionToBattery(selectedQuestion);
        }

        private void MoveQuestionToBattery(QuestionItem questionItem)
        {
            service.InsertQuestionToBattery(bateriaId, questionItem.Id);
            addedQuestions.Add(questionItem);
            availableQuestions.Remove(questionItem);

            addedQuestionsView.Refresh();
            availableQuestionsView.Refresh();
        }

        private void RemoveQuestionFromBattery(QuestionItem questionItem)
        {
            service.RemoveQuestionFromBattery(bateriaId, questionItem.Id);
            addedQuestions.Remove(questionItem);
            availableQuestions.Add(questionItem);

            addedQuestionsView.Refresh();
            availableQuestionsView.Refresh();
        }

        private void AddedQuestionsDataGrid_MouseDoubleClick(Object sender, RoutedEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            QuestionItem selectedQuestion = ((QuestionItem)contentSelected.Item);

            RemoveQuestionFromBattery(selectedQuestion);
        }

        private void ClearCourseButton_Click(object sender, RoutedEventArgs e)
        {
            CourseTextBox.Clear();
        }

        private void ClearTopicButton_Click(object sender, RoutedEventArgs e)
        {
            TopicTextBox.Clear();
        }

        private void ClearAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorTextBox.Clear();
        }

        private void ClearStatementButton_Click(object sender, RoutedEventArgs e)
        {
            StatementTextBox.Clear();
        }

        private void QuestionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionsDataGrid.SelectedItems.Count > 0)
            {
                AddQuestionButton.IsEnabled = true;
                AddedQuestionsDataGrid.SelectedIndex = -1;
            }
            else
            {
                AddQuestionButton.IsEnabled = false;
            }
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionItem item = (QuestionItem)QuestionsDataGrid.SelectedItem;

            MoveQuestionToBattery(item);
        }

        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionItem item = (QuestionItem)AddedQuestionsDataGrid.SelectedItem;

            RemoveQuestionFromBattery(item);
        }

        private void AddedQuestionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddedQuestionsDataGrid.SelectedItems.Count > 0)
            {
                DeleteQuestionButton.IsEnabled = true;
                QuestionsDataGrid.SelectedIndex = -1;
            }
            else
            {
                DeleteQuestionButton.IsEnabled = false;
            }
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
            //Idioma.Controles(this);
            this.Title = Idioma.info["FindPregTitulo"];
            selectTextQ.Text = Idioma.info["selectTextQ"];
            ClearStatementButton.Content = Idioma.info["ClearStatementButton"];
            ClearCourseButton.Content = Idioma.info["ClearCourseButton"];
            ClearTopicButton.Content = Idioma.info["ClearTopicButton"];
            ClearAuthorButton.Content = Idioma.info["ClearAuthorButton"];
            AvailableQuestionsTextBox.Text = Idioma.info["AvailableQuestionsTextBox"];
            AddedQuestionsTextBox.Text = Idioma.info["AddedQuestionsTextBox"];
            AddQuestionButton.Content = Idioma.info["AddQuestionButton"];
            DeleteQuestionButton.Content = Idioma.info["DeleteQuestionButton"];
            CloseButton.Content = Idioma.info["CloseButton"];
            HintAssist.SetHint(StatementTextBox, Idioma.info["StatementTextBox"]);
            HintAssist.SetHint(CourseTextBox, Idioma.info["CourseTextBox"]);
            HintAssist.SetHint(TopicTextBox, Idioma.info["TopicTextBox"]);
            HintAssist.SetHint(AuthorTextBox, Idioma.info["AuthorTextBox"]);
        }
    }

}
