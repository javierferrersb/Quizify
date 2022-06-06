using MaterialDesignThemes.Wpf;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for InscribeStudentCourseDialog.xaml
    /// </summary>
    public partial class InscribeStudentCourseDialog : Page
    {
        private static readonly Regex _numberRegex = new Regex("[^0-9]+"); //regex that matches only integer numbers
        private string idCursoObligatorio = "Es obligatorio introducir el identificador del curso";
        public InscribeStudentCourseDialog()
        {
            InitializeComponent();
            actualizar();
            CourseIdField.Clear();
        }

        private void InscribeCourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CourseIdField.Text.Length > 0)
            {
                ((VistaPrincipal)this.Tag as VistaPrincipal).InscribeStudentToCourse(int.Parse(CourseIdField.Text));
            }
            else
            {
                MessageBox.Show("Es obligatorio introducir el identificador del curso");
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return !_numberRegex.IsMatch(text);
        }

        private void CourseIdField_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            inscribe.Text = Idioma.info["inscribe"];
            CancelButton.Content = Idioma.info["CancelButton"];
            InscribeCourseButton.Content = Idioma.info["InscribeCourseButton"];
            idCursoObligatorio = Idioma.info["idCursoObligatorio"];
            HintAssist.SetHint(CourseIdField, Idioma.info["CourseIdField"]);
        }

        private void CourseIdField_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                InscribeCourseButton_Click(sender, e);
            }
        }
    }
}
