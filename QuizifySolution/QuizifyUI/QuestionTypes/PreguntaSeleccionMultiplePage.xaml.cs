using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Quizify.Services;
namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for PreguntaSeleccionMultiple.xaml
    /// </summary>
    public partial class PreguntaSeleccionMultiplePage : Page
    {
        private IQuizifyService service;
        private int questionId;
        private string studentEmail;
        private int currentRealizacionId;
        private bool sePuedenvariasCorrectas = true;
        private bool modifyQuestion;
        private ICollection<RadioButton> opciones;
        private ICollection<TextBlock> opText;
        public PreguntaSeleccionMultiplePage()
        {
            InitializeComponent();
            String[] en = new string[8];
            en[0] = "Opcion A";
            en[1] = "Opcion B";
            en[2] = "Opcion C";
            en[3] = "Opcion D";
            en[4] = "Opcion E";
            en[5] = "Opcion F";
            en[6] = "Opcion G";
            en[7] = "Opcion H";
            CreateRadioButtons(en);
        }
        public void CreateRadioButtons(String[] enunciados)
        {
            for (int i = 0; i < enunciados.Length; i++)
            {
                RowDefinition row = new();
                row.Height = new GridLength(1, GridUnitType.Star);
                GridOpciones.RowDefinitions.Add(row);
            }
            for (int i = 0; i < enunciados.Length; i++)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Pixel);
                ColumnDefinition cd2 = new();
                cd2.Width = new GridLength(4, GridUnitType.Star);
                g.ColumnDefinitions.Add(cd);
                g.ColumnDefinitions.Add(cd2);

                if (sePuedenvariasCorrectas)
                {
                    CheckBox cb = new();
                    g.Children.Add(cb);
                    Grid.SetColumn(cb, 0);
                }
                else
                {
                    RadioButton rb = new();
                    rb.GroupName = "Answers";
                    g.Children.Add(rb);
                    Grid.SetColumn(rb, 0);
                }
                

                TextBlock tb = new();
                tb.Text = enunciados[i];
                tb.VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(tb);
                Grid.SetColumn(tb, 1);

                GridOpciones.Children.Add(g);
                Grid.SetRow(g, i);
            }
        }
        public PreguntaSeleccionMultiplePage(IQuizifyService service, int questionId)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            currentRealizacionId = -1;
            
            this.questionId = questionId;

            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            string[] optionsText = service.GetSeleccionTextOptions(questionId).ToArray();
            if (optionsText.Length == 4)
            {
                textOp1.Text = optionsText[0];
                textOp2.Text = optionsText[1];
                textOp3.Text = optionsText[2];
                textOp4.Text = optionsText[3];
            }
            textOp1.Opacity = 100;
            textOp2.Opacity = 100;
            textOp3.Opacity = 100;
            textOp4.Opacity = 100;
            ShowCheckBox(true);
            bool[] correctAnswer = service.GetSeleccionCorrectAnswer(questionId).ToArray();
            if(correctAnswer[0] == true)
            {
                checkOp1.IsChecked = true;
            }
            if (correctAnswer[1] == true)
            {
                checkOp2.IsChecked = true;
            }
            if (correctAnswer[2] == true)
            {
                checkOp3.IsChecked = true;
            }
            if (correctAnswer[3] == true)
            {
                checkOp4.IsChecked = true;
            }
            checkOp1.IsEnabled = false;
            checkOp2.IsEnabled = false;
            checkOp3.IsEnabled = false;
            checkOp4.IsEnabled = false;

            studentEmail = "";
            modifyQuestion = true;
            ShowProgressBars(false);
            ShowEditFunctionality();
        }

        public PreguntaSeleccionMultiplePage(IQuizifyService service, int questionId, int currentRealizacionId, string studentEmail)
        {
            InitializeComponent();
            actualizar();
            ShowProgressBars(false);
            ShowCheckBox(false);
            RemoveSelectionButton.Visibility = Visibility.Visible;
            this.service = service;
            this.questionId = questionId;
            this.studentEmail = studentEmail;
            this.currentRealizacionId = currentRealizacionId;
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            string[] optionsText = service.GetSeleccionTextOptions(questionId).ToArray();
            if (optionsText.Length == 4)
            {
                textOp1.Text = optionsText[0];
                textOp2.Text = optionsText[1];
                textOp3.Text = optionsText[2];
                textOp4.Text = optionsText[3];
            }
            Option1.IsEnabled = true;
            Option2.IsEnabled = true;
            Option3.IsEnabled = true;
            Option4.IsEnabled = true;
            modifyQuestion = false;
            int selectedOption = service.GetSeleccionStudentAnswer(currentRealizacionId, questionId);
            if(selectedOption == 0)
            {
                Option1.IsChecked = true;
            }
            else if(selectedOption == 1)
            {
                Option2.IsChecked = true;
            }
            else if(selectedOption == 2)
            {
                Option3.IsChecked = true;
            }
            else if(selectedOption == 3)
            {
                Option4.IsChecked = true;
            }
            Option1.Checked += RadioButton_Checked;
            Option2.Checked += RadioButton_Checked;
            Option3.Checked += RadioButton_Checked;
            Option4.Checked += RadioButton_Checked;
            //...
        }

        public PreguntaSeleccionMultiplePage(int preguntaSeleccionId, int respuestaPreguntaId, IQuizifyService ser)
        {
            InitializeComponent();
            actualizar();
            ShowCheckBox(false);
            this.service = ser;
            this.questionId = preguntaSeleccionId;
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            string[] optionsText = service.GetSeleccionTextOptions(questionId).ToArray();
            if (optionsText.Length == 4)
            {
                textOp1.Text = optionsText[0];
                textOp2.Text = optionsText[1];
                textOp3.Text = optionsText[2];
                textOp4.Text = optionsText[3];
            }
            modifyQuestion = false;
            int answer = service.GetSelectionAnswerFromAnswerId(respuestaPreguntaId);
            if (answer == 0)
            {
                Option1.IsChecked = true;
            }
            else if (answer == 1)
            {
                Option2.IsChecked = true;
            }
            else if (answer == 2)
            {
                Option3.IsChecked = true;
            }
            else if (answer == 3)
            {
                Option4.IsChecked = true;
            }
            ShowProgressBars(false);

            Option1.IsEnabled = false;
            Option2.IsEnabled = false;
            Option3.IsEnabled = false;
            Option4.IsEnabled = false;

            Option1.Opacity = 100;
            Option2.Opacity = 100;
            Option3.Opacity = 100;
            Option4.Opacity = 100;

            textOp1.Opacity = 100;
            textOp2.Opacity = 100;
            textOp3.Opacity = 100;
            textOp4.Opacity = 100;

            MostrarRespuestaCorrecta(answer);
        }

        public PreguntaSeleccionMultiplePage(int preguntaSeleccionId, ICollection<int> respuestaPreguntaSeleccionIDs, IQuizifyService service)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.questionId = preguntaSeleccionId;

            string[] optionsText = service.GetSeleccionTextOptions(questionId).ToArray();
            if (optionsText.Length == 4)
            {
                textOp1.Text = optionsText[0];
                textOp2.Text = optionsText[1];
                textOp3.Text = optionsText[2];
                textOp4.Text = optionsText[3];
            }

            int numRespuestas1 = getRespuestasOption(respuestaPreguntaSeleccionIDs, 0);
            int numRespuestas2 = getRespuestasOption(respuestaPreguntaSeleccionIDs, 1);
            int numRespuestas3 = getRespuestasOption(respuestaPreguntaSeleccionIDs, 2);
            int numRespuestas4 = getRespuestasOption(respuestaPreguntaSeleccionIDs, 3);
            int todasRespuestas = numRespuestas1 + numRespuestas2 + numRespuestas3 + numRespuestas4;

            answersOp1Distribution.Maximum = todasRespuestas;
            answersOp2Distribution.Maximum = todasRespuestas;
            answersOp3Distribution.Maximum = todasRespuestas;
            answersOp4Distribution.Maximum = todasRespuestas;
            answersOp1Distribution.Value = numRespuestas1;
            answersOp2Distribution.Value = numRespuestas2;
            answersOp3Distribution.Value = numRespuestas3;
            answersOp4Distribution.Value = numRespuestas4;

            if (todasRespuestas != 0)
            {
                EstudiantesOp1.Text = numRespuestas1 + "/" + todasRespuestas + " (" + (numRespuestas1 * 100 / todasRespuestas) + "%)";
                EstudiantesOp2.Text = numRespuestas2 + "/" + todasRespuestas + " (" + (numRespuestas2 * 100 / todasRespuestas) + "%)";
                EstudiantesOp3.Text = numRespuestas3 + "/" + todasRespuestas + " (" + (numRespuestas3 * 100 / todasRespuestas) + "%)";
                EstudiantesOp4.Text = numRespuestas4 + "/" + todasRespuestas + " (" + (numRespuestas4 * 100 / todasRespuestas) + "%)";
            }
            else
            {
                EstudiantesOp1.Text = "0";
                EstudiantesOp2.Text = "0";
                EstudiantesOp3.Text = "0";
                EstudiantesOp4.Text = "0";
            }
            
            ShowProgressBars(true);
            ShowCheckBox(false);
            modifyQuestion = false;
            QuestionStatement.Text = service.getStatementByPregunta(preguntaSeleccionId);
            Option1.IsEnabled = false;
            Option2.IsEnabled = false;
            Option3.IsEnabled = false;
            Option4.IsEnabled = false;
            Option1.Opacity = 100;
            Option2.Opacity = 100;
            Option3.Opacity = 100;
            Option4.Opacity = 100;
            textOp1.Opacity = 100;
            textOp2.Opacity = 100;
            textOp3.Opacity = 100;
            textOp4.Opacity = 100;
        }
        private void MostrarRespuestaCorrecta(int studentAnswer)
        {
            bool correcta = false;
            bool[] correctAnswer = service.GetSeleccionCorrectAnswer(questionId).ToArray();
            if(studentAnswer >= 0)
            {
                if (correctAnswer[studentAnswer] == true) correcta = true;
                if (correcta)
                {
                    if (correctAnswer[0] == true)
                    {
                        Option1.Foreground = new SolidColorBrush(Colors.Green);
                        textOp1.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    if (correctAnswer[1] == true)
                    {
                        Option2.Foreground = new SolidColorBrush(Colors.Green);
                        textOp2.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    if (correctAnswer[2] == true)
                    {
                        Option3.Foreground = new SolidColorBrush(Colors.Green);
                        textOp3.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    if (correctAnswer[3] == true)
                    {
                        Option4.Foreground = new SolidColorBrush(Colors.Green);
                        textOp4.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }
                else
                {
                    if (correctAnswer[0] == false)
                    {
                        Option1.Foreground = new SolidColorBrush(Colors.Red);
                        textOp1.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    if (correctAnswer[1] == false)
                    {
                        Option2.Foreground = new SolidColorBrush(Colors.Red);
                        textOp2.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    if (correctAnswer[2] == false)
                    {
                        Option3.Foreground = new SolidColorBrush(Colors.Red);
                        textOp3.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    if (correctAnswer[3] == false)
                    {
                        Option4.Foreground = new SolidColorBrush(Colors.Red);
                        textOp4.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }
            else
            {
                if (correctAnswer[0] == true)
                {
                    Option1.Foreground = new SolidColorBrush(Colors.Green);
                    textOp1.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (correctAnswer[1] == true)
                {
                    Option2.Foreground = new SolidColorBrush(Colors.Green);
                    textOp2.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (correctAnswer[2] == true)
                {
                    Option3.Foreground = new SolidColorBrush(Colors.Green);
                    textOp3.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (correctAnswer[3] == true)
                {
                    Option4.Foreground = new SolidColorBrush(Colors.Green);
                    textOp4.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Option1.IsChecked == true) service.SetSeleccionStudentAnswer(currentRealizacionId, questionId, 0);
            else if (Option2.IsChecked == true) service.SetSeleccionStudentAnswer(currentRealizacionId, questionId, 1);
            else if (Option3.IsChecked == true) service.SetSeleccionStudentAnswer(currentRealizacionId, questionId, 2);
            else if (Option4.IsChecked == true) service.SetSeleccionStudentAnswer(currentRealizacionId, questionId, 3);
            else service.SetSeleccionStudentAnswer(currentRealizacionId, questionId, -1);
        }
        private void ShowProgressBars(bool show)
        {
            answersOp1Distribution.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            answersOp2Distribution.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            answersOp3Distribution.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            answersOp4Distribution.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            EstudiantesOp1.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            EstudiantesOp2.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            EstudiantesOp3.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            EstudiantesOp4.Visibility = show ? Visibility.Visible : Visibility.Hidden;
        }

        private void ShowCheckBox(bool show)
        {
            checkOp1.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            checkOp2.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            checkOp3.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            checkOp4.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            Option1.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            Option2.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            Option3.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            Option4.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
        }

        private void ShowTextBox(bool show)
        {
            textOp1.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            textOp2.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            textOp3.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            textOp4.Visibility = !show ? Visibility.Visible : Visibility.Hidden;
            op1Box.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            op2Box.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            op3Box.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            op4Box.Visibility = show ? Visibility.Visible : Visibility.Hidden;
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
            //  TrueRadioButton.IsEnabled = true;
            //  FalseRadioButton.IsEnabled = true;
            checkOp1.IsEnabled = true;
            checkOp2.IsEnabled = true;
            checkOp3.IsEnabled = true;
            checkOp4.IsEnabled = true;
            
            op1Box.Text = textOp1.Text;
            op2Box.Text = textOp2.Text;
            op3Box.Text = textOp3.Text;
            op4Box.Text = textOp4.Text;

            ShowTextBox(true);

            EditValueButton.IsEnabled = false;
            EditValueButton.Visibility = Visibility.Hidden;

            SaveValueButton.Visibility = Visibility.Visible;
            SaveValueButton.IsEnabled = true;

            EditStatementButton.IsEnabled = false;
        }

        private void RemoveButton_Clicked(object sender, RoutedEventArgs e)
        {
            Option1.IsChecked = false;
            Option2.IsChecked = false;
            Option3.IsChecked = false;
            Option4.IsChecked = false;
        }

        private void SaveValueHandler(object sender, RoutedEventArgs e)
        {
            // service.SetVFCorrectAnswer(questionId, (bool)TrueRadioButton.IsChecked);
            service.SetSeleccionCorrectAnswer(questionId, (bool)checkOp1.IsChecked, (bool)checkOp2.IsChecked, (bool)checkOp3.IsChecked, (bool)checkOp4.IsChecked);
            service.SetSeleccionTextOptions(questionId, op1Box.Text, op2Box.Text, op3Box.Text, op4Box.Text);
            checkOp1.IsEnabled = false;
            checkOp2.IsEnabled = false;
            checkOp3.IsEnabled = false;
            checkOp4.IsEnabled = false;

            textOp1.Text = op1Box.Text;
            textOp2.Text = op2Box.Text;
            textOp3.Text = op3Box.Text;
            textOp4.Text = op4Box.Text;

            ShowTextBox(false);

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
        private int getRespuestasOption(ICollection<int> respuestaPreguntasSeleccion, int option)
        {
            int res = 0;
            foreach (int respuestaPreguntaSeleccion in respuestaPreguntasSeleccion)
            {
                if (service.GetSelectionAnswerFromAnswerId(respuestaPreguntaSeleccion) == option)
                {
                    res++;
                }
            }
            return res;
        }
        private void ShowEditFunctionality()
        {

            QuestionStatement.SetValue(Grid.ColumnSpanProperty, 1);
            /*TrueRadioButton.SetValue(Grid.ColumnSpanProperty, 1);
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
            }*/

            EditStatementButton.Visibility = Visibility.Visible;
            EditStatementButton.IsEnabled = true;

            EditValueButton.Visibility = Visibility.Visible;
            EditValueButton.IsEnabled = true;
        }
        private void actualizar()
        {
            RemoveSelectionButton.Content = Idioma.info["RemoveSelectionButton"];
        }
    }
}
