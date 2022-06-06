using MaterialDesignThemes.Wpf;
using Quizify.Services;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizifyUI.QuizStatus
{
    /// <summary>
    /// Interaction logic for BorradorStatus.xaml
    /// </summary>
    public partial class BorradorStatus : Page
    {
        private static readonly Regex _numberRegex = new Regex("[^0-9]+"); //regex that matches only integer numbers
        private IQuizifyService service;
        private int quizId;
        private string snackM1 = "La fecha de inicio no puede ser mayor a la fecha de finalización";
        private string snackM2 = "Debe establecer fecha de finalización";
        private string snackM3 = "Debe establecer fecha de inicio";
        private string snackM4 = "Debe establecer fecha de inicio y de finalización";
        private string snackM5 = "Debe establecer el número de intentos";
        private string snackM6 = "Debe establecer el límite de tiempo";
        private string textoLanzamiento = "";
        private string textoError = "";
        private string denyText = "";
        private string OKText = "";

        public BorradorStatus(IQuizifyService service, int quizId, bool lanzadoPreviemante)
        {
            InitializeComponent();
            actualizar();
            this.service = service;

            this.quizId = quizId;

            LimitTimeCheckBox.IsChecked = true;
            LimitTriesCheckBox.IsChecked = true;
            AnswerModeComboBox.SelectedIndex = 0;

            if (lanzadoPreviemante)
            {
                StatusTextBlock.Text = "Borrador - Cancelado";
            }
        }

        private void LimitTriesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NumberOfTriesField.Text = "1";
            NumberOfTriesField.IsEnabled = true;
        }

        private void LimitTriesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NumberOfTriesField.Text = "";
            NumberOfTriesField.IsEnabled = false;
        }

        private void LimitTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TimeField.Text = "20";
            TimeField.IsEnabled = true;
        }

        private void LimitTimeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeField.Text = "";
            TimeField.IsEnabled = false;
        }

        private void TimeField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void NumberOfTriesField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            return !_numberRegex.IsMatch(text);
        }

        private void LaunchQuizButton_Click(object sender, RoutedEventArgs e)
        {
            bool dataCorrect = true;
            if (StartingDatePicker.SelectedDate > EndingDatePicker.SelectedDate)
            {
                ErrorDataSnackBarMessage.Content = snackM1;
                ErrorDataSnackBar.IsActive = true;
                EndingDatePicker.Focus();
                dataCorrect = false;
            }
            else if (StartingDatePicker.SelectedDate != null && EndingDatePicker.SelectedDate == null)
            {
                ErrorDataSnackBarMessage.Content = snackM2;
                ErrorDataSnackBar.IsActive = true;
                EndingDatePicker.Focus();
                dataCorrect = false;
            }
            else if (StartingDatePicker.SelectedDate == null && EndingDatePicker.SelectedDate != null)
            {
                ErrorDataSnackBarMessage.Content = snackM3;
                ErrorDataSnackBar.IsActive = true;
                StartingDatePicker.Focus();
                dataCorrect = false;
            }
            else if (StartingDatePicker.SelectedDate == null && EndingDatePicker.SelectedDate == null)
            {
                ErrorDataSnackBarMessage.Content = snackM4;
                ErrorDataSnackBar.IsActive = true;
                StartingDatePicker.Focus();
                dataCorrect = false;
            }

            if ((bool)LimitTriesCheckBox.IsChecked && NumberOfTriesField.Text == "")
            {
                ErrorDataSnackBarMessage.Content = snackM5;
                ErrorDataSnackBar.IsActive = true;
                NumberOfTriesField.Focus();
                dataCorrect = false;
            }

            if ((bool)LimitTimeCheckBox.IsChecked && TimeField.Text == "")
            {
                ErrorDataSnackBarMessage.Content = snackM6;
                ErrorDataSnackBar.IsActive = true;
                TimeField.Focus();
                dataCorrect = false;
            }
            if (dataCorrect)
            {
                service.LaunchQuiz(quizId, (DateTime)StartingDatePicker.SelectedDate, (DateTime)EndingDatePicker.SelectedDate, (bool)ShowResultCheckBox.IsChecked, (bool)RandomQuestionsCheckBox.IsChecked, (bool)LimitTriesCheckBox.IsChecked ? int.Parse(NumberOfTriesField.Text) : -1, (bool)LimitTimeCheckBox.IsChecked ? int.Parse(TimeField.Text) : -1, AnswerModeComboBox.SelectedIndex);
                ((QuizStatusManager)this.Tag).LoadContent();
            }
            else
            {
                Pack.Kind = PackIconKind.Error;
                InfoLanzamiento.Text = textoError;
                AcceptLaunchButton.Visibility = Visibility.Hidden;
                DenyLaunchButton.Content = OKText;
            }
        }
        private void RestartButton(object sender, RoutedEventArgs e)
        {
            Pack.Kind = PackIconKind.InformationCircle;
            InfoLanzamiento.Text = textoLanzamiento;
            AcceptLaunchButton.Visibility = Visibility.Visible;
            DenyLaunchButton.Content = denyText;
        }

        private void ErrorDataSnackBarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            ErrorDataSnackBar.IsActive = false;
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            ShowResultCheckBox.Content = Idioma.info["ShowResultCheckBox"];
            RandomQuestionsCheckBox.Content = Idioma.info["RandomQuestionsCheckBox"];
            LimitTriesCheckBox.Content = Idioma.info["LimitTriesCheckBox"];
            LimitTimeCheckBox.Content = Idioma.info["LimitTimeCheckBox"];
            VolverAtras.Content = Idioma.info["VolverAtras"];
            sinVolverAtras.Content = Idioma.info["sinVolverAtras"];
            LaunchQuizButton.Content = Idioma.info["LaunchQuizButton"];
            StatusMessageTextBlock.Text = Idioma.info["StatusMessageTextBlock"];
            StatusTextBlock.Text = Idioma.info["StatusTextBlock"];
            snackM1 = Idioma.info["snackM1"];
            snackM2 = Idioma.info["snackM2"];
            snackM3 = Idioma.info["snackM3"];
            snackM4 = Idioma.info["snackM4"];
            snackM5 = Idioma.info["snackM5"];
            snackM6 = Idioma.info["snackM6"];
            HintAssist.SetHint(StartingDatePicker, Idioma.info["StartingDatePicker"]);
            HintAssist.SetHint(EndingDatePicker, Idioma.info["EndingDatePicker"]);
            HintAssist.SetHint(NumberOfTriesField, Idioma.info["NumberOfTriesField"]);
            HintAssist.SetHint(TimeField, Idioma.info["TimeField"]);
            HintAssist.SetHint(AnswerModeComboBox, Idioma.info["AnswerModeComboBox"]);
            textoLanzamiento = Idioma.info["InfoLanzamiento"];
            textoError = Idioma.info["InfoLanzamientoError"];
            denyText = Idioma.info["DenyLaunchButton"];
            OKText = Idioma.info["DenyLaunchButtonOK"];
            if (Pack.Kind.Equals(PackIconKind.InformationCircle))
            {
                InfoLanzamiento.Text = textoLanzamiento;
                DenyLaunchButton.Content = denyText;
            }
            else
            {
                InfoLanzamiento.Text = textoError;
                DenyLaunchButton.Content = OKText;
            }
            AcceptLaunchButton.Content = Idioma.info["AcceptLaunchButton"];
        }

        private void LaunchQuizButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
