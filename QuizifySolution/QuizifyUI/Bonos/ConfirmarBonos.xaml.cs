using Quizify.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para ConfirmarBonos.xaml
    /// </summary>
    public partial class ConfirmarBonos : Window
    {
        private string personEmail;
        private IQuizifyService service;
        private String quizes;
        private TextBlock quizLabel;
        private string confirmBuy = "Confirmar compra";
        private string sureToBuy = "¿Estás seguro de confirmar la compra?";
        private string quizzesRestantes = "Quizzes restantes: ";
        public ConfirmarBonos(string personEmail, String bonos, TextBlock numQuizLabel, IQuizifyService service)
        {
            quizLabel = numQuizLabel;
            this.service = service;
            InitializeComponent();
            actualizar();
            quizes = bonos;
            this.personEmail = personEmail;
        }
        private int selectOffer(String nQuiz)
        {
            switch (nQuiz)
            {
                case "5":
                    return 5;
                case "10":
                    return 10;
                default:
                    return 25;
            }

        }
        private void startBuy(object sender, RoutedEventArgs e)
        {
            service.setQuizes(personEmail, selectOffer(quizes));
            quizLabel.Text = quizzesRestantes + service.getNumQuizesRestantes(personEmail);
            Close();
        }
        private void cancelBuy(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["ConfirmarBonosTitulo"];
            CountLabel.Content = Idioma.info["CountLabel"];
            CaducityLabel.Content = Idioma.info["CaducityLabel"];
            SecretNumberLabel.Content = Idioma.info["SecretNumberLabel"];
            bottonCancel1.Content = Idioma.info["bottonCancel1"];
            bottonBuy1.Content = Idioma.info["bottonBuy1"];
            confirmBuy = Idioma.info["confirmBuy"];
            sureToBuy = Idioma.info["sureToBuy"];
            quizzesRestantes = Idioma.info["quizzesRestantes"];
            InfoComprarBonos.Text = Idioma.info["InfoComprarBonos"];
            DenyLaunchButton.Content = Idioma.info["DenyLaunchButton"];
            AcceptLaunchButton.Content = Idioma.info["AcceptLaunchButton"];
        }
        private void changeText(object sender, KeyEventArgs e)
        {
            if (creditCard1.Text != "" && creditCardLimit.Text != "" && creditCardSecretNum.Text != ""
                && creditCard1.Text.Length == 16 && creditCardLimit.Text.Length == 5 && creditCardSecretNum.Text.Length == 3) { bottonBuy1.IsEnabled = true; }
            else { bottonBuy1.IsEnabled = false; }
        }
    }
}
