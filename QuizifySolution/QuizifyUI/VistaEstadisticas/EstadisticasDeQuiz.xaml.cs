using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para EstadisticasDeQuiz.xaml
    /// </summary>
    public partial class EstadisticasDeQuiz : Window
    {
        private IQuizifyService service;
        private int quizId;
        private QuizItem quiz;
        private ICollection<double> todasNotas;
        ICollection<double> col01;
        ICollection<double> col12;
        ICollection<double> col23;
        ICollection<double> col34;
        ICollection<double> col45;
        ICollection<double> col56;
        ICollection<double> col67;
        ICollection<double> col78;
        ICollection<double> col89;
        ICollection<double> col90;


        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public EstadisticasDeQuiz(IQuizifyService s, QuizItem q, int qId)
        {
            InitializeComponent();
            this.service = s;
            this.quizId = qId;
            this.quiz = q;
            QuizNameLabel.Text = quiz.Name;
            actualizar();
            EnviosLabel.Text += " " + service.GetNumberOfRealizaciones(qId, q.CourseId);
            MediaLabel.Text += " " +  service.GetAverageMarkOfQuiz(qId, q.CourseId);
            DesviacionLabel.Text += " " + service.GetStandardDeviation(qId, q.CourseId);
            xAxis.Title = Idioma.info["xAxis"];
            yAxis.Title = Idioma.info["yAxis"];

            todasNotas = service.GetAllMarks(q.Id, q.CourseId);
            col01 = new List<double>();
            col12 = new List<double>();
            col23 = new List<double>();
            col34 = new List<double>();
            col45 = new List<double>();
            col56 = new List<double>();
            col67 = new List<double>();
            col78 = new List<double>();
            col89 = new List<double>();
            col90 = new List<double>();
            RepartirValores();

            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(new ColumnSeries
            {
                Title = Idioma.info["NotasB"],
                Values = new ChartValues<double> {  col01.Count,
                                                    col12.Count,
                                                    col23.Count,
                                                    col34.Count,
                                                    col45.Count,
                                                    0,0,0,0,0}
            }); ;
            SeriesCollection.Add(new ColumnSeries
            {
                Title = Idioma.info["NotasA"],
                Values = new ChartValues<double> {  0,0,0,0,0,
                                                    col56.Count,
                                                    col67.Count,
                                                    col78.Count,
                                                    col89.Count,
                                                    col90.Count }
            }); ;

            Labels = new[] { "0-1", "1-2", "2-3", "3-4", "4-5", "5-6", "6-7", "7-8", "8-9", "9-10" };
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        private void RepartirValores()
        {
            foreach(double nota in todasNotas)
            {
                if (nota < 1.0)
                {
                    col01.Add(nota);
                }else if (nota < 2.0)
                {
                    col12.Add(nota);
                }else if (nota < 3.0)
                {
                    col23.Add(nota);
                }else if (nota < 4.0){
                    col34.Add(nota);
                }else if (nota < 5.0)
                {
                    col45.Add(nota);
                }else if (nota < 6.0)
                {
                    col56.Add(nota);
                }else if (nota < 7.0)
                {
                    col67.Add(nota);
                }else if (nota < 8.0)
                {
                    col78.Add(nota);
                }else if (nota < 9.0)
                {
                    col89.Add(nota);
                }else
                {
                    col90.Add(nota);
                }
            }
        }

        private void AbrirEstadisticas(object sender, RoutedEventArgs e)
        {
            VerEstadisticasInstructor verEstadisticasInstructor = new VerEstadisticasInstructor(quiz.Id, service.GetCourseOfTopic(service.GetTopicFromQuiz(quiz.Id)), service);
            verEstadisticasInstructor.ShowDialog();
        }

        private void actualizar()
        {
            //Calcular número envíos, media de notas y desviación estándar(?)
            EnviosLabel.Text = Idioma.info["EnviosLabel"];
            MediaLabel.Text = Idioma.info["MediaLabel"];
            DesviacionLabel.Text = Idioma.info["DesviacionLabel"];
            VerEstadísticasPreguntas.Content = Idioma.info["VerEstadísticasPreguntas"];
        }

    }
}
