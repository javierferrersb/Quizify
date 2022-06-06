using Quizify.Services;
using System.Windows;

namespace QuizifyUI
{
    public partial class VerLasPreguntasWindows : Window
    {
        private IQuizifyService service;
        public VerLasPreguntasWindows(IQuizifyService service, int questionId)
        {
            InitializeComponent();
            this.service = service;
            int type = service.GetQuestionType(questionId);
            if (type == 0)
            {
                questionFrame.Navigate(new PreguntaAbiertaPage(service, questionId));
            }
            else if (type == 1)
            {
                questionFrame.Navigate(new PreguntaVerdaderoFalsoPage(service, questionId));
            }
        }
    }
}
