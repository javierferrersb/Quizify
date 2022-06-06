
using Quizify.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para EdicionBateria.xaml
    /// </summary>
    public partial class EdicionBateria : Window
    {
        Boolean cambio;
        private IQuizifyService service;
        private int bateriaId;
        private string personEmail;
        private int quizId;
        private string siModificas = "Si modificas un campo vas a poder crear una nueva batería o modificar la misma";
        private string atention = "Atención";
        private string complete = "Complete todos los campos";
        private string antesDe = "Antes de continuar debe rellenar todos los campos";
        private string questionVF = "Verdadero/Falso";
        private string questionOpen = "Abierta";
        private string questionMult = "Selección multiple";
        private string questionCor = "Correspondencia";
        private string questionPar = "Parametrizada";
        public EdicionBateria(IQuizifyService service, string email, int batteryId, int quizId)
        {
            cambio = true;
            this.service = service;
            bateriaId = batteryId;
            personEmail = email;
            this.quizId = quizId;
            InitializeComponent();
            actualizar();
            setCursosComboBox();
            completeGaps(bateriaId, quizId);
        }

        private void setContenidoComboBox()
        {
            contenidoComboBox.ItemsSource = service.AddContenidoByCurso(personEmail, cursoComboBox.SelectedItem.ToString()); ;
        }

        private void setQuizesComboBox()
        {
            quizComboBox.ItemsSource = service.GetQuizzesFromInstructorCourseAndTopic(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString());
        }
        private void setCursosComboBox()
        {
            cursoComboBox.ItemsSource = service.GetCoursesFromTeacher(personEmail);
        }

        private void cursoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cambio == false)
            {
                advertenciaCrearNuevaBatería();
            }
            contenidoComboBox.IsEnabled = true;
            quizComboBox.SelectedItem = null;
            quizComboBox.IsEnabled = false;
            setContenidoComboBox();
        }

        private void contenidoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cambio == false)
            {
                advertenciaCrearNuevaBatería();
            }

            if (contenidoComboBox.SelectedItem != null)
            {
                quizComboBox.IsEnabled = true;
                setQuizesComboBox();
            }

        }

        public void saltarAlerta()
        {
            MessageBoxResult confirmResult = MessageBox.Show(complete, antesDe, MessageBoxButton.YesNo);
            if (confirmResult == MessageBoxResult.Yes) { }
            else { }
        }

        private void quizComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cambio == false)
            {
                advertenciaCrearNuevaBatería();
            }
        }

        public void completeGaps(int batteryId, int quizId)
        {
            nombreBateria.Text = service.GetNameBatteryById(bateriaId);
            cursoComboBox.SelectedItem = service.getCursoByQuizId(quizId);
            contenidoComboBox.SelectedItem = service.getContenidoByQuizId(quizId);
            quizComboBox.SelectedItem = service.GetNameQuizById(quizId);
            cambio = false;
        }

        private void advertenciaCrearNuevaBatería()
        {
            MessageBoxResult confirmResult = MessageBox.Show(siModificas, atention, MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                guardarCambios.Visibility = Visibility.Visible;
                crearNuevaBateria.Visibility = Visibility.Visible;
                cambio = true;
            }
            else
            {
                cambio = true;
                completeGaps(bateriaId, quizId);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void editQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (cursoComboBox.SelectedItem != null && contenidoComboBox.SelectedItem != null
               && quizComboBox.SelectedItem != null && nombreBateria.Text != null)
            {
                BuscadorPreguntas b = new BuscadorPreguntas(service, personEmail, bateriaId);
                b.ShowDialog();
                Close();
            }
            else
            {
                saltarAlerta();
            }

        }

        private void changeName(object sender, RoutedEventArgs e)
        {
            if (cambio == false)
            {
                advertenciaCrearNuevaBatería();

            }
        }

        private void editQuestionNewBattery_Click(object sender, RoutedEventArgs e)
        {
            if (cursoComboBox.SelectedItem != null && contenidoComboBox.SelectedItem != null
               && quizComboBox.SelectedItem != null && nombreBateria.Text != null)
            {
                service.GetQuestionTypeFromBateria(bateriaId);
                BuscadorPreguntas buscador;
               
                if (service.GetQuestionTypeFromBateria(bateriaId)==0)
                {
                    buscador = new BuscadorPreguntas(service, personEmail, service.CreateBateria(0, service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()), nombreBateria.Text));
                    buscador.ShowDialog();
                    Close();
                }
                else if (service.GetQuestionTypeFromBateria(bateriaId)==1)
                {
                    buscador = new BuscadorPreguntas(service, personEmail, service.CreateBateria(1, service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()), nombreBateria.Text));
                    buscador.ShowDialog();
                    Close();
                }
                else if (service.GetQuestionTypeFromBateria(bateriaId)==2)
                {
                    buscador = new BuscadorPreguntas(service, personEmail, service.CreateBateria(2, service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()), nombreBateria.Text));
                    buscador.ShowDialog();
                    Close();
                }

            }
            else
            {
                saltarAlerta();
            }
        }

        private void saveChanges(object sender, RoutedEventArgs e)
        {
            if (cursoComboBox.SelectedItem != null && contenidoComboBox.SelectedItem != null
               && quizComboBox.SelectedItem != null && nombreBateria.Text != null)
            {
                if (service.GetNameBatteryById(bateriaId) != nombreBateria.Text)
                {
                    service.SetBatteryName(bateriaId, nombreBateria.Text);
                }
                if (quizId != service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()))
                {
                    service.AddBatteryToQuiz(bateriaId, service.GetQuizFromInstructorCourseNameTopicNameAndQuizName(personEmail, cursoComboBox.SelectedItem.ToString(), contenidoComboBox.SelectedItem.ToString(), quizComboBox.SelectedItem.ToString()));
                }
                guardarCambios.Visibility = Visibility.Hidden;
                crearNuevaBateria.Visibility = Visibility.Hidden;
                cambio = false;
            }
            else
            {
                saltarAlerta();
            }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["EditBatTitulo"];
            guardarCambios.Content = Idioma.info["guardarCambios"];
            nameLabel.Content = Idioma.info["nameLabel"];
            cursoLabel.Content = Idioma.info["cursoLabel"];
            topicLabel.Content = Idioma.info["topicLabel"];
            returnButton.Content = Idioma.info["returnButton"];
            editQuestion.Content = Idioma.info["editQuestion"];
            siModificas = Idioma.info["siModificas"];
            atention = Idioma.info["atention"];
            complete = Idioma.info["complete"];
            antesDe = Idioma.info["antesDe"];
            crearNuevaBateria.Content = Idioma.info["crearNuevaBateria"];
            questionVF = Idioma.info["questionVF"];
            questionOpen = Idioma.info["questionOpen"];
            questionMult = Idioma.info["questionMult"];
            questionCor = Idioma.info["questionCor"];
            questionPar = Idioma.info["questionPar"];
        }
    }
}
