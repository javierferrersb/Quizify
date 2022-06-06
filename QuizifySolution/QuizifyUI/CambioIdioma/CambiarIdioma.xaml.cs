using Quizify.Services;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para AsignarPuntuación.xaml
    /// </summary>
    public partial class CambiarIdioma : Window
    {
        //private String subject;
        private string file = Idioma.file;
        private string confirmChanges = "Confirmar cambios";
        private string sureToSave = "¿Estás seguro de guardar los cambios?";

        public CambiarIdioma(IQuizifyService service)
        {
            InitializeComponent();
            if (file.Equals("es.txt"))
            {
                languageSelector.SelectedItem = es;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
                languageSelector.SelectedItem = en;
            }
            actualizar();
        }

        private void saveChanges(object sender, RoutedEventArgs e)
        {
            string language = languageSelector.Text;
            if (language.Equals(es.Content.ToString()))
            {
                file = "es.txt";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            }
            else if (language.Equals(en.Content.ToString()))
            {
                file = "en.txt";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
            }
            Idioma.cambiarIdioma(file);
            actualizar();
            Close();
        }

        private void close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void actualizar()
        {
            this.Title = Idioma.info["IdiomaTitulo"];
            IdiomaLabel.Content = Idioma.info["IdiomaLabel"];
            CancelButton.Content = Idioma.info["CancelButton"];
            SaveButton.Content = Idioma.info["SaveButton"];
            confirmChanges = Idioma.info["confirmChanges"];
            sureToSave = Idioma.info["sureToSave"];
            es.Content = Idioma.info["es"];
            en.Content = Idioma.info["en"];
            InfoCambiarIdioma.Text = Idioma.info["InfoCambiarIdioma"];
            DenyLaunchButton.Content = Idioma.info["DenyLaunchButton"];
            AcceptLaunchButton.Content = Idioma.info["AcceptLaunchButton"];
        }
    }
}
