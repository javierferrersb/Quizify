using Quizify.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para ComprarBonos.xaml
    /// </summary>
    public partial class ComprarBonos : Window
    {
        private IQuizifyService service;
        private string personEmail;
        private String offerSelected;
        private TextBlock quizLabel;
        private string quizzesRestantes = "Quizzes restantes: ";
        public ComprarBonos(IQuizifyService service, string personEmail, TextBlock numQuizLabel)
        {
            quizLabel = numQuizLabel;
            InitializeComponent();
            actualizar();
            this.service = service;
            this.personEmail = personEmail;
            numQuizes.Content = service.getNumQuizesRestantes(personEmail);
        }

        private void selectedOffer1(object sender, RoutedEventArgs e)
        {
            bottonBuy.IsEnabled = true;
            offerSelected = "5";
        }

        private void selectedOffer3(object sender, RoutedEventArgs e)
        {
            bottonBuy.IsEnabled = true;
            offerSelected = "25";
        }

        private void startBuy(object sender, RoutedEventArgs e)
        {
            ConfirmarBonos confirmacion = new(personEmail, offerSelected, quizLabel, service);
            confirmacion.ShowDialog();
            Close();
        }

        private void cancelBuy(object sender, RoutedEventArgs e)
        {
            quizLabel.Text = quizzesRestantes + service.getNumQuizesRestantes(personEmail);
            Close();
        }

        private void selectedOffer2(object sender, RoutedEventArgs e)
        {
            bottonBuy.IsEnabled = true;
            offerSelected = "10";
        }
        private void actualizar()
        {
            this.Title = Idioma.info["ComprarBonosTitulo"];
            MyQuizzesLabel.Content = Idioma.info["MyQuizzesLabel"];
            bono5.Header = Idioma.info["bono5"];
            bono5block.Text = Idioma.info["bono5block"];
            bono10.Header = Idioma.info["bono10"];
            bono10block.Text = Idioma.info["bono10block"];
            bono25.Header = Idioma.info["bono25"];
            bono25block.Text = Idioma.info["bono25block"];
            firstOffer.Content = Idioma.info["firstOffer"];
            secondOffer.Content = Idioma.info["secondOffer"];
            thirdOffer.Content = Idioma.info["thirdOffer"];
            bottonCancel.Content = Idioma.info["bottonCancel"];
            bottonBuy.Content = Idioma.info["bottonBuy"];
            quizzesRestantes = Idioma.info["quizzesRestantes"];
        }
    }
}
