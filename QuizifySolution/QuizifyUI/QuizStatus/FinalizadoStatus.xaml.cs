using QuizifyUI.CorrecionQuizes;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI.QuizStatus
{
    /// <summary>
    /// Interaction logic for FinalizadoStatus.xaml
    /// </summary>
    public partial class FinalizadoStatus : Page
    {
        public FinalizadoStatus()
        {
            InitializeComponent();
            actualizar();
        }
        private void actualizar()
        {
            StatusFMessageTextBlock.Text = Idioma.info["StatusFMessageTextBlock"];
            StatusFTextBlock.Text = Idioma.info["StatusFTextBlock"];
        }
    }
}
