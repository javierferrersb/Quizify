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
    /// Lógica de interacción para ListaDeEstudiantesParaPreguntaAbierta.xaml
    /// </summary>
    public partial class ListaDeEstudiantesParaPreguntaAbierta : Page
    {
        private ICollection<String> estudiantes;
        private int quizId;
        private IQuizifyService service;
        private int PreguntaAbiertaId;
        private ICollection<int> RespuestaPreguntaAbierta;
        private string nameStCol = "Nombre";
        private string emailStCol = "Email";

        public ListaDeEstudiantesParaPreguntaAbierta(ICollection<String> listE, ICollection<int> respuestasAbiertaEstudiantes, int questionToOpenId, int qId, IQuizifyService s)
        {
            InitializeComponent();
            actualizar();
            this.estudiantes = listE;
            this.RespuestaPreguntaAbierta = respuestasAbiertaEstudiantes;
            this.service = s;
            this.quizId = qId;
            this.PreguntaAbiertaId = questionToOpenId;
            NombreContenido.Text = service.GetEnunciadoPreguntaById(PreguntaAbiertaId);
            ShowEstudiantes();
            totalRespuestas.Text += " " + RespuestaPreguntaAbierta.Count;
            if (service.GetNotaMediaDePregunta(respuestasAbiertaEstudiantes) == -10.0)
            {
                calificacionMedia.Text += "Todavía no esta corregido";
            }
            else { calificacionMedia.Text += " " + service.GetNotaMediaDePregunta(respuestasAbiertaEstudiantes); }
        }


        private void ShowEstudiantes()
        {
            List<List<string>> list = new();
            service.GetStudentsAttributes(estudiantes, list);

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
            //Cojo respuesta del estudiante y abro PreguntaAbiertaPage
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            int selectedStudentIndex = contentSelected.GetIndex();
            String studentMail = estudiantes.ElementAt(selectedStudentIndex);

            if (service.GetRealizaQuizFromQuizAndStudent(quizId, studentMail) != -1)
            {
                int quizEstudiante = service.GetIdRealizaQuizFromQuizAndStudent(quizId, studentMail);
                //Realizacion
                int respuestaFinalEstudiante = service.GetLastRealizaQuizById((int)quizEstudiante);

                //RespuestaPregunta
                int respuestaAbierta;
                int indiceActual = 0, indicePregunta = 0;
                for (int i = 0; i < service.GetCountRespuestasRealizacionById(respuestaFinalEstudiante); i++)
                {
                    if (service.GetIdPreguntaAsociadaRespuestaRealizacionById(respuestaFinalEstudiante,i) == PreguntaAbiertaId)
                    {
                        indiceActual = indicePregunta;
                    }
                    indicePregunta++;
                }

                respuestaAbierta = service.GetIdRespuestaRealizacionById(respuestaFinalEstudiante,indiceActual);
                PreguntaAbiertaWindow preguntaAbierta = new PreguntaAbiertaWindow(respuestaAbierta, service);
                preguntaAbierta.ShowDialog();
            }
            else
            {
                String studentName = service.GetNombreEstudianteByEmail(studentMail);
                PreguntaAbiertaWindow noHayRespuesta = new PreguntaAbiertaWindow(studentName + " aun no ha contestado la pregunta");
                noHayRespuesta.ShowDialog();
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
            nameStCol = Idioma.info["nameStCol"];
            emailStCol = Idioma.info["emailStCol"];
            totalRespuestas.Text = Idioma.info["totalRespuestas"];
            calificacionMedia.Text = Idioma.info["calificacionMedia"];
        }
    }
}
