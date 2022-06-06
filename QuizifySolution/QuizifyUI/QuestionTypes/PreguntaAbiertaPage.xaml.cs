using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for PreguntaAbierta.xaml
    /// </summary>
    public partial class PreguntaAbiertaPage : Page
    {
        private IQuizifyService service;
        private int questionId;
        private string studentEmail;
        private int currentRealizacionId;
        private List<KeyWordItem> listaOriginal;
        private ICollectionView lista;

        public PreguntaAbiertaPage(IQuizifyService service, int questionId)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.questionId = questionId;
            QuestionStatement.Text = service.GetQuestionStatement(questionId);

            studentEmail = "";
            ShowEditFunctionality();
        }

        public PreguntaAbiertaPage(IQuizifyService service, int questionId, int currentRealizacionId, string studentEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.questionId = questionId;
            this.currentRealizacionId = currentRealizacionId;
            AnswerField.Text = service.GetOpenStudentAnswer(currentRealizacionId, questionId);
            this.studentEmail = studentEmail;
            AnswerField.TextChanged += AnswerField_TextChanged;
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
        }
        public PreguntaAbiertaPage(int preguntaId, int respuestaPreguntaId, bool correccion, IQuizifyService ser)
        {
            InitializeComponent();
            actualizar();
            this.service = ser;
            this.questionId = preguntaId;

            if (service.GetOpenAnswerRespuestaPreguntaAbierta(respuestaPreguntaId) != null)
            {
                AnswerField.Text = service.GetOpenAnswerRespuestaPreguntaAbierta(respuestaPreguntaId);
            }
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            if (correccion)
            {
                AnswerField.IsEnabled = false;
            }
            else
            {
                AnswerField.TextChanged += AnswerField_TextChanged;
            }
        }
        public PreguntaAbiertaPage(int preguntaId, int respuestaPreguntaId, IQuizifyService ser, bool permitirEdicion)
        {
            InitializeComponent();
            actualizar();
            this.service = ser;
            this.questionId = preguntaId;
            if (service.GetOpenAnswerRespuestaPreguntaAbierta(respuestaPreguntaId) != null)
            {
                AnswerField.Text = service.GetOpenAnswerRespuestaPreguntaAbierta(respuestaPreguntaId);
            }
            AnswerField.TextChanged += AnswerField_TextChanged;
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            AnswerField.IsEnabled = false;
        }

        private void AnswerField_TextChanged(object sender, TextChangedEventArgs e)
        {
            service.SetOpenStudentQuestionAnswer(currentRealizacionId, questionId, AnswerField.Text);
        }

        private void EditStatementHandler(object sender, RoutedEventArgs e)
        {
            EditStatementField.Text = service.GetQuestionStatement(questionId);
            ShowEditStatementField();
            EditStatementField.Focus();
        }

        private void SaveStatementHandler(object sender, RoutedEventArgs e)
        {
            service.SetQuestionStatement(questionId, EditStatementField.Text);
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            HideEditStatementField();
        }

        private void ShowEditStatementField()
        {
            EditStatementField.IsEnabled = true;
            EditStatementField.Visibility = Visibility.Visible;

            EditStatementButton.IsEnabled = false;
            EditStatementButton.Visibility = Visibility.Hidden;

            SaveStatementButton.Visibility = Visibility.Visible;
            SaveStatementButton.IsEnabled = true;

            QuestionStatement.Visibility = Visibility.Hidden;
            QuestionStatement.IsEnabled = false;
        }

        private void HideEditStatementField()
        {
            EditStatementField.IsEnabled = false;
            EditStatementField.Visibility = Visibility.Hidden;

            EditStatementButton.IsEnabled = true;
            EditStatementButton.Visibility = Visibility.Visible;

            SaveStatementButton.Visibility = Visibility.Hidden;
            SaveStatementButton.IsEnabled = false;

            QuestionStatement.Visibility = Visibility.Visible;
            QuestionStatement.IsEnabled = true;
        }

        private void ShowEditFunctionality()
        {
            listaOriginal = new();
            lista = new ListCollectionView(listaOriginal);
            KeyWordsListView.ItemsSource = lista;

            QuestionStatement.SetValue(Grid.ColumnSpanProperty, 1);

            AnswerField.IsEnabled = false;

            EditStatementButton.Visibility = Visibility.Visible;
            KeyWordsButton.Visibility = Visibility.Visible;
            EditStatementButton.IsEnabled = true; 

            LoadKeyWords();
        }

        private void LoadKeyWords()
        {
            List<string> data = service.GetKeyWordsFromOpenQuestion(questionId);
            for (int i = 0; i < data.Count; i++)
            {
                listaOriginal.Add(new KeyWordItem
                {
                    Word = data[i],
                    Position = i.ToString(),
                });
            }
            lista.Refresh();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            EditStatementButton.ToolTip = Idioma.info["EditStatementButton"];
            SaveStatementButton.ToolTip = Idioma.info["SaveStatementButton"];
            HintAssist.SetHint(AnswerField, Idioma.info["AnswerField"]);
            HintAssist.SetHint(KeyWordsTextBox, Idioma.info["KeyWordsTextBoxHint"]);
            KeyWordsTextTitleBlock.Text = Idioma.info["KeyWordsTextTitleBlock"];
            AcceptWordsButton.Content = Idioma.info["AcceptWordsButton"];
            KeyWordsButtonText.Text = Idioma.info["KeyWordsButtonText"];
        }

        private void KeyWordsButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        private void AcceptWordsButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        private void KeyWordsAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyWordsTextBox.Text != "")
            {
                service.AddKeyWordToOpenQuestion(questionId, KeyWordsTextBox.Text);
                listaOriginal.Add(new KeyWordItem
                {
                    Word = KeyWordsTextBox.Text,
                    Position = listaOriginal.Count.ToString(),
                });
                lista.Refresh();
                KeyWordsTextBox.Text = "";
            }
        }

        private void DeleteWordButton_Click(object sender, RoutedEventArgs e)
        {
            string wordToDelete = (string)(sender as Button).Tag;
            if (wordToDelete != null)
            {
                service.RemoveKeyWordFromOpenQuestion(questionId, wordToDelete);
                listaOriginal.Remove(listaOriginal.Where(x => x.Word == wordToDelete).First());
                lista.Refresh();
            }
        }

        private void KeyWordsTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                KeyWordsAddButton_Click(sender, e);
                KeyWordsTextBox.Focus();
            }
        }
    }

    public struct KeyWordItem
    {
        public string Word { get; set; }
        public string Position { get; set; }
    }
}
