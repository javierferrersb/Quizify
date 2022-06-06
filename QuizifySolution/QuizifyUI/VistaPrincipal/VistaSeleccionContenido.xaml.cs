using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para VistaCursoDeEstudiante.xaml
    /// </summary>
    public partial class VistaSeleccionContenido : Page
    {
        private int courseId;
        private string personEmail;
        private IQuizifyService service;
        private string codigoCurso = "Código del curso: ";
        private string nombreContenido = "Nombre";
        public VistaSeleccionContenido(IQuizifyService service, int courseId, string personEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.courseId = courseId;
            this.personEmail = personEmail;

            if (PersonIsStudent())
            {
                HideInstructorFunctionality();
            }

            CourseCodeTextBlock.Text = codigoCurso + courseId;

            SetTextPath();
            ShowContenidos();
        }

        private void HideInstructorFunctionality()
        {
            AddTopicButton.Visibility = Visibility.Hidden;
            CloneButton.Visibility = Visibility.Hidden;
            CourseInfoButton.Visibility = Visibility.Hidden;
        }

        private bool PersonIsStudent()
        {
            return service.IsPersonAStudent(personEmail);
        }

        public void SetTextPath()
        {
            NombreCurso.Text = service.GetCourseName(courseId);
        }

        public void ShowContenidos()
        {
            List<List<String>> list = service.GetTopicsFromCourse(courseId);

            CreateTopicsColumns();

            foreach (List<String> listOfTopicInfo in list)
            {
                TopicsTable.Items.Add(new TopicItem
                {
                    Id = int.Parse(listOfTopicInfo.ElementAt(0)),
                    Name = listOfTopicInfo.ElementAt(1),
                    CourseId = int.Parse(listOfTopicInfo.ElementAt(2))
                });
            }
        }

        private void CreateTopicsColumns()
        {
            TopicsTable.Columns.Clear();
            TopicsTable.Items.Clear();

            GridManager.AddGridColumn(TopicsTable, nombreContenido, "Name");

            TopicsTable.MouseDoubleClick += TopicsTable_MouseDoubleClick;
        }

        private void TopicsTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            int selectedTopicId = ((TopicItem)contentSelected.Item).Id;

            ((VistaPrincipal)this.Tag as VistaPrincipal).ShowQuizzesOfTopic(selectedTopicId, courseId);

        }

        private void AddTopicButton_Click(object sender, RoutedEventArgs e)
        {
            ((VistaPrincipal)this.Tag as VistaPrincipal).ShowCreateTopicDialog(courseId);
        }

        private void CopyCourse(object sender, RoutedEventArgs e)
        {
            (this.Tag as VistaPrincipal).ShowCopyCourseDialog(courseId);
        }

        private void EditCourseButton_Click(object sender, RoutedEventArgs e)
        {
            ((VistaPrincipal)this.Tag as VistaPrincipal).ShowEditCourseDialog(courseId);
        }
        private void actualizar()
        {
            // Idioma.Controles(this);
            CloneButtonText.Text = Idioma.info["CloneButtonText"];
            AddTopicButtonText.Text = Idioma.info["AddTopicButtonText"];
            codigoCurso = Idioma.info["codigoCurso"];
            nombreContenido = Idioma.info["nombreContenido"];
            showTheCode.Text = Idioma.info["showTheCode"];
        }
    }
}
