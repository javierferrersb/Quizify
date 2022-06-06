using Quizify.Services;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI.QuizStatus
{
    /// <summary>
    /// Interaction logic for PublicadoInactivoStatus.xaml
    /// </summary>
    public partial class PublicadoStatus : Page
    {
        private IQuizifyService service;
        private int quizId;
        public PublicadoStatus(IQuizifyService service, int quizId, bool isActive)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.quizId = quizId;

            LoadDateData();
            if (isActive)
            {
                HideUnlaunchButton();
                ShowFinishNowButton();
            }
        }

        private void LoadDateData()
        {
            OpeningDateTextBlock.Text = service.GetQuizOpeningDate(quizId).Value.ToShortDateString();
            ClosingDateTextBlock.Text = service.GetQuizClosingDate(quizId).Value.ToShortDateString();
        }

        private void UnlaunchQuizButton_Click(object sender, RoutedEventArgs e)
        {
            service.UnlaunchQuiz(quizId);
            ((QuizStatusManager)this.Tag).LoadContent();
        }

        private void HideUnlaunchButton()
        {
            UnlaunchQuizButton.Visibility = Visibility.Hidden;
        }
        private void actualizar()
        {
            OpeningDateMessageTextBlock.Text = Idioma.info["OpeningDateMessageTextBlock"];
            ClosingDateMessageTextBlock.Text = Idioma.info["ClosingDateMessageTextBlock"];
            UnlaunchQuizButton.Content = Idioma.info["UnlaunchQuizButton"];
            StatusPIMessageTextBlock.Text = Idioma.info["StatusPIMessageTextBlock"];
            StatusPITextBlock.Text = Idioma.info["StatusPITextBlock"];
        }

        private void FinishNowButton_Click(object sender, RoutedEventArgs e)
        {
            service.FinishQuizImmediately(quizId);
            ((QuizStatusManager)this.Tag).LoadContent();
        }

        private void ShowFinishNowButton()
        {
            FinishNowButton.Visibility = Visibility.Visible;
        }
    }
}
