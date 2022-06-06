using Quizify.Services;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizifyUI.CorrecionQuizes
{
    /// <summary>
    /// Interaction logic for PreguntaVFRevision.xaml
    /// </summary>
    public partial class PreguntaVFRevision : Page
    {
        public PreguntaVFRevision(IQuizifyService service, int respondePreguntaId, string questionStatement, bool correctAnswer, bool IsStudentMode)
        {
            InitializeComponent();
            bool? studentAnswer = service.GetVFStudentAnswer(respondePreguntaId);
            StatementTextBlock.Text = questionStatement;

            if (studentAnswer == null)
            {
                if (!IsStudentMode)
                {
                    TrueRadioButton.Foreground = Brushes.Red;
                    FalseRadioButton.Foreground = Brushes.Red;
                }
            }
            else
            {
                if (studentAnswer.Value)
                {
                    TrueRadioButton.IsChecked = true;
                }
                else
                {
                    FalseRadioButton.IsChecked = true;
                }
                if (!IsStudentMode)
                {
                    if (correctAnswer == studentAnswer.Value && correctAnswer == true)
                    {
                        TrueRadioButton.Foreground = Brushes.Green;
                    }
                    else if (correctAnswer == studentAnswer.Value && correctAnswer == false)
                    {
                        FalseRadioButton.Foreground = Brushes.Green;
                    }
                    else if (studentAnswer.Value == true)
                    {
                        TrueRadioButton.Foreground = Brushes.Red;
                        FalseRadioButton.Foreground = Brushes.Green;
                    }
                    else
                    {
                        FalseRadioButton.Foreground = Brushes.Red;
                        TrueRadioButton.Foreground = Brushes.Green;
                    }
                }
            }
            UpdateLanguage();
        }
        private void UpdateLanguage()
        {
            TrueRadioButton.Content = Idioma.info["TrueRadioButton"];
            FalseRadioButton.Content = Idioma.info["FalseRadioButton"];
            StatementLabel.Text = Idioma.info["StatementTextBox"] + ": ";
            AnswerLabel.Text = Idioma.info["AnswerField"] + ": ";
        }
    }
}
