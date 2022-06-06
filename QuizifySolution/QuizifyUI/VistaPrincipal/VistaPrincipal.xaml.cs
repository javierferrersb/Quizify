using MaterialDesignThemes.Wpf;
using Quizify.Services;
using QuizifyUI.QuizStatus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para VistaInstructor.xaml
    /// </summary>
    public partial class VistaPrincipal : Window
    {
        private string personEmail;
        private IQuizifyService service;
        public List<TabItem> tabs;
        private List<CourseItem> processedCoursesList;
        private string morningMessage = "Buenos días, ";
        private string afterMessage = "Buenas tardes, ";
        private string eveningMessage = "Buenas noches, ";
        private string errorIncribe = "No existe un curso con ese identificador, o usted ya está inscrito en él";
        private string principalTittle = "Pantalla principal de ";
        private string quizNameCol = "Nombre";
        private string quizDateCreateCol = "Fecha de Creación";
        private string quizAutorCol = "Autor";
        private string quizInitDateCol = "Fecha de Inicio";
        private string quizClosingDateCol = "Fecha de Cierre";
        private string quizNumAttemptsCol = "Número de Intentos";
        private string quizReanudando = "Reanudando quiz pausado";
        private string information = "Información";
        private string volverRealizar = "¿Desea volver a realizar el quiz?";
        private string maxRealizaciones = "Ha alcanzado el número max. de realizaciones, no puede volver a realizarlo.\n¿Desea ver el resultado de la última realización?";
        private string sinPermisoResultado = "El Instructor no permite abrir los resultados del examen";
        public VistaPrincipal(IQuizifyService service, string personEmail, bool isDarkModeOn)
        {
            InitializeComponent();
            this.service = service;

            tabs = new List<TabItem>();
            this.personEmail = personEmail;
            DarkModeSwitch.IsChecked = isDarkModeOn;

            if (PersonIsStudent())
            {
                ActivateStudentHandlers();
            }
            else
            {
                LoadQuestions();
                ActivateInstructorHandlers();
            }
            actualizar();
        }

        private void ActivateStudentHandlers()
        {
            TabButtonPlus.Click += ShowInscribeStudentDialog;
        }

        private void ActivateInstructorHandlers()
        {
            TabButtonPlus.Click += CreateCourseButton_Click;
        }

        private bool PersonIsStudent()
        {
            return service.IsPersonAStudent(personEmail);
        }

        private void SetWindowTitle()
        {
            this.Title = principalTittle + service.GetPersonName(personEmail);
        }

        private void SetTitleMessage()
        {
            int currentHour = DateTime.Now.Hour;
            if (currentHour > 7 && currentHour < 12)
            {
                GreetingTextBlock.Text = morningMessage;
            }
            else if (currentHour >= 12 && currentHour < 18)
            {
                GreetingTextBlock.Text = afterMessage;
            }
            else
            {
                GreetingTextBlock.Text = eveningMessage;
            }
            GreetingTextBlock.Text += service.GetPersonName(personEmail);
        }
        public void InscribeStudentToCourse(int courseId)
        {
            if (service.InscribeStudentToCourse(personEmail, courseId))
            {
                LoadCourse(courseId);
                LoadAllQuizesView();
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            else
            {
                MessageBox.Show(errorIncribe, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void CreateAllQuizesColumns()
        {
            QuizzesTable.Columns.Clear();
            QuizzesTable.Items.Clear();

            GridManager.AddGridColumn(QuizzesTable, quizNameCol, "Name");
            GridManager.AddGridColumn(QuizzesTable, quizDateCreateCol, "CreationDate");
            GridManager.AddGridColumn(QuizzesTable, quizAutorCol, "Author");
            GridManager.AddGridColumn(QuizzesTable, quizInitDateCol, "StartDate");
            GridManager.AddGridColumn(QuizzesTable, quizClosingDateCol, "EndDate");
            GridManager.AddGridColumn(QuizzesTable, quizNumAttemptsCol, "NumberOfTries");

            QuizzesTable.MouseDoubleClick += QuizzesTable_MouseDoubleClick;
        }

        private void QuizzesTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (contentSelected == null) return;

            QuizItem selectedQuiz = (QuizItem)contentSelected.Item;

            HandleQuizDoubleQuick(selectedQuiz);
        }

        public void HandleQuizDoubleQuick(QuizItem quiz)
        {
            if (PersonIsStudent())
            {
                HandleStudentEnteringTest(quiz.Id);
            }
            else
            {
                ShowExistingQuizDialog(quiz);
            }
        }

        private void HandleStudentEnteringTest(int quizId)
        {
            if (IsStudentResumingQuiz(quizId))
            {
                MessageBox.Show(quizReanudando, information, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                HacerQuiz(quizId);
            }
            else if (StudentHasRemainingTries(quizId))
            {
                HacerQuiz(quizId);
            }
            else if (!IsStudentResumingQuiz(quizId) && StudentHasRealizedQuiz(quizId))
            {
                MessageBoxResult result;
                //If student can re-do the quiz again...
                if (StudentHasRemainingTries(quizId))
                {
                    result = MessageBox.Show(volverRealizar, information, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        HacerQuiz(quizId);
                    }
                }//The student cannot re-do the exam
                else
                {
                    if (!StudentHasRemainingTries(quizId))
                    {
                        MessageBox.Show(maxRealizaciones, information, MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }
            }
        }

        private void HacerQuiz(int quizId)
        {
            HacerQuiz hacerQuizWindow = new HacerQuiz(service, quizId, personEmail);
            hacerQuizWindow.ShowDialog();
        }

        private bool IsStudentResumingQuiz(int quizId)
        {
            return service.StudentHasUnfinishedAnsweredTest(personEmail, quizId);
        }

        private bool StudentsCanViewQuizAnswers(int quizId)
        {
            return service.GetQuizViewResult(quizId);
        }

        private bool StudentHasRemainingTries(int quizId)
        {
            int quizNumberOfTries = service.GetQuizNumberOfAttempts(quizId);
            if (quizNumberOfTries == -1)
            {
                return true;
            }
            else
            {
                return service.GetStudentRealizationsCount(personEmail, quizId) < quizNumberOfTries;
            }
        }

        private bool StudentHasRealizedQuiz(int quizId)
        {
            return service.GetStudentRealizationsCount(personEmail, quizId) > 0;
        }
        private void LoadAllQuizesView()
        {
            List<List<String>> list = service.GetAllQuizzes(personEmail);

            CreateAllQuizesColumns();

            foreach (List<String> listofQuizInfo in list)
            {
                QuizItem item = (new QuizItem
                {
                    Id = int.Parse(listofQuizInfo.ElementAt(0)),
                    Name = listofQuizInfo.ElementAt(1),
                    CreationDate = listofQuizInfo.ElementAt(2),
                    Author = listofQuizInfo.ElementAt(3),
                    StartDate = listofQuizInfo.ElementAt(4),
                    EndDate = listofQuizInfo.ElementAt(5),
                    NumberOfTries = listofQuizInfo.ElementAt(6),
                    CourseId = int.Parse(listofQuizInfo.ElementAt(7)),
                    Status = int.Parse(listofQuizInfo.ElementAt(8))
                });
                if (!PersonIsStudent() || QuizIsActive(item))
                {
                    QuizzesTable.Items.Add(item);
                }
            }
        }
        private bool QuizIsActive(QuizItem item)
        {
            return item.Status == 2;
        }

        private void LoadCoursesTabs()
        {
            List<List<String>> rawCoursesList = service.GetCoursesFromPerson(personEmail);

            processedCoursesList = new List<CourseItem>();

            foreach (List<String> listOfCourseInfo in rawCoursesList)
            {
                processedCoursesList.Add(new CourseItem
                {
                    Id = int.Parse(listOfCourseInfo.ElementAt(0)),
                    Name = listOfCourseInfo.ElementAt(1),
                    Description = listOfCourseInfo.ElementAt(2)
                });
            }

            foreach (CourseItem course in processedCoursesList)
            {
                LoadCourse(course.Id);
            }
        }

        private void LoadQuestions()
        {
            TabItem tab = new TabItem();

            StackPanel header = new StackPanel();
            PackIcon packIcon = new PackIcon();
            packIcon.Kind = PackIconKind.Folder;
            packIcon.Height = 24;
            packIcon.Width = 24;
            packIcon.HorizontalAlignment = HorizontalAlignment.Center;
            header.Children.Add(packIcon);

            TextBlock courseName = new TextBlock();
            courseName.Text = Idioma.info["Question"];
            courseName.HorizontalAlignment = HorizontalAlignment.Center;
            header.Children.Add(courseName);
            tab.Header = header;

            Frame contentFrame = new Frame();
            contentFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
            contentFrame.Content = new VistaSeleccionPreguntas(service);
            contentFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            contentFrame.LoadCompleted += ContentFrame_LoadCompleted1;
            contentFrame.Margin = new Thickness(20, 10, 10, 10);
            tab.Content = contentFrame;
            InstructorTabControl.Items.Add(tab);
        }

        private void ContentFrame_LoadCompleted1(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            (e.Content as VistaSeleccionPreguntas).Tag = this;
        }

        private void LoadCourse(int courseId)
        {
            TabItem tab = new TabItem();

            tab.Tag = courseId;
            StackPanel header = new StackPanel();
            PackIcon packIcon = new PackIcon();
            packIcon.Kind = GetPackIconKind(courseId);
            packIcon.Height = 24;
            packIcon.Width = 24;
            packIcon.HorizontalAlignment = HorizontalAlignment.Center;
            header.Children.Add(packIcon);

            TextBlock courseName = new TextBlock();
            courseName.Text = service.GetCourseName(courseId);
            courseName.HorizontalAlignment = HorizontalAlignment.Center;
            header.Children.Add(courseName);
            tab.Header = header;

            Frame contentFrame = new Frame();
            contentFrame.LoadCompleted += ContentFrame_LoadCompleted;
            contentFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            contentFrame.Content = new VistaSeleccionContenido(service, courseId, personEmail);
            contentFrame.Margin = new Thickness(20, 10, 10, 10);
            tab.Content = contentFrame;
            InstructorTabControl.Items.Add(tab);
            tabs.Add(tab);
        }

        private PackIconKind GetPackIconKind(int courseId)
        {
            switch (service.GetCourseIcon(courseId))
            {
                case 1:
                    return PackIconKind.Cog;
                case 2:
                    return PackIconKind.Airplane;
                case 3:
                    return PackIconKind.Adjust;
                case 4:
                    return PackIconKind.Airballoon;
                case 5:
                    return PackIconKind.Calculator;
                case 6:
                    return PackIconKind.Alien;
                case 7:
                    return PackIconKind.MathCompass;
                case 8:
                    return PackIconKind.Flask;
                case 9:
                    return PackIconKind.SineWave;
                case 10:
                    return PackIconKind.Book;
                case 11:
                    return PackIconKind.Music;
                case 12:
                    return PackIconKind.ChartLine;
                case 13:
                    return PackIconKind.Earth;
            }
            return PackIconKind.Cog;
        }
        private void ClearCoursesTabs()
        {
            int count = InstructorTabControl.Items.Count;
            for (int i = 4; i < count; i++)
            {
                InstructorTabControl.Items.RemoveAt(4);
            }
        }
        private void LoadInstructorProfileTab()
        {
            ProfileFrame.Content = new AccountPage(service, personEmail);
        }

        private void ContentFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            (e.Content as VistaSeleccionContenido).Tag = this;
        }

        private void QuizSelectFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            (e.Content as VistaSeleccionQuiz).Tag = this;
        }



        public void ShowQuizzesOfTopic(int topicId, int courseId)
        {
            ((Frame)((TabItem)InstructorTabControl.SelectedItem).Content).LoadCompleted -= ContentFrame_LoadCompleted;
            ((Frame)((TabItem)InstructorTabControl.SelectedItem).Content).LoadCompleted += QuizSelectFrame_LoadCompleted;
            ((Frame)((TabItem)InstructorTabControl.SelectedItem).Content).Content = new VistaSeleccionQuiz(service, topicId, courseId, personEmail);
        }

        public void ShowExistingQuizDialog(QuizItem quiz)
        {
            DialogFrame.Content = new ExistingQuizDialog(service, quiz, personEmail, this);
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        public void ShowCopyQuizDialog(int quizId, int topicId)
        {
            DialogFrame.Content = new NewQuizDialog(quizId, topicId);
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        public void ShowQuizStatusDialog(QuizItem quiz)
        {
            QuizStatusManager status = new QuizStatusManager(service, quiz, this);
            DialogHost.CloseDialogCommand.Execute(null, null);
            status.ShowDialog();
        }

        public void HandleQuestionDoubleClick(PreguntaItem pregunta)
        {
            if (true)
            {
                int quizId = pregunta.QuizId;
                bool activo = service.IsQuizActiveFromPregunta(pregunta.Id, quizId);
                DialogFrame.Content = new EditQuestion(service, pregunta, activo);
                DialogHost.OpenDialogCommand.Execute(null, null);
            }
        }

        public void ShowCopyCourseDialog(int cursoId)
        {
            DialogFrame.Content = new NewCourseDialog(cursoId);
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        public void CreateCourse(string courseName)
        {
            if (courseName != "")
            {
                int newCourseId = service.CreateNewCourse(personEmail, courseName, 1, "");
                LoadCourse(newCourseId);
                DialogHost.CloseDialogCommand.Execute(null, null);
                CursoCreadoSnackbar.IsActive = true;
            }
        }

        public void ShowCreateTopicDialog(int courseId)
        {
            DialogFrame.Content = new NewTopicDialog(courseId);
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        public void ShowCreateQuizDialog(int topicId)
        {
            if (service.getNumQuizesRestantes(personEmail) > 0)
            {
                DialogFrame.Content = new NewQuizDialog(topicId);
                DialogHost.OpenDialogCommand.Execute(null, null);
            }
            else
            {
                NoQuizesSnackbar.IsActive = true;
            }
        }

        private void ShowInscribeStudentDialog(object sender, RoutedEventArgs e)
        {
            DialogFrame.Content = new InscribeStudentCourseDialog();
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        public void CreateNewTopic(int courseId, string newTopicName)
        {
            service.AddTopicToCourse(courseId, newTopicName);
            RefreshTopicOfOpenTab(courseId);
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        private void RefreshTopicOfOpenTab(int courseId)
        {
            TabItem item = (TabItem)InstructorTabControl.SelectedItem;
            Frame contentFrame = new Frame();
            contentFrame.LoadCompleted += ContentFrame_LoadCompleted;
            contentFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            contentFrame.Content = new VistaSeleccionContenido(service, courseId, personEmail);
            contentFrame.Margin = new Thickness(20, 10, 10, 10);
            item.Content = contentFrame;
        }

        private void CreateCourseButton_Click(object sender, RoutedEventArgs e)
        {
            NewCourseDialog dialog = new NewCourseDialog();
            DialogFrame.Content = dialog;
            DialogHost.OpenDialogCommand.Execute(null, null);
        }

        private void DialogFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (DialogFrame.Content is ExistingQuizDialog)
            {
                ((ExistingQuizDialog)DialogFrame.Content as ExistingQuizDialog).Tag = this;
            }
            else if (DialogFrame.Content is NewCourseDialog)
            {
                ((NewCourseDialog)DialogFrame.Content as NewCourseDialog).Tag = this;
            }
            else if (DialogFrame.Content is NewTopicDialog)
            {
                ((NewTopicDialog)DialogFrame.Content).Tag = this;
            }
            else if (DialogFrame.Content is NewQuizDialog)
            {
                ((NewQuizDialog)DialogFrame.Content).Tag = this;
            }
            else if (DialogFrame.Content is EditCourseDialog)
            {
                ((EditCourseDialog)DialogFrame.Content).Tag = this;
            }
            else if (DialogFrame.Content is InscribeStudentCourseDialog)
            {
                ((InscribeStudentCourseDialog)DialogFrame.Content).Tag = this;
            }
            else if (DialogFrame.Content is EditQuestion)
            {
                ((EditQuestion)DialogFrame.Content).Tag = this;
            }

        }
        private void SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            CursoCreadoSnackbar.IsActive = false;
            NoQuizesSnackbar.IsActive = false;
        }

        public void GoUpCurrentTab(int courseId)
        {
            RefreshTopicOfOpenTab(courseId);
        }

        public void CreateQuiz(string quizName, int topicId)
        {
            int createdQuizId = service.CreateQuiz(quizName, 0, -1, 10.0, DateTime.Now.Date, 1, topicId, false, 0);
            EditQuiz crearQuizWindow = new(service, createdQuizId, personEmail, this);
            DialogHost.CloseDialogCommand.Execute(null, null);
            RefreshQuizesList(createdQuizId);
            crearQuizWindow.ShowDialog();
        }

        public void CopyQuiz(string quizName, int quizId, int topicId)
        {
            service.CopyQuizNormal(quizName, quizId, topicId);
            DialogHost.CloseDialogCommand.Execute(null, null);
            LoadAllQuizesView();
            try
            {
                ((Frame)((TabItem)InstructorTabControl.SelectedItem).Content).LoadCompleted += QuizSelectFrame_LoadCompleted;
                ((Frame)((TabItem)InstructorTabControl.SelectedItem).Content).Content = new VistaSeleccionQuiz(service, topicId, service.GetCourseOfTopic(topicId), personEmail);
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void CopyCourse(string CourseName, int cursoId)
        {
            int newCourseId = service.CopyCoursePrototype(CourseName, cursoId, personEmail);
            LoadCourse(newCourseId);
            LoadAllQuizesView();
            DialogHost.CloseDialogCommand.Execute(null, null);
            CursoCreadoSnackbar.IsActive = true;
        }

        private void DarkModeSwitch_Click(object sender, RoutedEventArgs e)
        {
            ToggleBaseColour((bool)DarkModeSwitch.IsChecked);
        }

        private void ToggleBaseColour(bool isDark)
        {
            PaletteHelper _paletteHelper = new PaletteHelper();
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        public void Logout()
        {
            MainWindow window = new MainWindow(service, DarkModeSwitch.IsChecked.Value);
            window.Show();
            this.Close();
        }

        private void ProfileFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            (e.Content as AccountPage).Tag = this;
        }

        public void ShowEditCourseDialog(int courseId)
        {
            EditCourseDialog dialog = new EditCourseDialog(service, courseId);
            DialogFrame.Content = dialog;
            DialogHost.OpenDialogCommand.Execute(null, null);
        }
        public void HideEditCourseDialog(int courseId)
        {
            int itemToOpen = InstructorTabControl.SelectedIndex;
            ClearCoursesTabs();
            LoadCoursesTabs();
            DialogHost.CloseDialogCommand.Execute(null, null);
            InstructorTabControl.SelectedIndex = itemToOpen;
        }

        private void BuscadorPreguntasButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["PrincipalTitulo"];
            opcionesButton.Content = Idioma.info["opcionesButton"];
            GreetingTextBlock.Text = Idioma.info["GreetingTextBlock"];
            InitTab.Text = Idioma.info["InitTab"];
            profileTab.Text = Idioma.info["profileTab"];
            morningMessage = Idioma.info["morningMessage"];
            afterMessage = Idioma.info["afterMessage"];
            eveningMessage = Idioma.info["eveningMessage"];
            errorIncribe = Idioma.info["errorIncribe"];
            principalTittle = Idioma.info["principalTittle"];
            quizNameCol = Idioma.info["quizNameCol"];
            quizDateCreateCol = Idioma.info["quizDateCreateCol"];
            quizAutorCol = Idioma.info["quizAutorCol"];
            quizInitDateCol = Idioma.info["quizInitDateCol"];
            quizClosingDateCol = Idioma.info["quizClosingDateCol"];
            quizNumAttemptsCol = Idioma.info["quizNumAttemptsCol"];
            quizReanudando = Idioma.info["quizReanudando"];
            information = Idioma.info["information"];
            volverRealizar = Idioma.info["volverRealizar"];
            maxRealizaciones = Idioma.info["maxRealizaciones"];
            sinPermisoResultado = Idioma.info["sinPermisoResultado"];
            TabButtonPlus.ToolTip = Idioma.info["TabButtonPlus"];
            SnackSinQuiz.Content = Idioma.info["SnackSinQuiz"];
            CursoCreadoSnack.Content = Idioma.info["CursoCreadoSnack"];
            SetTitleMessage();

            ReloadEverything();
        }

        private void ReloadEverything()
        {
            int maxTabs = InstructorTabControl.Items.Count;
            for (int i = 3; i < maxTabs; i++)
            {
                if (InstructorTabControl.Items[3] is TabItem)
                {
                    InstructorTabControl.Items.RemoveAt(3);
                }
            }

            if (!PersonIsStudent())
            {
                LoadQuestions();
            }
            SetWindowTitle();
            LoadInstructorProfileTab();
            LoadAllQuizesView();
            LoadCoursesTabs();
        }

        private void opcionesIdioma(object sender, RoutedEventArgs e)
        {
            CambiarIdioma ci = new CambiarIdioma(service);
            ci.ShowDialog();
            actualizar();
        }
        public void RefreshQuizesList(int quizId)
        {
            LoadAllQuizesView();
            foreach (TabItem tab in tabs)
            {
                if (tab.Tag.ToString() == service.GetCourseOfTopic(service.GetTopicFromQuiz(quizId)).ToString())
                {
                    Frame contentFrame = new Frame();
                    contentFrame.LoadCompleted += QuizSelectFrame_LoadCompleted;
                    contentFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
                    contentFrame.Content = new VistaSeleccionQuiz(service, service.GetTopicFromQuiz(quizId), service.GetCourseOfTopic(service.GetTopicFromQuiz(quizId)), personEmail);
                    contentFrame.Margin = new Thickness(20, 10, 10, 10);
                    tab.Content = contentFrame;
                }
            }
            CloseDialog();
        }
        public void CloseDialog()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }


    }

    public struct QuizItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreationDate { get; set; }
        public string Author { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string NumberOfTries { get; set; }
        public int CourseId { get; set; }
        public int Status { get; set; }
    }

    public struct TopicItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
    }
    public struct CourseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public struct QuestionItem
    {
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public string Autor { get; set; }
        public string Curso { get; set; }
        public string Contenido { get; set; }
        public int Type { get; set; }
        public double Puntos { get; set; }
    }
    public struct BateriaItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumQuestions { get; set; }
        public int Type { get; set; }
    }
}
