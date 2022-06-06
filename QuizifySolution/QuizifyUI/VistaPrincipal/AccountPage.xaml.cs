using Quizify.Services;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for InstructorAccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        private string personEmail;
        private IQuizifyService service;
        private string quizzesRestantes = "Quizzes restantes: ";
        public AccountPage(IQuizifyService service, string personEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.personEmail = personEmail;

            FillData();
            if (PersonIsStudent())
            {
                HideInstructorFunctionality();
            }
            else
            {
                ShowRemainigQuizes();
            }
        }

        private bool PersonIsStudent()
        {
            return service.IsPersonAStudent(personEmail);
        }

        private void HideInstructorFunctionality()
        {
            RemainingQuizesTextBlock.Visibility = Visibility.Hidden;
            BuyBonosButton.Visibility = Visibility.Hidden;
            GestionBatBut.Visibility = Visibility.Hidden;
        }

        private void ShowRemainigQuizes()
        {
            RemainingQuizesTextBlock.Text = quizzesRestantes + service.getNumQuizesRestantes(personEmail);

        }
        private void FillData()
        {
            NameTextBlock.Text = service.GetPersonName(personEmail);
            EmailTextBlock.Text = personEmail;
        }
        private void BuyBonosButton_Click(object sender, RoutedEventArgs e)
        {
            ComprarBonos compra = new(service, personEmail, RemainingQuizesTextBlock);
            compra.ShowDialog();
            RemainingQuizesTextBlock.Text = quizzesRestantes + service.getNumQuizesRestantes(personEmail);
        }

        private void actualizar()
        {
            //Idioma.Controles(this);
            LogoutButton.Content = Idioma.info["LogoutButton"];
            RemainingQuizesTextBlock.Text = Idioma.info["RemainingQuizesTextBlock"];
            BuyBonosButton.Content = Idioma.info["BuyBonosButton"];
            quizzesRestantes = Idioma.info["quizzesRestantes"];
            GestionBatBut.Content = Idioma.info["GestionBatBut"];
            InfoCerrarSesion.Text = Idioma.info["InfoCerrarSesion"];
            AcceptLaunchButton.Content = Idioma.info["AcceptLaunchButton"];
            DenyLaunchButton.Content = Idioma.info["DenyLaunchButton"];
        }

        private void gestionBateria(object sender, RoutedEventArgs e)
        {
            GestionBateriaPreguntas g = new GestionBateriaPreguntas(service, personEmail);
            g.ShowDialog();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            (this.Tag as VistaPrincipal).Logout();
        }

    }
}
