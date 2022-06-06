using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for EditCourseDialog.xaml
    /// </summary>
    public partial class EditCourseDialog : Page
    {
        private IQuizifyService service;
        private int courseId;
        public EditCourseDialog(IQuizifyService service, int courseId)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.courseId = courseId;

            SetFields();
        }

        private void SetFields()
        {
            NameTextBox.Text = service.GetCourseName(courseId);
            DescriptionTextBox.Text = service.GetCourseDescription(courseId);
            IconComboBox.SelectedIndex = service.GetCourseIcon(courseId) - 1;
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            service.UpdateCourseInfo(courseId, NameTextBox.Text, DescriptionTextBox.Text, IconComboBox.SelectedIndex + 1);
            ((VistaPrincipal)this.Tag as VistaPrincipal).HideEditCourseDialog(courseId);
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            EditText.Text = Idioma.info["EditText"];
            SaveButton.Content = Idioma.info["SaveButton"];
            IconLabel.Content = Idioma.info["IconLabel"];
            HintAssist.SetHint(NameTextBox, Idioma.info["NameTextBox"]);
            HintAssist.SetHint(DescriptionTextBox, Idioma.info["DescriptionTextBox"]);
        }
    }
}
