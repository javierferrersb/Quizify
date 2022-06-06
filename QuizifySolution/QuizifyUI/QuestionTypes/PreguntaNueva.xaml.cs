using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for PreguntaNueva.xaml
    /// </summary>
    public partial class PreguntaNueva : Page
    {
        public PreguntaNueva()
        {
            InitializeComponent();
            actualizar();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            PulseParaAñadir.Text = Idioma.info["PulseParaAñadir"];
        }
    }
}
