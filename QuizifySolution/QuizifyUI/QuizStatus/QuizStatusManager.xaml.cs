using Quizify.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizifyUI.QuizStatus
{
    /// <summary>
    /// Interaction logic for QuizStatusManager.xaml
    /// </summary>
    public partial class QuizStatusManager : Window
    {
        private IQuizifyService service;
        private QuizItem quiz;
        private List<Button> statusButtons;
        private Window VistaPrincipalWindow;
        private string statusOf = "Estado de ";
        public QuizStatusManager(IQuizifyService service, QuizItem quiz, Window VistaPrincipalWindow)
        {
            InitializeComponent();
            actualizar();
            statusButtons = new List<Button>();
            SetStatusButtonsList();
            this.service = service;

            this.VistaPrincipalWindow = VistaPrincipalWindow;
            this.quiz = quiz;
            SetQuizStatusButtons();
            SetWindowTitleName();
            SetHeaderName();
            LoadContent();
        }
        private void SetWindowTitleName()
        {
            this.Title = statusOf + quiz.Name;
        }

        private void SetHeaderName()
        {
            QuizNameTextBlock.Text = statusOf + quiz.Name;
        }

        private void SetStatusButtonsList()
        {
            statusButtons.Add(BorradorButton);
            statusButtons.Add(PublicadoInactivoButton);
            statusButtons.Add(PublicadoActivoButton);
            statusButtons.Add(FinalizadoButton);
            statusButtons.Add(CalificadoButton);
        }
        private void SetQuizStatusButtons()
        {
            string quizStatus = quiz.Status.ToString();
            if (quizStatus == "-1")
            {
                quizStatus = "0";
            }
            foreach (Button button in statusButtons)
            {
                if (button.Tag.ToString() == quizStatus)
                {
                    EnableButton(button);
                }
                else
                {
                    DisableButton(button);
                }
            }
        }

        private static void DisableButton(Button button)
        {
            button.Cursor = Cursors.No;
            button.Style = (Style)Application.Current.Resources["MaterialDesignFlatButton"];
        }

        private static void EnableButton(Button button)
        {
            button.Cursor = Cursors.Arrow;
            button.Style = (Style)Application.Current.Resources["MaterialDesignRaisedButton"];
        }

        private void QuizStatusFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (QuizStatusFrame.Content is BorradorStatus)
            {
                ((BorradorStatus)QuizStatusFrame.Content as BorradorStatus).Tag = this;
            }
            else if (QuizStatusFrame.Content is PublicadoStatus)
            {
                ((PublicadoStatus)QuizStatusFrame.Content as PublicadoStatus).Tag = this;
            }
            else if (QuizStatusFrame.Content is FinalizadoStatus)
            {
                ((FinalizadoStatus)QuizStatusFrame.Content as FinalizadoStatus).Tag = this;
            }
            else if (QuizStatusFrame.Content is CalificadoStatus)
            {
                ((CalificadoStatus)QuizStatusFrame.Content as CalificadoStatus).Tag = this;
            }
        }

        public void LoadContent()
        {
            quiz.Status = service.GetQuizStatus(quiz.Id);
            ((VistaPrincipal)VistaPrincipalWindow).RefreshQuizesList(quiz.Id);
            SetQuizStatusButtons();
            switch (quiz.Status)
            {
                case -1:
                    QuizStatusFrame.Navigate(new BorradorStatus(service, quiz.Id, true));
                    break;
                case 0:
                    QuizStatusFrame.Content = new BorradorStatus(service, quiz.Id, false);
                    break;
                case 1:
                    QuizStatusFrame.Content = new PublicadoStatus(service, quiz.Id, false);
                    break;
                case 2:
                    QuizStatusFrame.Content = new PublicadoStatus(service, quiz.Id, true);
                    break;
                case 3:
                    QuizStatusFrame.Content = new FinalizadoStatus();
                    break;
                case 4:
                    QuizStatusFrame.Content = new CalificadoStatus();
                    break;
            }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["QuizStatusManagerTitulo"];
            BorradorButton.Content = Idioma.info["BorradorButton"];
            PublicadoInactivoButton.Content = Idioma.info["PublicadoInactivoButton"];
            PublicadoActivoButton.Content = Idioma.info["PublicadoActivoButton"];
            FinalizadoButton.Content = Idioma.info["FinalizadoButton"];
            CalificadoButton.Content = Idioma.info["CalificadoButton"];
            AcceptButton.Content = Idioma.info["AcceptButton"];
            statusOf = Idioma.info["statusOf"];
        }

        public void SetViewingAnswers()
        {
            service.SetQuizViewAnsers(quiz.Id, true);
            LoadContent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
