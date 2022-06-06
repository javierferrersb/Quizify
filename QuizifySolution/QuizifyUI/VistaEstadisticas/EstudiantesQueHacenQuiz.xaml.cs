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
    /// Lógica de interacción para AlumnosQueHacenQuiz.xaml
    /// </summary>
    public partial class EstudiantesQueHacenQuiz : Window
    {
        private int quizId;
        private ICollection<String> estudiantesIDs;
        private IQuizifyService service;
        private int QuizNoHecho = 0, QuizEnPausa = 0, QuizTerminado = 0;
        private string estudiantesNoQuiz = "Estudiantes que no han empezado el quiz: ";
        private string estudiantesPausaQuiz = "\nEstudiantes que han pausado el quiz: ";
        private string estudiantesFinQuiz = "\nEstudiantes que han terminado el quiz: ";
        private string nameStCol = "Nombre";
        private string emailStCol = "Email";
        private string quizYaTerminado = " ya ha terminado el examen";
        private string quizTerminado = "Examen terminado";
        private string realizandoQuiz = " está realizando el examen";
        private string quizRealizandose = "Examen realizándose";
        private string sinRealizarQuiz = " aun no ha realizado el examen";
        private string quizNoRealizado = "Examen no realizado";
        public EstudiantesQueHacenQuiz(ICollection<String> estudiantes, int qId, IQuizifyService ser)
        {
            InitializeComponent();
            this.service = ser;
            this.quizId = qId;
            this.estudiantesIDs = estudiantes;

            NombreContenido.Text = service.GetNameQuizById(quizId);

            actualizar();
            foreach (String est in estudiantesIDs)
            {
                //RealizaQuiz
                if (service.GetRealizaQuizFromQuizAndStudent(quizId, est) == -1)
                {
                    QuizNoHecho++;
                }
                else
                {
                    int? quizEstudianteRealizaQuiz = service.GetIdRealizaQuizFromQuizAndStudent(quizId, est);
                    if (service.GetTerminadoLastRealizaQuizById((int)quizEstudianteRealizaQuiz))
                    {
                        QuizTerminado++;
                    }
                    else if (!service.GetTerminadoLastRealizaQuizById((int)quizEstudianteRealizaQuiz))
                    {
                        QuizEnPausa++;
                    }

                }
            }
            QuizzesHechos.Text = estudiantesNoQuiz + QuizNoHecho
                    + estudiantesPausaQuiz + QuizEnPausa
                    + estudiantesFinQuiz + QuizTerminado;

            ShowEstudiantes();
        }

        private void ShowEstudiantes()
        {
            List<List<string>> list = new();
            service.GetStudentsAttributes(estudiantesIDs, list);

            CreateStudentsColumns();
            foreach (List<String> listOfStudentInfo in list)
            {
                StudentItem item = (new StudentItem
                {
                    Email = listOfStudentInfo[1],
                    Name = listOfStudentInfo[0],
                });
                GridEstudiantes.Items.Add(item);
            }
        }

        private void CreateStudentsColumns()
        {
            GridEstudiantes.Columns.Clear();
            GridEstudiantes.Items.Clear();

            System.Windows.Controls.DataGridTextColumn nameCol = new System.Windows.Controls.DataGridTextColumn();
            System.Windows.Controls.DataGridTextColumn emailCol = new System.Windows.Controls.DataGridTextColumn();

            nameCol.Header = nameStCol;
            emailCol.Header = emailStCol;

            nameCol.IsReadOnly = true;
            emailCol.IsReadOnly = true;

            nameCol.Binding = new Binding("Name");
            emailCol.Binding = new Binding("Email");

            GridEstudiantes.Columns.Add(nameCol);
            GridEstudiantes.Columns.Add(emailCol);

            GridEstudiantes.MouseDoubleClick += GridEstudiantes_MouseDoubleClick;
        }



        private void GridEstudiantes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Al hacer doble_click sobre el estudiante, se debe abrir la realización de este estudiante del examen que hizo
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            int indexSelectedStudent = contentSelected.GetIndex();
            String studentMail = estudiantesIDs.ElementAt(indexSelectedStudent);

            if (service.GetRealizaQuizFromQuizAndStudent(quizId, studentMail) != -1)
            {
                int? realizaEstudiante = service.GetIdRealizaQuizFromQuizAndStudent(quizId, studentMail);
                bool condicionEsTerminado = service.GetTerminadoLastRealizaQuizById((int)realizaEstudiante);
                int quizEstudianteRealizacion;
                string MessageInBox = "", MessageInHeader = "";
                if (condicionEsTerminado)
                {
                    MessageInBox = service.GetNombreEstudianteByEmail(studentMail) + quizYaTerminado;
                    MessageInHeader = quizTerminado;
                    quizEstudianteRealizacion = service.GetLastRealizaQuizById((int)realizaEstudiante);
                }
                else
                {
                    MessageInBox = service.GetNombreEstudianteByEmail(studentMail) + realizandoQuiz;
                    MessageInHeader = quizRealizandose;
                    quizEstudianteRealizacion = service.GetLastRealizaQuizById((int)realizaEstudiante);
                }
                MessageBox.Show(MessageInBox, MessageInHeader, MessageBoxButton.OK);
                VerEstadisticasInstructor verExamen = new VerEstadisticasInstructor(quizId, studentMail, quizEstudianteRealizacion, service);
                verExamen.ShowDialog();
            }
            else
            {
                MessageBox.Show(service.GetNombreEstudianteByEmail(studentMail) + sinRealizarQuiz, quizNoRealizado, MessageBoxButton.OK);
            }
        }

        public struct StudentItem
        {
            public String Email { get; set; }
            public String Name { get; set; }
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["EstudiantesQuizTitulo"];
            estudiantesNoQuiz = Idioma.info["estudiantesNoQuiz"];
            estudiantesPausaQuiz = Idioma.info["estudiantesPausaQuiz"];
            estudiantesFinQuiz = Idioma.info["estudiantesFinQuiz"];
            nameStCol = Idioma.info["nameStCol"];
            emailStCol = Idioma.info["emailStCol"];
            quizYaTerminado = Idioma.info["quizYaTerminado"];
            quizTerminado = Idioma.info["quizTerminado"];
            realizandoQuiz = Idioma.info["realizandoQuiz"];
            quizRealizandose = Idioma.info["quizRealizandose"];
            sinRealizarQuiz = Idioma.info["sinRealizarQuiz"];
            quizNoRealizado = Idioma.info["quizNoRealizado"];
        }
    }
}
