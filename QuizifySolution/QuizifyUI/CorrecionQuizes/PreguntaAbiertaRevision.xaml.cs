using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizifyUI.CorrecionQuizes
{
    /// <summary>
    /// Interaction logic for PreguntaAbiertaRevision.xaml
    /// </summary>
    public partial class PreguntaAbiertaRevision : Page
    {
        private string answer;
        List<String> keyWords;
        public PreguntaAbiertaRevision(IQuizifyService service, int respondePreguntaId, string questionStatement, List<String> keyWords, bool IsStudentMode)
        {
            InitializeComponent();
            this.keyWords = keyWords;

            answer = service.GetRespuestOfRespuestaPreguntaAbiertaById(respondePreguntaId);
            AnswerTextBlock.Text = answer;
            StatementTextBlock.Text = questionStatement;

            if (!IsStudentMode)
            {
                LoadKeyWords();
            }

            UpdateLanguage();
        }
        private void LoadKeyWords()
        {
            for (int i = 0; i < keyWords.Count; i++)
            {
                RowDefinition row = new();
                row.Height = new GridLength(1, GridUnitType.Auto);
                KeyWordsGrid.RowDefinitions.Add(row);

                TextBlock keyTextBlock = new();
                keyTextBlock.Text = keyWords[i];
                KeyWordsGrid.Children.Add(keyTextBlock);
                Grid.SetColumn(keyTextBlock, 1);
                Grid.SetRow(keyTextBlock, i);

                TextBlock markTextBlock = new();
                markTextBlock.Text = answer.Contains(keyWords[i]) ? "✓" : "✗";
                markTextBlock.Foreground = answer.Contains(keyWords[i]) ? Brushes.Green : Brushes.Red;
                KeyWordsGrid.Children.Add(markTextBlock);
                Grid.SetColumn(markTextBlock, 0);
                Grid.SetRow(markTextBlock, i);

                KeyWordsLabel.Visibility = Visibility.Visible;
            }
        }
        private void UpdateLanguage()
        {
            StatementLabel.Text = Idioma.info["StatementTextBox"] + ": ";
            AnswerLabel.Text = Idioma.info["AnswerField"] + ": ";
            KeyWordsLabel.Text = Idioma.info["KeyWordsLabel"];
        }
    }
}
