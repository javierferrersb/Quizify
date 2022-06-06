using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para PreguntaAbiertaWindow.xaml
    /// </summary>
    public partial class PreguntaAbiertaWindow : Window
    {
        private IQuizifyService service;
        private int preguntaAsociada;


        public PreguntaAbiertaWindow(int respuestaPreguntaAbierta, IQuizifyService ser)
        {
            InitializeComponent();
            actualizar();
            this.service = ser;
            this.preguntaAsociada = service.GetIdPreguntaAsociadaRespuestaPreguntaAbiertaById(respuestaPreguntaAbierta);


            QuestionStatement.Text = service.GetStatmentRespuestaPreguntaAbiertaById(respuestaPreguntaAbierta);
            AnswerField.IsEnabled = false;
            AnswerField.Text = service.GetRespuestOfRespuestaPreguntaAbiertaById(respuestaPreguntaAbierta);
        }

        public PreguntaAbiertaWindow(String noHaRealizadoQuiz)
        {
            InitializeComponent();
            actualizar();
            QuestionStatement.Text = noHaRealizadoQuiz;
            AnswerField.IsEnabled = false;
            AnswerField.Text = "";
            this.service = null;
            this.preguntaAsociada = 0;
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["PregAbiertaTitulo"];
            HintAssist.SetHint(AnswerField, Idioma.info["AnswerField"]);
        }
    }
}
