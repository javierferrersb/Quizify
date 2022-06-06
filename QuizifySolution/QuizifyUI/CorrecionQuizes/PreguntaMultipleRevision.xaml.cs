using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizifyUI.CorrecionQuizes
{
    /// <summary>
    /// Interaction logic for PreguntaMultipleRevision.xaml
    /// </summary>
    public partial class PreguntaMultipleRevision : Page
    {
        public PreguntaMultipleRevision(int respondePreguntaId, string questionStatement, int studentAnswer, List<bool> CorrectAnswers, List<string> options, bool IsStudentMode)
        {
            InitializeComponent();
            UpdateLanguage();

            Question1RadioButton.Content = options[0];
            Question2RadioButton.Content = options[1];
            Question3RadioButton.Content = options[2];
            Question4RadioButton.Content = options[3];

            StatementTextBlock.Text = questionStatement;

            if (studentAnswer == -1)
            {
                if (!IsStudentMode)
                {
                    Question1RadioButton.Foreground = Brushes.Red;
                    Question2RadioButton.Foreground = Brushes.Red;
                    Question3RadioButton.Foreground = Brushes.Red;
                    Question4RadioButton.Foreground = Brushes.Red;
                }
            }
            else
            {
                switch (studentAnswer)
                {
                    case 0:
                        Question1RadioButton.IsChecked = true;
                        break;
                    case 1:
                        Question2RadioButton.IsChecked = true;
                        break;
                    case 2:
                        Question3RadioButton.IsChecked = true;
                        break;
                    case 3:
                        Question4RadioButton.IsChecked = true;
                        break;
                }
                if (!IsStudentMode)
                {
                    if (CorrectAnswers.ElementAt(studentAnswer))
                    {
                        switch (studentAnswer)
                        {
                            case 0:
                                Question1RadioButton.Foreground = Brushes.Green;
                                break;
                            case 1:
                                Question2RadioButton.Foreground = Brushes.Green;
                                break;
                            case 2:
                                Question3RadioButton.Foreground = Brushes.Green;
                                break;
                            case 3:
                                Question4RadioButton.Foreground = Brushes.Green;
                                break;
                        }
                    }
                    else
                    {
                        switch (studentAnswer)
                        {
                            case 0:
                                Question1RadioButton.Foreground = Brushes.Red;
                                break;
                            case 1:
                                Question2RadioButton.Foreground = Brushes.Red;
                                break;
                            case 2:
                                Question3RadioButton.Foreground = Brushes.Red;
                                break;
                            case 3:
                                Question4RadioButton.Foreground = Brushes.Red;
                                break;
                        }
                        for (int i = 0; i < CorrectAnswers.Count; i++)
                        {
                            if (CorrectAnswers.ElementAt(i))
                            {
                                switch (i)
                                {
                                    case 0:
                                        Question1RadioButton.Foreground = Brushes.Green;
                                        break;
                                    case 1:
                                        Question2RadioButton.Foreground = Brushes.Green;
                                        break;
                                    case 2:
                                        Question3RadioButton.Foreground = Brushes.Green;
                                        break;
                                    case 3:
                                        Question4RadioButton.Foreground = Brushes.Green;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateLanguage()
        {
            StatementLabel.Text = Idioma.info["StatementTextBox"] + ": ";
            AnswerLabel.Text = Idioma.info["AnswerField"] + ": ";
        }
    }
}
