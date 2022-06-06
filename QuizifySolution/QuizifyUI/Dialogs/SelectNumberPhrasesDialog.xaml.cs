using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for NewTopicDialog.xaml
    /// </summary>
    public partial class SelectNumberPhrasesDialog : Page
    {
        public SelectNumberPhrasesDialog()
        {
            InitializeComponent();
            actualizar();
        }

        private void SelectNumberButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            /*CancelTopicButton.Content = Idioma.info["CancelTopicButton"];
            CreateTopicButton.Content = Idioma.info["CreateTopicButton"];
            nombreObligatorioTopic = Idioma.info["nombreObligatorioTopic"];
            HintAssist.SetHint(TopicNameField, Idioma.info["TopicNameField"]);*/
        }
    }
}
