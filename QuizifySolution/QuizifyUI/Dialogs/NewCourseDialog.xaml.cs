using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for NewCourseDialog.xaml
    /// </summary>
    public partial class NewCourseDialog : Page
    {
        public int cursoId;
        private string nombreObligatorioCurso = "Es obligatorio introducir un nombre para el curso";
        public NewCourseDialog()
        {
            this.cursoId = -1;
            InitializeComponent();
            actualizar();
            CourseNameField.Clear();
        }

        public NewCourseDialog(int cursoId)
        {
            this.cursoId = cursoId;
            InitializeComponent();
            actualizar();
            CourseNameField.Clear();
        }

        private void CreateCourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CourseNameField.Text.Length > 0)
            {
                if (cursoId < 0)
                {
                    (this.Tag as VistaPrincipal).CreateCourse(CourseNameField.Text);
                }
                else
                {
                    (this.Tag as VistaPrincipal).CopyCourse(CourseNameField.Text, this.cursoId);
                }
            }
            else
            {
                MessageBox.Show(nombreObligatorioCurso);
            }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            NewCurso.Text = Idioma.info["NewCurso"];
            CancelCourseButton.Content = Idioma.info["CancelCourseButton"];
            CreateCourseButton.Content = Idioma.info["CreateCourseButton"];
            NewCurso.Text = Idioma.info["NewCurso"];
            nombreObligatorioCurso = Idioma.info["nombreObligatorioCurso"];
            HintAssist.SetHint(CourseNameField, Idioma.info["CourseNameField"]);
        }

        private void CourseNameField_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CreateCourseButton_Click(sender, e);
            }
        }
    }
}
