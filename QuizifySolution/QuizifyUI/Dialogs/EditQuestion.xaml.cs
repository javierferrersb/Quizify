using Quizify.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para EditQuestion.xaml
    /// </summary>
    public partial class EditQuestion : Page
    {
        private IQuizifyService service;
        private int questionId;
        private PreguntaItem q;
        public EditQuestion(IQuizifyService service, PreguntaItem question, bool activo)
        {
            InitializeComponent();
            questionName.Text = service.GetQuestionStatement(question.Id);
            this.service = service;
            this.questionId = question.Id;
            this.q = question;
            EditQuestionButton.IsEnabled = activo;
            EditQuestionButton.Content = Idioma.info["editarPregunta"];
            asignarCompetenciaButton.Content = Idioma.info["asignarCompetenciaButton"];


        }

        private void asignarCompetencia_Click(object sender, RoutedEventArgs e)
        {
            AsignarContenido ac = new AsignarContenido(service, q);
            ac.ShowDialog();
        }

        private void EditQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            VerLasPreguntasWindows vpw = new VerLasPreguntasWindows(service, questionId);
            vpw.ShowDialog();
        }
    }
}
