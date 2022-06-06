using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for NewTopicDialog.xaml
    /// </summary>
    public partial class NewTopicDialog : Page
    {
        private int courseId;
        private string nombreObligatorioTopic = "Es obligatorio introducir un nombre para el contenido";
        public NewTopicDialog(int courseId)
        {
            InitializeComponent();
            actualizar();
            TopicNameField.Clear();
            this.courseId = courseId;
        }

        private void CreateTopicButton_Click(object sender, RoutedEventArgs e)
        {
            if (TopicNameField.Text.Length > 0)
            {
                (this.Tag as VistaPrincipal).CreateNewTopic(courseId, TopicNameField.Text);

            }
            else
            {
                MessageBox.Show(nombreObligatorioTopic);
            }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            NewTopic.Text = Idioma.info["NewTopic"];
            CancelTopicButton.Content = Idioma.info["CancelTopicButton"];
            CreateTopicButton.Content = Idioma.info["CreateTopicButton"];
            nombreObligatorioTopic = Idioma.info["nombreObligatorioTopic"];
            HintAssist.SetHint(TopicNameField, Idioma.info["TopicNameField"]);
        }

        private void TopicNameField_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CreateTopicButton_Click(sender, e);
            }
        }
    }
}
