using System.Windows.Controls;

namespace QuizifyUI.QuizStatus
{
    /// <summary>
    /// Interaction logic for CalificadoStatus.xaml
    /// </summary>
    public partial class CalificadoStatus : Page
    {
        public CalificadoStatus()
        {
            InitializeComponent();
            actualizar();
        }
        public void actualizar()
        {
            StatusCMessageTextBlock.Text = Idioma.info["StatusCMessageTextBlock"];
            StatusCTextBlock.Text = Idioma.info["StatusCTextBlock"];
        }
    }
}
