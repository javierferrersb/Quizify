using Quizify.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para AsignarPesoEvaluacion.xaml
    /// </summary>
    public partial class AsignarPesoEvaluacion : Window
    {
        private int contenidoId;
        private int quizId;
        private IQuizifyService service;
        private double sumaTotal;
        public double restante;
        private string supera100 = "No puede superar el 100% en el total de la nota";
        private string sinNotaNegativa = "No puedes introducir nota negativa";
        private string elementosNoNumericos = "No puedes introducir elementos no númericos";
        public AsignarPesoEvaluacion(IQuizifyService service, int contenidoId, int quizId)
        {
            this.quizId = quizId;
            this.contenidoId = contenidoId;
            this.service = service;
            InitializeComponent();
            actualizar();
            showAll();
            quizMark.Text = "" + service.GetNotaQuizById(quizId);
            otherMark.Text = "30";
            markBox.Text = "" + service.GetNotaQuizById(quizId);
            restante = (Double.Parse(otherMark.Text));
        }

        public void showAll()
        {
            sumaTotal = service.getNotaRestoQuizes(contenidoId);
            sumaTotal += 30.0;
            totalMark.Text = sumaTotal.ToString();
            sumaTotal -= 30.0;
            sumaTotal = sumaTotal - service.GetNotaQuizById(quizId);
            otherQuizMark.Text = sumaTotal.ToString();
        }

        private void save_click(object sender, RoutedEventArgs e)
        {
            service.SetQuizWeight(quizId, Double.Parse(quizMark.Text));
            Close();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void setMark_Click(object sender, KeyEventArgs e)
        {
            try
            {
                quizMark.Text = markBox.Text;
                if (quizMark.Text != "")
                {
                    if ((restante + Double.Parse(quizMark.Text) + Double.Parse(otherQuizMark.Text)) > 100)
                    {
                        MessageBoxResult confirmResult = MessageBox.Show("Error", supera100, MessageBoxButton.YesNo);
                        setMarkAndQuiz(confirmResult);
                    }

                    if (Double.Parse(quizMark.Text) < 0)
                    {
                        MessageBoxResult confirmResult = MessageBox.Show(sinNotaNegativa, "Error", MessageBoxButton.YesNo);
                        setMarkAndQuiz(confirmResult);
                    }
                    totalMark.Text = "" + (restante + Double.Parse(quizMark.Text) + Double.Parse(otherQuizMark.Text));
                }
            }
            catch (Exception)
            {
                MessageBoxResult confirmResult = MessageBox.Show(elementosNoNumericos, "Error", MessageBoxButton.YesNo);
                setMarkAndQuiz(confirmResult);
                totalMark.Text = "" + (restante + Double.Parse(quizMark.Text) + Double.Parse(otherQuizMark.Text));
            }
        }

        private void setMarkAndQuiz(MessageBoxResult msg)
        {
            if (msg == MessageBoxResult.Yes)
            {
                markBox.Text = "" + service.GetNotaQuizById(quizId);
                quizMark.Text = "" + service.GetNotaQuizById(quizId);
            }
            else
            {
                markBox.Text = "" + service.GetNotaQuizById(quizId);
                quizMark.Text = "" + service.GetNotaQuizById(quizId);
            }
        }

        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["PesoEvalTitulo"];
            subjectLabel.Content = Idioma.info["subjectLabel"];
            valueLabel.Content = Idioma.info["valueLabel"];
            OtherQuizLabel.Content = Idioma.info["OtherQuizLabel"];
            restLabel.Content = Idioma.info["restLabel"];
            exitMarkMenu.Content = Idioma.info["exitMarkMenu"];
            saveMark.Content = Idioma.info["saveMark"];
            supera100 = Idioma.info["supera100"];
            sinNotaNegativa = Idioma.info["sinNotaNegativa"];
            elementosNoNumericos = Idioma.info["elementosNoNumericos"];
        }
    }
}
