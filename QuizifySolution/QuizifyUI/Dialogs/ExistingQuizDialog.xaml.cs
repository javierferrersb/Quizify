using Quizify.Services;
using QuizifyUI.CorrecionQuizes;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for ExistingQuizDialog.xaml
    /// </summary>
    public partial class ExistingQuizDialog : Page
    {
        private QuizItem quiz;
        private string personEmail;
        private IQuizifyService service;
        private Window mainWindow;
        public ExistingQuizDialog(IQuizifyService service, QuizItem quiz, string personEmail, Window mainWindow)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.personEmail = personEmail;

            this.mainWindow = mainWindow;
            this.quiz = quiz;
            DisableCorrespondingButtons();
            ShowQuizName();
        }

        private void DisableCorrespondingButtons()
        {
            switch (quiz.Status)
            {
                case -1:
                    ViewQuizResultsButton.IsEnabled = false;
                    QuizRecuperacionButton.IsEnabled = false;
                    ViewStudentResultsButton.IsEnabled = false;
                    break;
                case 0:
                    ViewQuizResultsButton.IsEnabled = false;
                    QuizRecuperacionButton.IsEnabled = false;
                    ViewStudentResultsButton.IsEnabled = false;
                    break;
                case 1:
                    QuizRecuperacionButton.IsEnabled = false;
                    break;
                case 2:
                    QuizRecuperacionButton.IsEnabled = false;
                    break;
                case 3:
                    QuizRecuperacionButton.IsEnabled = false;
                    break;
                default:
                    EditQuizButton.IsEnabled = false;
                    break;
            }
        }
        private void ShowQuizName()
        {
            QuizNameTextBlock.Text = quiz.Name;
        }

        private void EditQuizButton_Click(object sender, RoutedEventArgs e)
        {
            EditQuiz editarQuizWindow = new(service, quiz.Id, personEmail, mainWindow);
            editarQuizWindow.ShowDialog();
        }

        private void ViewQuizResultsButton_Click(object sender, RoutedEventArgs e)
        {
            EstadisticasDeQuiz estadisticasDeQuiz = new EstadisticasDeQuiz(service, quiz, quiz.Id);
            estadisticasDeQuiz.ShowDialog();
            //            VerEstadisticasInstructor verEstadisticasInstructor = new VerEstadisticasInstructor(quiz.Id, service.GetCourseOfTopic(service.GetTopicFromQuiz(quiz.Id)), service);
            //            verEstadisticasInstructor.ShowDialog();
        }

        private void WeightQuizButtonButton_Click(object sender, RoutedEventArgs e)
        {
            AsignarPesoEvaluacion asignarPesoEvaluacion = new AsignarPesoEvaluacion(service, service.GetTopicFromQuiz(quiz.Id), quiz.Id);
            asignarPesoEvaluacion.ShowDialog();
        }

        private void ViewStudentResultsButton_Click(object sender, RoutedEventArgs e)
        {
            CorregirQuiz corregirQuiz = new CorregirQuiz(service, quiz.Id);
            corregirQuiz.ShowDialog();
        }

        private void CloneQuizButton_Click(object sender, RoutedEventArgs e)
        {
            ((VistaPrincipal)this.Tag as VistaPrincipal).ShowCopyQuizDialog(quiz.Id, service.GetTopicFromQuiz(quiz.Id));
        }

        private void QuizStatusButton_Click(object sender, RoutedEventArgs e)
        {
            ((VistaPrincipal)this.Tag as VistaPrincipal).ShowQuizStatusDialog(quiz);
        }
        private void actualizar()
        {
            EditQuizButtonText.Text = Idioma.info["EditQuizButtonText"];
            QuizStatusButtonText.Text = Idioma.info["QuizStatusButton"];
            ViewQuizResultsButtonText.Text = Idioma.info["ViewQuizResultsButton"];
            ViewStudentResultsButtonText.Text = Idioma.info["ViewStudentResultsButton"];
            WeightQuizButtonText.Text = Idioma.info["MarkQuizButton"];
            CloneQuizButtonText.Text = Idioma.info["CloneQuizButton"];
            HelpTextBlock.Text = Idioma.info["HelpTextBlock"];
            EditQuizButtonTextHelp.Text = Idioma.info["EditQuizButtonText"];
            QuizStatusButtonTextHelp.Text = Idioma.info["QuizStatusButton"];
            ViewQuizResultsButtonTextHelp.Text = Idioma.info["ViewQuizResultsButton"];
            ViewStudentResultsButtonTextHelp.Text = Idioma.info["ViewStudentResultsButton"];
            WeightQuizButtonTextHelp.Text = Idioma.info["MarkQuizButton"];
            RecuperacionQuizButtonText.Text = Idioma.info["RecoveryQuizButton"];
            CloneQuizButtonTextHelp.Text = Idioma.info["CloneQuizButton"];
            CloseHelpButton.Content = Idioma.info["CloseHelpButton"];
            RecuperacionQuizButtonTextHelp.Text = Idioma.info["RecoveryQuizButton"];
            RecuperacionQuizButtonText.Text = Idioma.info["RecoveryQuizButton"];
        }

        private void WeightQuizButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["WeightQuizButtonHelp"]);
        }

        private void QuizStatusButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["QuizStatusButtonHelp"]);
        }

        private void EditQuizButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["EditQuizButtonHelp"]);
        }

        private void ViewQuizResultsButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["ViewQuizResultsButtonHelp"]);
        }

        private void ViewStudentResultsButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["ViewStudentResultsButtonHelp"]);
        }

        private void CloneQuizButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["CloneQuizButtonHelp"]);
        }

        private void QuizRecuperacionButtonButton_Click(object sender, RoutedEventArgs e)
        {
            int newQuizId = service.CopyQuizNormal(service.GetNameQuizById(quiz.Id) + " - Recuperación", quiz.Id, service.GetTopicFromQuiz(quiz.Id));
            ((VistaPrincipal)this.Tag as VistaPrincipal).RefreshQuizesList(newQuizId);
        }

        private void QuizRecuperacionButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Idioma.info["RecoverQuizButtonHelp"]);
        }
    }
}
