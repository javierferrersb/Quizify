using MaterialDesignThemes.Wpf;
using Quizify.Services;
using QuizifyUI.SignUp;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IQuizifyService service;
        public bool persona;
        private PaletteHelper _paletteHelper = new PaletteHelper();
        bool dataLoadedBool;
        Task<bool> dataLoaded;

        public MainWindow()
        {
            InitializeComponent();
            dataLoaded = new Task<bool>(() =>
            {
                return false;
            });
            Idioma.cambiarIdioma(new Properties.Settings().lang);
            actualizar();
            //LoginButton.KeyDown
            dataLoadedBool = false;
            service = new QuizifyService();
        }

        public MainWindow(IQuizifyService service, bool darkMode)
        {
            InitializeComponent();
            actualizar();
            dataLoadedBool = true;
            this.service = service;
            this.darkModeSwitch.IsChecked = darkMode;
            MouseEnter -= MainWindowElement_MouseEnter;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            login();
        }
        private async void LoginEnter_Pressed(object sender, KeyEventArgs e)
        {
            //if((KeyDown(e.Source)).Equals(Key.Enter) )
            if (e.Key.Equals(Key.Enter))
            {
                login();
            }
        }
        private void EnableAllButtons()
        {
            Mouse.OverrideCursor = null;
            LoadingBar.Visibility = Visibility.Hidden;
            EmailField.IsEnabled = true;
            PasswordField.IsEnabled = true;
            LoginButton.IsEnabled = true;
            SignUpButton.IsEnabled = true;
            StudentExampleLoginButton.IsEnabled = true;
            InstructorExampleLoginButton.IsEnabled = true;
            sampleDbButton.IsEnabled = true;
        }

        private void DisableAllButtons()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            LoadingBar.Visibility = Visibility.Visible;

            EmailField.IsEnabled = false;
            PasswordField.IsEnabled = false;
            LoginButton.IsEnabled = false;
            StudentExampleLoginButton.IsEnabled = false;
            InstructorExampleLoginButton.IsEnabled = false;
            sampleDbButton.IsEnabled = false;
            SignUpButton.IsEnabled = false;
        }

        private async void login()
        {
            try
            {
                DisableAllButtons();

                string email = EmailField.Text;
                string password = PasswordField.Password;

                while (!dataLoadedBool)
                {
                    await Task.Run(() =>
                    {
                        string hello = "";
                    });
                }

                await Task.Run(() =>
                {

                    persona = service.GetPerson(email, password);
                });
                EnableAllButtons();

                VistaPrincipal vistaInstructorWindow = new(service, EmailField.Text, (bool)darkModeSwitch.IsChecked);
                vistaInstructorWindow.Show();
                this.Close();
            }
            catch (ServiceException exception)
            {
                EnableAllButtons();

                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void sampleDbButton_Click(object sender, RoutedEventArgs e)
        {
            DisableAllButtons();

            while (!dataLoadedBool)
            {
                await Task.Run(() =>
                {
                    string hello = "";
                });
            }

            service.FillExampleData();

            EnableAllButtons();
        }
        private void InstructorExampleLoginButton_Click(object sender, RoutedEventArgs e)
        {
            EmailField.Text = "pepe@pepe.com";
            PasswordField.Password = "pepe";
            LoginButton_Click(sender, e);
        }

        private void StudentExampleLoginButton_Click(object sender, RoutedEventArgs e)
        {
            EmailField.Text = "manolo@manolo.com";
            PasswordField.Password = "manolo";
            LoginButton_Click(sender, e);
        }

        private void ToggleBaseColour(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleBaseColour((bool)darkModeSwitch.IsChecked);
        }
        private void actualizar()
        {
            HintAssist.SetHint(PasswordField, Idioma.info["PasswordField"]);
            SignUpButton.Content = Idioma.info["SignUpButton"];
        }

        private async void MainWindowElement_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnter -= MainWindowElement_MouseEnter;
            await Task.Run(() =>
            {
                dataLoaded = service.StartDBTask();
            });
            dataLoadedBool = dataLoaded.Result;
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            DisableAllButtons();

            while (!dataLoadedBool)
            {
                await Task.Run(() =>
                {
                    string hello = "";
                });
            }

            EnableAllButtons();
            SignUpWindow window = new(service);
            window.ShowDialog();
        }
    }
}
