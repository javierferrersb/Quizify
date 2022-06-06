using Quizify.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for CreateQuiz.xaml
    /// </summary>
    public partial class EditQuiz : Window
    {
        private IQuizifyService service;
        private int quizId;
        private int currentQuestionNum;
        private List<BateriaItem> baterias;
        private string personEmail;
        private Window mainWindow;
        private string statementString = "Enunciado";
        private string preguntaString = "Pregunta ";
        private string of = " de ";
        private string frase = "Frase";
        private string termino = "Termino";
        public EditQuiz(IQuizifyService service, int quizId, string personEmail, Window mainWindow)
        {
            InitializeComponent();
            actualizar();
            this.service = service;
            this.personEmail = personEmail;
            baterias = new List<BateriaItem>();
            this.quizId = quizId;

            LoadQuestionsData();
            currentQuestionNum = 0;

            LoadQuestionsData();
            LoadQuizSettingsValues();
            LoadCurrentQuestion();
            QuizName.Text = service.GetQuizName(quizId);
            this.Title = service.GetQuizName(quizId);

            this.mainWindow = mainWindow;
            ((VistaPrincipal)mainWindow).CloseDialog();
        }

        private void LoadQuestionsData()
        {
            List<List<String>> list = service.GetBatteriesFromQuiz(quizId);

            baterias.Clear();
            foreach (List<String> item in list)
            {
                baterias.Add(new BateriaItem
                {
                    Id = int.Parse(item[0]),
                    Name = item[1],
                    NumQuestions = int.Parse(item[2]),
                    Type = int.Parse(item[3])
                });
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionNum++;
            LoadCurrentQuestion();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionNum--;
            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            QuestionNum.Text = preguntaString + (currentQuestionNum + 1) + of + baterias.Count;
            if (baterias.Count == 0 || currentQuestionNum == baterias.Count)
            {
                setMarkButton.IsEnabled = false;
                QuestionFrame.Navigate(new PreguntaNueva());
                DeleteQuestionButton.Visibility = Visibility.Hidden;
                QuestionNum.Visibility = Visibility.Hidden;
            }
            else
            {
                BateriaItem bateriaToOpen = baterias.ElementAt(currentQuestionNum);
                if (bateriaToOpen.NumQuestions == 1)
                {
                    if (bateriaToOpen.Type == 0)
                    {
                        QuestionFrame.Navigate(new PreguntaAbiertaPage(service, service.GetQuestionIdFromBattery(bateriaToOpen.Id, 0)));
                    }
                    else if (bateriaToOpen.Type == 1)
                    {
                        QuestionFrame.Navigate(new PreguntaVerdaderoFalsoPage(service, service.GetQuestionIdFromBattery(bateriaToOpen.Id, 0)));
                    }
                    else if (bateriaToOpen.Type == 2)
                    {
                        QuestionFrame.Navigate(new PreguntaSeleccionMultiplePage(service, service.GetQuestionIdFromBattery(bateriaToOpen.Id, 0)));
                    }
                    else if (bateriaToOpen.Type == 3)
                    {
                        QuestionFrame.Navigate(new PreguntaCorrespondenciaPage(service, service.GetQuestionIdFromBattery(bateriaToOpen.Id, 0)));
                    }
                }
                else
                {
                    QuestionFrame.Navigate(new PreguntaDeBateria(service, bateriaToOpen.Id));
                }
                endQuiz.IsEnabled = true;
                setMarkButton.IsEnabled = true;
                DeleteQuestionButton.Visibility = Visibility.Visible;
                QuestionNum.Visibility = Visibility.Visible;
            }
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            if (currentQuestionNum == baterias.Count)
            {
                setMarkButton.IsEnabled = false;
                NextButton.IsEnabled = false;
                AddQuestionButton.Visibility = Visibility.Visible;
            }
            else
            {
                NextButton.IsEnabled = true;
                AddQuestionButton.Visibility = Visibility.Hidden;
            }
            if (currentQuestionNum == 0)
            {
                PreviousButton.IsEnabled = false;
            }
            else
            {
                PreviousButton.IsEnabled = true;
            }
        }

        private void CreatePreguntaAbierta(object sender, RoutedEventArgs e)
        {

            int nuevaBateria = service.CreateBateria(0, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
            service.CreateQuestionInBateria(nuevaBateria, statementString, service.GetInstructorEmailFromQuiz(quizId));

            setMarkButton.IsEnabled = true;

            LoadQuestionsData();

            LoadCurrentQuestion();
        }

        private void CreatePreguntaIndependiente(object sender, RoutedEventArgs e)
        {
            SeleccionarPregunta sp = new SeleccionarPregunta(service);
            sp.ShowDialog();
            Question q = (Question)sp.QuestionsDataGrid.SelectedItem;
            if (q != null)
            {
                if (service.GetQuestionType(q.Id) == 1)
                {
                    int nuevaBateria = service.CreateBateria(1, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
                    service.InsertQuestionToBattery(nuevaBateria, q.Id);
                }
                else if (service.GetQuestionType(q.Id) == 0)
                {
                    int nuevaBateria = service.CreateBateria(0, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
                    service.InsertQuestionToBattery(nuevaBateria, q.Id);
                }
                else if (service.GetQuestionType(q.Id) == 2)
                {
                    int nuevaBateria = service.CreateBateria(2, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
                    service.InsertQuestionToBattery(nuevaBateria, q.Id);
                }

            }

            LoadQuestionsData();

            LoadCurrentQuestion();
        }

        private void CreatePreguntaVerdaderoFalso(object sender, RoutedEventArgs e)
        {
            int nuevaBateria = service.CreateBateria(1, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
            service.CreateQuestionInBateria(nuevaBateria, statementString, service.GetInstructorEmailFromQuiz(quizId));

            setMarkButton.IsEnabled = true;

            LoadQuestionsData();

            LoadCurrentQuestion();
        }
        private void CreatePreguntaSeleccionMultiple(object sender, RoutedEventArgs e)
        {
            int nuevaBateria = service.CreateBateria(2, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
            service.CreateQuestionInBateria(nuevaBateria, statementString, service.GetInstructorEmailFromQuiz(quizId));
            setMarkButton.IsEnabled = true;

            LoadQuestionsData();

            LoadCurrentQuestion();
        }
        private void CreatePreguntaCorrespondencia(object sender, RoutedEventArgs e)
        {
            int nuevaBateria = service.CreateBateria(3, quizId, "Bateria de quiz " + service.GetQuizName(quizId) + " en la posicion " + currentQuestionNum + 1);
            service.CreateQuestionInBateria(nuevaBateria, statementString, service.GetInstructorEmailFromQuiz(quizId));
            setMarkButton.IsEnabled = true;

            int questionId = service.GetQuestionIdFromBattery(nuevaBateria, 0);

            service.initPreguntaCorrespondencia(questionId, frase, termino);

            LoadQuestionsData();

            LoadCurrentQuestion();
        }
        private void AddBateriaPreguntas(object sender, RoutedEventArgs e)
        {
            SeleccionarBateria sb = new SeleccionarBateria(service, personEmail);
            sb.ShowDialog();
            Battery item = (Battery)sb.BatteryTable.SelectedItem;
            if (item != null)
            {
                service.AddItemABateria(quizId, item.ID);
            }

            LoadQuestionsData();

            LoadCurrentQuestion();
        }

        private void modifyQuestion(object sender, RoutedEventArgs e)
        {
            AsignarPuntuacion asigPunt = new(service, baterias[currentQuestionNum].Id);
            asigPunt.ShowDialog();
        }

        private void quizFinished(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoadQuizSettingsValues()
        {
            QuizNameField.Text = service.GetQuizName(quizId);
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveQuizSettingsValues();
        }

        private void SaveQuizSettingsValues()
        {
            if (QuizNameField.Text != "")
            {
                service.SetQuizName(quizId, QuizNameField.Text);
                QuizName.Text = QuizNameField.Text;
                ((VistaPrincipal)mainWindow).RefreshQuizesList(quizId);
            }
            else
            {
                MessageBoxResult confirmResult = MessageBox.Show("No puedes dejar en blanco el nombre del Quiz", "Atención", MessageBoxButton.OK);

                QuizNameField.Text = QuizName.Text;
            }
        }

        private void CancelSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadQuizSettingsValues();
        }

        private void setMarkButton_Click(object sender, RoutedEventArgs e)
        {
            AsignarPuntuacion ap = new AsignarPuntuacion(service, service.GetNotaBateriaIdFromBatteryAndQuiz(quizId, baterias[currentQuestionNum].Id));
            ap.ShowDialog();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["EditQuizTitulo"];
            PreviousButton.Content = Idioma.info["PreviousButton"];
            NextButton.Content = Idioma.info["NextButton"];
            setMarkButton.Content = Idioma.info["setMarkButton"];
            endQuiz.Content = Idioma.info["endQuiz"];
            AddQuestionButton.ToolTip = Idioma.info["AddQuestionButton1"];
            CreateBateriaButton.ToolTip = Idioma.info["CreateBateriaButton"];
            CreatePreguntaAbiertaButton.ToolTip = Idioma.info["CreatePreguntaAbiertaButton"];
            CreatePreguntaVerdaderoFalsoButton.ToolTip = Idioma.info["CreatePreguntaVerdaderoFalsoButton"];
            CreatePreguntaSeleccionMultipleButton.ToolTip = Idioma.info["CreatePreguntaSeleccionMultipleButton"];
            statementString = Idioma.info["statementString"];
            ajustesQuiz.Text = Idioma.info["ajustesQuiz"];
            CancelSettingsButton.Content = Idioma.info["CancelSettingsButton"];
            SaveSettingsButton.Content = Idioma.info["SaveSettingsButton"];
            DeleteQuestionTextBlock.Text = Idioma.info["DeleteQuestionTextBlock"];
            preguntaString = Idioma.info["preguntaString"];
            of = Idioma.info["of"];
            frase = Idioma.info["frase"];
            termino = Idioma.info["termino"];
        }

        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Idioma.info["DeleteQuestionMessageBoxMessage"], Idioma.info["DeleteQuestionMessageBoxCaption"], MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                service.RemoveBatteryFromQuiz(quizId, baterias.ElementAt(currentQuestionNum).Id, currentQuestionNum);
                baterias.RemoveAt(currentQuestionNum);
                LoadCurrentQuestion();
                if (baterias.Count == 0)
                {
                    endQuiz.IsEnabled = false;
                }
            }
        }
    }
}
