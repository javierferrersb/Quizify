using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for NewQuizDialog.xaml
    /// </summary>
    public partial class NewQuizDialog : Page
    {
        private int topicId;
        private int quizId;
        private string nombreObligatorioQuiz = "Es obligatorio introducir un nombre para el quiz";
        public NewQuizDialog(int topicId)
        {
            InitializeComponent();
            actualizar();
            this.topicId = topicId;
            this.quizId = -1;
        }

        public NewQuizDialog(int quiz, int topic)
        {
            InitializeComponent();
            actualizar();
            this.quizId = quiz;
            this.topicId = topic;
        }
        private void CreateQuizButton_Click(object sender, RoutedEventArgs e)
        {

            if (QuizNameField.Text.Length > 0)
            {
                if (quizId < 0)
                {
                    (this.Tag as VistaPrincipal).CreateQuiz(QuizNameField.Text, topicId);
                }
                else
                {
                    (this.Tag as VistaPrincipal).CopyQuiz(QuizNameField.Text, quizId, topicId);
                }
            }
            else
            {
                MessageBox.Show(nombreObligatorioQuiz);
            }

        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            NewQuiz.Text = Idioma.info["NewQuiz"];
            CancelQuizButton.Content = Idioma.info["CancelQuizButton"];
            CreateQuizButton.Content = Idioma.info["CreateQuizButton"];
            nombreObligatorioQuiz = Idioma.info["nombreObligatorioQuiz"];
            HintAssist.SetHint(QuizNameField, Idioma.info["QuizNameField"]);
        }

        private void QuizNameField_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CreateQuizButton_Click(sender, e);
            }
        }
    }
}
