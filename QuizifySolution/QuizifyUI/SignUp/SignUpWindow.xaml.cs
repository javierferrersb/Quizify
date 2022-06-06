using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System;
using System.Net.Mail;
using System.Windows;

namespace QuizifyUI.SignUp
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private IQuizifyService service;
        public SignUpWindow(IQuizifyService service)
        {
            InitializeComponent();
            this.service = service;
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            HintAssist.SetHint(PasswordTextBox, Idioma.info["PasswordField"]);
            HintAssist.SetHint(ConfirmPasswordTextBox, Idioma.info["ConfirmPasswordTextBox"]);
            HintAssist.SetHint(NameTextBox, Idioma.info["FullNameTextBox"]);
            HintAssist.SetHint(EmailTextBox, Idioma.info["EmailTextBox"]);
            InstructorRadioButton.Content = Idioma.info["InstructorRadioButton"];
            StudentRadioButton.Content = Idioma.info["StudentRadioButton"];
            SignUpButton.Content = Idioma.info["SignUpButton"];
            SignUpTitleTextBox.Text = Idioma.info["SignUpTitleTextBox"];
            UserTypeTextBlock.Text = Idioma.info["UserTypeTextBlock"];

            SetWindowTitle();
        }

        private void SetWindowTitle()
        {
            this.Title = Idioma.info["SignUpTitleTextBox"];
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                MessageBox.Show(Idioma.info["EmptyNameErrorMessage"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!MailAddress.TryCreate(EmailTextBox.Text, out _))
            {
                MessageBox.Show(Idioma.info["IncorrectEmailErrorMessage"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (PasswordTextBox.Password == "")
            {
                MessageBox.Show(Idioma.info["EmptyPasswordErrorMessage"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (PasswordTextBox.Password != ConfirmPasswordTextBox.Password)
            {
                MessageBox.Show(Idioma.info["IncorrectPasswordErrorMessage"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (StudentRadioButton.IsChecked.Value)
                {
                    service.SignUp(0,NameTextBox.Text, EmailTextBox.Text, PasswordTextBox.Password);
                }
                else if (InstructorRadioButton.IsChecked.Value)
                {
                    service.SignUp(1,NameTextBox.Text, EmailTextBox.Text, PasswordTextBox.Password);
                }
                else
                {
                    MessageBox.Show(Idioma.info["EmptyRoleErrorMessage"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (ServiceException)
            {
                MessageBox.Show(Idioma.info["EmailInUseErrorMessage"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show(Idioma.info["SignUpCompleteMessage"], "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
