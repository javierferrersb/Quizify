using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para CreacionBateriaPreguntas.xaml
    /// </summary>
    public partial class CreacionBateriaPreguntas : Window
    {
        private IQuizifyService service;
        private string personEmail;
        private string complete = "Complete todos los campos";
        private string antesDe = "Antes de continuar debe rellenar todos los campos";
        private string questionVF = "Verdadero/Falso";
        private string questionOpen = "Abierta";
        private string questionMult = "Selección multiple";
        private string questionCor = "Correspondencia";
        private string questionPar = "Parametrizada";
        public CreacionBateriaPreguntas(IQuizifyService service, string personEmail)
        {
            this.personEmail = personEmail;
            this.service = service;
            InitializeComponent();
            actualizar();
            setQuestionType();
            setCursosComboBox();
        }
        private void setQuestionType()
        {
            List<String> c = new List<String>();
            c.Add(questionVF);
            c.Add(questionOpen);
            c.Add(questionMult);
            c.Add(questionCor);
            c.Add(questionPar);
            tipoPreguntaComboBox.ItemsSource = c;
        }
        private void setContenidoComboBox()
        {

            contenidoComboBox.ItemsSource = service.AddContenidoByCurso(personEmail, cursoComboBox.SelectedItem.ToString());
        }

        private void setQuizesComboBox()
        {

            quizComboBox.ItemsSource = service.GetQuizzesFromInstructorCourseAndTopic(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString());
        }

        private void setCursosComboBox()
        {
            cursoComboBox.ItemsSource = service.GetCoursesFromTeacher(personEmail);
        }
        private void addQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (cursoComboBox.SelectedItem != null && tipoPreguntaComboBox.SelectedItem != null && contenidoComboBox.SelectedItem != null
                && quizComboBox.SelectedItem != null && nombreBateria.Text != null)
            {
                BuscadorPreguntas buscador;
                if (tipoPreguntaComboBox.SelectedItem.Equals(questionOpen))
                {
                    buscador = new BuscadorPreguntas(service, personEmail, service.CreateBateria(0,service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()), nombreBateria.Text));
                    buscador.ShowDialog();
                    Close();
                }
                else if (tipoPreguntaComboBox.SelectedItem.Equals(questionVF))
                {
                    buscador = new BuscadorPreguntas(service, personEmail, service.CreateBateria(1,service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()), nombreBateria.Text));
                    buscador.ShowDialog();
                    Close();
                }
                else if (tipoPreguntaComboBox.SelectedItem.Equals(questionMult))
                {
                    buscador = new BuscadorPreguntas(service, personEmail, service.CreateBateria(2, service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()), nombreBateria.Text));
                    buscador.ShowDialog();
                    Close();
                }
            }
            else
            {
                MessageBox.Show(antesDe, complete, MessageBoxButton.OK);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cursoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contenidoComboBox.IsEnabled = true;
            quizComboBox.SelectedItem = null;
            quizComboBox.IsEnabled = false;
            setContenidoComboBox();

        }

        private void contenidoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(contenidoComboBox.SelectedItem != null)
            {
                quizComboBox.IsEnabled = true;
                setQuizesComboBox();
            }
        }

        private void tipoPreguntaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClearNombre_Click(object sender, RoutedEventArgs e)
        {
            nombreBateria.Clear();
        }

        private void ClearTipoPregunta_Click(object sender, RoutedEventArgs e)
        {
            tipoPreguntaComboBox.SelectedItem = null;
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["CreaBatTitulo"];
            nameLabel.Content = Idioma.info["nameLabel"];
            cursoLabel.Content = Idioma.info["cursoLabel"];
            pregTypeLabel.Content = Idioma.info["pregTypeLabel"];
            topicLabel.Content = Idioma.info["topicLabel"];
            borrar1.Content = Idioma.info["borrar1"];
            borrar4.Content = Idioma.info["borrar1"];
            complete = Idioma.info["complete"];
            antesDe = Idioma.info["antesDe"];
            returnButton.Content = Idioma.info["returnButton"];
            addQuestion.Content = Idioma.info["addQuestion"];
            questionVF = Idioma.info["questionVF"];
            questionOpen = Idioma.info["questionOpen"];
            questionMult = Idioma.info["questionMult"];
            questionCor = Idioma.info["questionCor"];
            questionPar = Idioma.info["questionPar"];
    }
    }
}
