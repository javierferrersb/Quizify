using Quizify.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for PreguntaVerdaderoFalsoPage.xaml
    /// </summary>
    public partial class PreguntaVerdaderoFalsoPage : Page
    {
        private IQuizifyService service;
        private int questionId;
        private string studentEmail;
        private int currentRealizacionId;

        public PreguntaVerdaderoFalsoPage(IQuizifyService service, int questionId)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            currentRealizacionId = -1;

            this.questionId = questionId;

            QuestionStatement.Text = service.GetQuestionStatement(questionId);

            studentEmail = "";
            ShowProgressBars(false);
            ShowEditFunctionality();
        }

        public PreguntaVerdaderoFalsoPage(IQuizifyService service, int questionId, int currentRealizacionId, string studentEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.questionId = questionId;
            this.studentEmail = studentEmail;
            this.currentRealizacionId = currentRealizacionId;

            if (service.GetVFStudentAnswer(currentRealizacionId, questionId) == true)
            {
                TrueRadioButton.IsChecked = true;
            }
            else if (service.GetVFStudentAnswer(currentRealizacionId, questionId) == false)
            {
                FalseRadioButton.IsChecked = true;
            }

            ShowProgressBars(false);
            TrueRadioButton.Checked += RadioButton_Checked;
            FalseRadioButton.Checked += RadioButton_Checked;
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
        }
        public PreguntaVerdaderoFalsoPage(int preguntaVerdaderoFalsoId, int respuestaPreguntaId, bool correccion, IQuizifyService ser)
        {
            InitializeComponent();
            actualizar();
            this.service = ser;
            this.questionId = preguntaVerdaderoFalsoId;

            if (service.getVFAnswerByRespuestaPregunta(respuestaPreguntaId) != null)
            {
                if (service.getVFAnswerByRespuestaPregunta(respuestaPreguntaId) == true)
                {
                    TrueRadioButton.IsChecked = true;
                }
                else if (service.getVFAnswerByRespuestaPregunta(respuestaPreguntaId) == false)
                {
                    FalseRadioButton.IsChecked = true;
                }
            }

            ShowProgressBars(false);
            QuestionStatement.Text = service.getStatementByPregunta(preguntaVerdaderoFalsoId);
            if (correccion)
            {
                TrueRadioButton.IsEnabled = false;
                FalseRadioButton.IsEnabled = false;
                TrueRadioButton.Opacity = 100;
                FalseRadioButton.Opacity = 100;
                MostrarRespuestaCorrecta(service.getVFAnswerByRespuestaPregunta(respuestaPreguntaId));
            }
            else
            {
                TrueRadioButton.Checked += RadioButton_Checked;
                FalseRadioButton.Checked += RadioButton_Checked;
            }
        }
        public PreguntaVerdaderoFalsoPage(int preguntaVerdaderoFalsoId, ICollection<int> respuestaPreguntaVFIDs, bool correccion, IQuizifyService service)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.questionId = preguntaVerdaderoFalsoId;

            int numRespuestasV = getRespuestasV(respuestaPreguntaVFIDs);
            int numRespuestasF = getRespuestasF(respuestaPreguntaVFIDs);
            int todasRespuestas = numRespuestasV + numRespuestasF;

            answersVDistribution.Maximum = todasRespuestas;
            answersFDistribution.Maximum = todasRespuestas;
            answersVDistribution.Value = numRespuestasV;
            answersFDistribution.Value = numRespuestasF;

            if (todasRespuestas != 0)
            {
                EstudiantesVerdadero.Text = numRespuestasV + "/" + todasRespuestas + " (" + (numRespuestasV * 100 / todasRespuestas) + "%)";
                EstudiantesFalso.Text = numRespuestasF + "/" + todasRespuestas + " (" + (numRespuestasF * 100 / todasRespuestas) + "%)";
            }
            else
            {
                EstudiantesVerdadero.Text = "0";
                EstudiantesFalso.Text = "0";
            }


            ShowProgressBars(true);
            QuestionStatement.Text = service.getStatementByPregunta(preguntaVerdaderoFalsoId);
            if (correccion)
            {
                TrueRadioButton.IsEnabled = false;
                FalseRadioButton.IsEnabled = false;
                TrueRadioButton.Opacity = 100;
                FalseRadioButton.Opacity = 100;
            }
        }
        private void MostrarRespuestaCorrecta(bool? studentAnswer)
        {
            bool correctAnswer = service.GetVFCorrectAnswer(questionId);

            if (studentAnswer == true)
            {
                if (correctAnswer == true)
                {
                    TrueRadioButton.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    FalseRadioButton.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else if (studentAnswer == false)
            {
                if (correctAnswer == false)
                {
                    FalseRadioButton.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    TrueRadioButton.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                if (correctAnswer == false)
                {
                    FalseRadioButton.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    TrueRadioButton.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
        }



        private void EditStatementHandler(object sender, RoutedEventArgs e)
        {
            EditStatementField.Text = service.GetQuestionStatement(questionId);
            ShowEditStatementField();
            EditStatementField.Focus();
        }

        private void SaveStatementHandler(object sender, RoutedEventArgs e)
        {
            service.SetQuestionStatement(questionId, EditStatementField.Text);
            QuestionStatement.Text = EditStatementField.Text;

            HideEditStatementField();
        }

        private void EditValueHandler(object sender, RoutedEventArgs e)
        {
            TrueRadioButton.IsEnabled = true;
            FalseRadioButton.IsEnabled = true;

            EditValueButton.IsEnabled = false;
            EditValueButton.Visibility = Visibility.Hidden;

            SaveValueButton.Visibility = Visibility.Visible;
            SaveValueButton.IsEnabled = true;

            EditStatementButton.IsEnabled = false;
        }

        private void SaveValueHandler(object sender, RoutedEventArgs e)
        {
            service.SetVFCorrectAnswer(questionId, (bool)TrueRadioButton.IsChecked);

            TrueRadioButton.IsEnabled = false;
            FalseRadioButton.IsEnabled = false;

            EditValueButton.IsEnabled = true;
            EditValueButton.Visibility = Visibility.Visible;

            SaveValueButton.Visibility = Visibility.Hidden;
            SaveValueButton.IsEnabled = false;

            EditStatementButton.IsEnabled = true;
        }

        private void ShowEditStatementField()
        {
            EditStatementField.IsEnabled = true;
            EditStatementField.Visibility = Visibility.Visible;

            EditStatementButton.IsEnabled = false;
            EditStatementButton.Visibility = Visibility.Hidden;

            SaveStatementButton.Visibility = Visibility.Visible;
            SaveStatementButton.IsEnabled = true;

            QuestionStatement.Visibility = Visibility.Hidden;
            QuestionStatement.IsEnabled = false;

            EditValueButton.IsEnabled = false;
        }

        private void HideEditStatementField()
        {
            EditStatementField.IsEnabled = false;
            EditStatementField.Visibility = Visibility.Hidden;

            EditStatementButton.IsEnabled = true;
            EditStatementButton.Visibility = Visibility.Visible;

            SaveStatementButton.Visibility = Visibility.Hidden;
            SaveStatementButton.IsEnabled = false;

            QuestionStatement.Visibility = Visibility.Visible;
            QuestionStatement.IsEnabled = true;

            EditValueButton.IsEnabled = true;
        }

        private void ShowEditFunctionality()
        {

            QuestionStatement.SetValue(Grid.ColumnSpanProperty, 1);
            TrueRadioButton.SetValue(Grid.ColumnSpanProperty, 1);
            FalseRadioButton.SetValue(Grid.ColumnSpanProperty, 1);

            TrueRadioButton.IsEnabled = false;
            FalseRadioButton.IsEnabled = false;

            if (service.GetVFCorrectAnswer(questionId) == true)
            {
                TrueRadioButton.IsChecked = true;
            }
            else
            {
                FalseRadioButton.IsChecked = true;
            }

            EditStatementButton.Visibility = Visibility.Visible;
            EditStatementButton.IsEnabled = true;

            EditValueButton.Visibility = Visibility.Visible;
            EditValueButton.IsEnabled = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (TrueRadioButton.IsChecked == true)
            {
                service.SetVFStudentQuestionAnswer(currentRealizacionId, questionId, true);
            }
            else
            {
                service.SetVFStudentQuestionAnswer(currentRealizacionId, questionId, false);
            }
        }

        private int getRespuestasV(ICollection<int> respuestaPreguntasVF)
        {
            int res = 0;
            foreach (int respuestaPreguntaVF in respuestaPreguntasVF)
            {
                if (service.getVFAnswerByRespuestaPregunta(respuestaPreguntaVF) == true)
                {
                    res++;
                }
            }
            return res;
        }
        private int getRespuestasF(ICollection<int> respuestaPreguntasVF)
        {
            int res = 0;
            foreach (int respuestaPreguntaVF in respuestaPreguntasVF)
            {
                if (service.getVFAnswerByRespuestaPregunta(respuestaPreguntaVF) == false)
                {
                    res++;
                }
            }
            return res;
        }

        private void ShowProgressBars(bool show)
        {
            answersVDistribution.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            answersFDistribution.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            EstudiantesVerdadero.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            EstudiantesFalso.Visibility = show ? Visibility.Visible : Visibility.Hidden;
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            TrueRadioButton.Content = Idioma.info["TrueRadioButton"];
            FalseRadioButton.Content = Idioma.info["FalseRadioButton"];
            EditStatementButton.ToolTip = Idioma.info["EditStatementButton"];
            SaveStatementButton.ToolTip = Idioma.info["SaveStatementButton"];
            EditValueButton.ToolTip = Idioma.info["EditValueButton"];
            SaveValueButton.ToolTip = Idioma.info["SaveValueButton"];
        }
    }
}
