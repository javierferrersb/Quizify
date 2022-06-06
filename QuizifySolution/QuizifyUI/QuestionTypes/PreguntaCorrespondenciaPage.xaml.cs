using Quizify.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for PreguntaVerdaderoFalsoPage.xaml
    /// </summary>
    public partial class PreguntaCorrespondenciaPage : Page
    {
        private IQuizifyService service;
        private int questionId;
        private string studentEmail;
        private int currentRealizacionId;
        private string[] frases;
        private string[] terminos;
        private List<TextBlock> frasesBlock;
        private List<TextBox> frasesBox;
        private List<TextBlock> terminosBlock;
        private List<TextBox> terminosBox;
        private List<ComboBox> seleccion;
        private List<ProgressBar> resultadosBar;
        private List<TextBlock> resultadosText;
        private string[] alf = { "A", "B", "C", "D", "E" };
        string frase = "Frase";
        string termino = "Termino";
        public PreguntaCorrespondenciaPage(IQuizifyService service, int questionId)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            currentRealizacionId = -1;

            this.questionId = questionId;
            this.currentRealizacionId = -1;

            generarFrasesTerminos();

            QuestionStatement.Text = service.GetQuestionStatement(questionId);

            studentEmail = "";
            //ShowProgressBars(false);
            ShowEditFunctionality();

            loadCurrentCorrectAnswer();
            foreach(ComboBox cb in seleccion)
            {
                cb.SelectionChanged += ComboBoxHandler;
            }
        }

        private void generarFrasesTerminos()
        {
            frases = service.GetCorrespondenciaFrases(questionId).ToArray();
            terminos = service.GetCorrespondenciaTerminos(questionId).ToArray();
            resultadosBar = new List<ProgressBar>();
            resultadosText = new List<TextBlock>();

            frasesBlock = new List<TextBlock>();
            frasesBox = new List<TextBox>();
            terminosBlock = new List<TextBlock>();
            terminosBox = new List<TextBox>();
            seleccion = new List<ComboBox>();

            TextBlock text;
            for (int i = 0; i < terminos.Length; i++)
            {
                text = new TextBlock();
                text.Text = alf[i] + ". " + frases[i];
                frasesBlock.Add(text);
                TextBox box = new TextBox();
                box.Text = frases[i];
                box.Visibility = Visibility.Hidden;
                frasesBox.Add(box);
            }
            TextBlock term;
            for (int i = 0; i < terminos.Length; i++)
            {
                term = new TextBlock();
                term.Text = (i + 1) + ". " + terminos[i];
                terminosBlock.Add(term);
                TextBox box = new TextBox();
                box.Visibility = Visibility.Hidden;
                box.Text = terminos[i];
                terminosBox.Add(box);
                ComboBox cb = new ComboBox();
                cb.IsEnabled = false;
                seleccion.Add(cb);
            }
            for (int i = 0; i < frases.Length; i++)
            {
                ProgressBar pb = new ProgressBar();
                pb.Visibility = Visibility.Hidden;
                TextBlock pt = new TextBlock();
                pt.Visibility = Visibility.Hidden;
                resultadosBar.Add(pb);
                resultadosText.Add(pt);
            }
            foreach (ComboBox cb in seleccion)
            {
                for(int i = 0; i < seleccion.Count; i++)
                {
                    cb.Items.Add(alf[i]);
                }
            }

            for (int i = 0; i < frases.Length; i++)
            {
                RowDefinition row1 = new();
                row1.Height = new GridLength(1, GridUnitType.Auto);
                FrasesGrid.RowDefinitions.Add(row1);
                RowDefinition row2 = new();
                row2.Height = new GridLength(1, GridUnitType.Auto);
                TermGrid.RowDefinitions.Add(row2);
            }

            /*int j = 0;
            foreach(TextBlock t in frasesBlock)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Pixel);
                g.ColumnDefinitions.Add(cd);

                t.VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(t);
                Grid.SetColumn(t, 0);

                FrasesGrid.Children.Add(g);
                Grid.SetRow(g, j);
                j++;
            }*/
            for (int i = 0; i < frasesBlock.Count; i++)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Auto);
                g.ColumnDefinitions.Add(cd);

                frasesBlock.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                frasesBox.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(frasesBlock.ElementAt(i));
                g.Children.Add(frasesBox.ElementAt(i));
                Grid.SetColumn(frasesBlock.ElementAt(i), 0);
                Grid.SetColumn(frasesBox.ElementAt(i), 0);

                FrasesGrid.Children.Add(g);
                Grid.SetRow(g, i);
            }
            for (int i = 0; i < terminosBlock.Count; i++)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Auto);
                ColumnDefinition cd2 = new();
                cd2.Width = new GridLength(4, GridUnitType.Auto);
                ColumnDefinition cd3 = new();
                cd3.Width = new GridLength(35, GridUnitType.Auto);
                ColumnDefinition cd4 = new();
                cd4.Width = new GridLength(35, GridUnitType.Auto);
                g.ColumnDefinitions.Add(cd);
                g.ColumnDefinitions.Add(cd2);
                g.ColumnDefinitions.Add(cd3);
                g.ColumnDefinitions.Add(cd4);

                terminosBlock.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                terminosBox.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                seleccion.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                resultadosBar.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                resultadosText.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(terminosBlock.ElementAt(i));
                g.Children.Add(terminosBox.ElementAt(i));
                g.Children.Add(seleccion.ElementAt(i));
                g.Children.Add(resultadosBar.ElementAt(i));
                g.Children.Add(resultadosText.ElementAt(i));
                Grid.SetColumn(terminosBlock.ElementAt(i), 0);
                Grid.SetColumn(terminosBox.ElementAt(i), 0);
                Grid.SetColumn(seleccion.ElementAt(i), 1);
                Grid.SetColumn(resultadosBar.ElementAt(i), 2);
                Grid.SetColumn(resultadosText.ElementAt(i), 3);

                TermGrid.Children.Add(g);
                Grid.SetRow(g, i);
            }
            /*j = 0;
            foreach(TextBlock t in terminosBlock)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Pixel);
                ColumnDefinition cd2 = new();
                cd2.Width = new GridLength(4, GridUnitType.Star);
                g.ColumnDefinitions.Add(cd);
                g.ColumnDefinitions.Add(cd2);

                t.VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(t);
                Grid.SetColumn(t, 0);

                TermGrid.Children.Add(g);
                Grid.SetRow(g, j);
                j++;
            }*/
        }

        private void loadCurrentCorrectAnswer()
        {
            Dictionary<int, int> respuestas = service.GetCorrespondenciaCorrectAnswer(questionId);
            for(int i = 0; i < respuestas.Count; i++)
            {
                seleccion.ElementAt(i).SelectedIndex = respuestas[i];
            }
        }

        public PreguntaCorrespondenciaPage(IQuizifyService service, int questionId, int currentRealizacionId, string studentEmail)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.questionId = questionId;
            this.studentEmail = studentEmail;
            this.currentRealizacionId = currentRealizacionId;

            generarFrasesTerminos();

            foreach(ComboBox cb in seleccion)
            {
                cb.IsEnabled = true;
            }
            Dictionary<int, int> respuestaStudent = service.GetCorrespondenciaStudentAnswer(currentRealizacionId, questionId);
            for(int i = 0; i < respuestaStudent.Count; i++)
            {
                seleccion.ElementAt(i).SelectedIndex = respuestaStudent[i];
            }
            /*if (service.GetVFStudentAnswer(currentRealizacionId, questionId) == true)
            {
                TrueRadioButton.IsChecked = true;
            }
            else if (service.GetVFStudentAnswer(currentRealizacionId, questionId) == false)
            {
                FalseRadioButton.IsChecked = true;
            }

            ShowProgressBars(false);
            TrueRadioButton.Checked += RadioButton_Checked;
            FalseRadioButton.Checked += RadioButton_Checked;*/
            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            foreach (ComboBox cb in seleccion)
            {
                cb.SelectionChanged += ComboBoxHandler;
            }
        }
        public PreguntaCorrespondenciaPage(int preguntaCorrespondenciaId, int respuestaPreguntaId, bool correccion, IQuizifyService ser)
        {
            InitializeComponent();
            actualizar();
            this.service = ser;
            this.questionId = preguntaCorrespondenciaId;

            generarFrasesTerminos();

            QuestionStatement.Text = service.GetQuestionStatement(questionId);
            foreach(ComboBox c in seleccion)
            {
                c.Opacity = 100;
            }
            MostrarRespuestaCorrecta(service.getCorrespondenciaAnswerByRespuestaPregunta(respuestaPreguntaId));
            /*if (service.getVFAnswerByRespuestaPregunta(respuestaPreguntaId) != null)
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
            }*/
        }
        public PreguntaCorrespondenciaPage(int preguntaCorrespondenciaId, ICollection<int> respuestaPreguntaCorrespondenciaIDs, bool correccion, IQuizifyService service)
        {
            InitializeComponent();
            actualizar();
            
            this.service = service;
            this.questionId = preguntaCorrespondenciaId;

            resultadosBar = new List<ProgressBar>();
            resultadosText = new List<TextBlock>();

            generarFrasesTerminos();

            foreach (ComboBox c in seleccion)
            {
                c.Opacity = 100;
            }

            loadCurrentCorrectAnswer();

            for(int i = 0; i < resultadosBar.Count; i++)
            {
                resultadosBar.ElementAt(i).Visibility = Visibility.Visible;
                resultadosText.ElementAt(i).Visibility = Visibility.Visible;
            }
            int todasRespuestas = respuestaPreguntaCorrespondenciaIDs.Count;
            for(int i = 0; i < resultadosBar.Count; i++)
            {
                int numRespuestas = getRespuestasCorrectas(i, respuestaPreguntaCorrespondenciaIDs);
                resultadosBar.ElementAt(i).Maximum = todasRespuestas;
                resultadosBar.ElementAt(i).Value = numRespuestas;
                resultadosText.ElementAt(i).Text = numRespuestas + "/" + todasRespuestas + " (" + (numRespuestas * 100 / todasRespuestas) + "%)";
            }

            /*int numRespuestasV = getRespuestasV(respuestaPreguntaVFIDs);
            int numRespuestasF = getRespuestasF(respuestaPreguntaVFIDs);
            int todasRespuestas = numRespuestasV + numRespuestasF;

            /*answersVDistribution.Maximum = todasRespuestas;
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
            }*/
        }
        private void MostrarRespuestaCorrecta(Dictionary<int, int> studentAnswer)
        {
            Dictionary<int, int> correctAnswer = service.GetCorrespondenciaCorrectAnswer(questionId);
            for(int i = 0; i < seleccion.Count; i++)
            {
                if (studentAnswer[i] == -1)
                {
                    seleccion.ElementAt(i).SelectedIndex = correctAnswer[i];
                }
                else
                {
                    seleccion.ElementAt(i).SelectedIndex = studentAnswer[i];
                    if(studentAnswer[i] == correctAnswer[i])
                    {
                        seleccion.ElementAt(i).Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        seleccion.ElementAt(i).Foreground = new SolidColorBrush(Colors.Red);
                    }
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
            //TrueRadioButton.IsEnabled = true;
            //FalseRadioButton.IsEnabled = true;
            selectNumFrase.Visibility = Visibility.Visible;
            numFrasesLabel.Visibility = Visibility.Visible;
            foreach(ComboBox cb in seleccion)
            {
                cb.IsEnabled = true;
            }
            MostrarTextBox(true);

            EditValueButton.IsEnabled = false;
            EditValueButton.Visibility = Visibility.Hidden;

            SaveValueButton.Visibility = Visibility.Visible;
            SaveValueButton.IsEnabled = true;

            switch (frases.Length)
            {
                case 5:
                    selectNumFrase.SelectedIndex = 0;
                    break;
                case 4:
                    selectNumFrase.SelectedIndex = 1;
                    break;
                case 3:
                    selectNumFrase.SelectedIndex = 2;
                    break;
                case 2:
                    selectNumFrase.SelectedIndex = 3;
                    break;
                default:
                    break;
            }

            selectNumFrase.SelectionChanged += SelectNumFrasesHandler;

            EditStatementButton.IsEnabled = false;
        }

        private void MostrarTextBox(bool mostrar)
        {
            foreach (TextBox fb in frasesBox)
            {
                fb.Visibility = mostrar ? Visibility.Visible : Visibility.Hidden;
            }
            foreach (TextBlock fb in frasesBlock)
            {
                fb.Visibility = !mostrar ? Visibility.Visible : Visibility.Hidden;
            }
            foreach (TextBox tb in terminosBox)
            {
                tb.Visibility = mostrar ? Visibility.Visible : Visibility.Hidden;
            }
            foreach (TextBlock tb in terminosBlock)
            {
                tb.Visibility = !mostrar ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void LoadFrasesTerminos()
        {
            int i = 0;
            foreach (TextBlock fb in frasesBlock)
            {
                fb.Text = alf[i] + ". " + frases[i];
                i++;
            }
            i = 0;
            foreach(TextBlock tb in terminosBlock)
            {
                tb.Text = (i + 1) + ". " + terminos[i];
                i++;
            }

        }

        private void ComboBoxHandler(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)e.Source;
            if(cb.SelectedIndex == -1)
            {
                return;
            }
            foreach(ComboBox combo in seleccion)
            {
                if (!cb.Equals(combo))
                {
                    if(combo.SelectedIndex == cb.SelectedIndex)
                    {
                        combo.SelectedIndex = -1;
                        if (currentRealizacionId != -1)
                        {
                            service.SetCorrespondenciaStudentAnswer(currentRealizacionId, questionId, combo.SelectedIndex, seleccion.IndexOf(combo));
                        }
                    }
                }
            }
            if(currentRealizacionId != -1)
            {
                service.SetCorrespondenciaStudentAnswer(currentRealizacionId, questionId, cb.SelectedIndex, seleccion.IndexOf(cb));
            }
        }

        private void SelectNumFrasesHandler(object sender, RoutedEventArgs e)
        {
            int numFrase = -1;
            switch (selectNumFrase.SelectedIndex)
            {
                case 0:
                    numFrase = 5;
                    break;
                case 1:
                    numFrase = 4;
                    break;
                case 2:
                    numFrase = 3;
                    break;
                case 3:
                    numFrase = 2;
                    break;
                default:
                    break;
            }
            if (numFrase == -1) return;
            FrasesGrid.Children.RemoveRange(0, FrasesGrid.Children.Count);
            TermGrid.Children.RemoveRange(0, TermGrid.Children.Count);
            frasesBlock = new List<TextBlock>();
            frasesBox = new List<TextBox>();
            terminosBlock = new List<TextBlock>();
            terminosBox = new List<TextBox>();
            seleccion = new List<ComboBox>();
            for (int i = 0; i < numFrase; i++)
            {
                TextBlock text = new TextBlock();
                text.Text = alf[i] + ". " + frase;
                text.Visibility = Visibility.Hidden;
                frasesBlock.Add(text);
                TextBox box = new TextBox();
                box.Text = frase;
                box.Visibility = Visibility.Visible;
                frasesBox.Add(box);
            }
            for (int i = 0; i < numFrase; i++)
            {
                TextBlock term = new TextBlock();
                term.Text = (i + 1) + ". " + termino;
                term.Visibility = Visibility.Hidden;
                terminosBlock.Add(term);
                TextBox box = new TextBox();
                box.Visibility = Visibility.Visible;
                box.Text = termino;
                terminosBox.Add(box);
                ComboBox cb = new ComboBox();
                cb.IsEnabled = true;
                seleccion.Add(cb);
            }
            foreach (ComboBox cb in seleccion)
            {
                for (int i = 0; i < seleccion.Count; i++)
                {
                    cb.Items.Add(alf[i]);
                }
            }

            for (int i = 0; i < numFrase; i++)
            {
                RowDefinition row1 = new();
                row1.Height = new GridLength(1, GridUnitType.Auto);
                FrasesGrid.RowDefinitions.Add(row1);
                RowDefinition row2 = new();
                row2.Height = new GridLength(1, GridUnitType.Auto);
                TermGrid.RowDefinitions.Add(row2);
            }
            /*int j = 0;
            foreach(TextBlock t in frasesBlock)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Pixel);
                g.ColumnDefinitions.Add(cd);

                t.VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(t);
                Grid.SetColumn(t, 0);

                FrasesGrid.Children.Add(g);
                Grid.SetRow(g, j);
                j++;
            }*/
            
            for (int i = 0; i < frasesBlock.Count; i++)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Auto);
                g.ColumnDefinitions.Add(cd);

                frasesBlock.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                frasesBox.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(frasesBlock.ElementAt(i));
                g.Children.Add(frasesBox.ElementAt(i));
                Grid.SetColumn(frasesBlock.ElementAt(i), 0);
                Grid.SetColumn(frasesBox.ElementAt(i), 0);

                FrasesGrid.Children.Add(g);
                Grid.SetRow(g, i);
            }
 
            for (int i = 0; i < terminosBlock.Count; i++)
            {
                Grid g = new();
                ColumnDefinition cd = new();
                cd.Width = new GridLength(35, GridUnitType.Auto);
                ColumnDefinition cd2 = new();
                cd2.Width = new GridLength(4, GridUnitType.Auto);
                /*ColumnDefinition cd3 = new();
                cd3.Width = new GridLength(35, GridUnitType.Auto);
                ColumnDefinition cd4 = new();
                cd4.Width = new GridLength(35, GridUnitType.Auto);*/
                g.ColumnDefinitions.Add(cd);
                g.ColumnDefinitions.Add(cd2);
               /* g.ColumnDefinitions.Add(cd3);
                g.ColumnDefinitions.Add(cd4);*/

                terminosBlock.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                terminosBox.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                seleccion.ElementAt(i).VerticalAlignment = VerticalAlignment.Center;
                g.Children.Add(terminosBlock.ElementAt(i));
                g.Children.Add(terminosBox.ElementAt(i));
                g.Children.Add(seleccion.ElementAt(i));
                Grid.SetColumn(terminosBlock.ElementAt(i), 0);
                Grid.SetColumn(terminosBox.ElementAt(i), 0);
                Grid.SetColumn(seleccion.ElementAt(i), 1);

                TermGrid.Children.Add(g);
                Grid.SetRow(g, i);
            }
            for(int i = 0; i < numFrase; i++)
            {
                seleccion.ElementAt(i).SelectedIndex = i;
            }
            foreach (ComboBox cb in seleccion)
            {
                cb.SelectionChanged += ComboBoxHandler;
            }
        }

        private void SaveValueHandler(object sender, RoutedEventArgs e)
        {
            int numberFrases = -1;
            switch (selectNumFrase.SelectedIndex)
            {
                case 0:
                    numberFrases = 5;
                    break;
                case 1:
                    numberFrases = 4;
                    break;
                case 2:
                    numberFrases = 3;
                    break;
                case 3:
                    numberFrases = 2;
                    break;
                default:
                    break;
            }
            frases = new string[numberFrases];
            terminos = new string[numberFrases];
            service.SetCorrespondenciaNumberPhrases(questionId, numberFrases);
            selectNumFrase.Visibility = Visibility.Hidden;
            numFrasesLabel.Visibility = Visibility.Hidden;
            int i = 0;
            foreach(TextBox fb in frasesBox)
            {
                frases[i] = fb.Text;
                i++;
            }
            i = 0;
            foreach (TextBox tb in terminosBox)
            {
                terminos[i] = tb.Text;
                i++;
            }
            service.SetCorrespondenciaFrasesTerminosText(questionId, frases, terminos);
            foreach(ComboBox combo in seleccion)
            {
                service.SetCorrespondenciaFraseTermino(questionId, seleccion.IndexOf(combo), combo.SelectedIndex);
                combo.IsEnabled = false;
            }
            LoadFrasesTerminos();
            EditValueButton.IsEnabled = true;
            EditValueButton.Visibility = Visibility.Visible;

            SaveValueButton.Visibility = Visibility.Hidden;
            SaveValueButton.IsEnabled = false;

            selectNumFrase.SelectionChanged -= SelectNumFrasesHandler;

            EditStatementButton.IsEnabled = true;

            MostrarTextBox(false);
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

        private int getRespuestasCorrectas(int indexTermino, ICollection<int> respuestasId)
        {
            int res = 0;
            foreach (int respuestaPregunta in respuestasId)
            {
                if (service.getCorrespondenciaAnswerByRespuestaPregunta(respuestaPregunta)[indexTermino] == service.GetCorrespondenciaCorrectAnswer(questionId)[indexTermino])
                {
                    res++;
                }
            }
            return res;
        }
        private void actualizar()
        {
            //TrueRadioButton.Content = Idioma.info["TrueRadioButton"];
            //FalseRadioButton.Content = Idioma.info["FalseRadioButton"];
            EditStatementButton.ToolTip = Idioma.info["EditStatementButton"];
            SaveStatementButton.ToolTip = Idioma.info["SaveStatementButton"];
            EditValueButton.ToolTip = Idioma.info["EditValueButton"];
            SaveValueButton.ToolTip = Idioma.info["SaveValueButton"];
            numFrasesLabel.Content = Idioma.info["numFrasesLabel"];
            frase = Idioma.info["frase"];
            termino = Idioma.info["termino"];
        }
    }
}
