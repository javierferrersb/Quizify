using Quizify.Services;
using System.Windows;
using System.Windows.Controls;

namespace QuizifyUI
{
    /// <summary>
    /// Interaction logic for ExistingQuizDialog.xaml
    /// </summary>
    public partial class EditBattery : Page
    {
        private int batteryId;
        private string email;
        private IQuizifyService service;
        private int quizId;
        public EditBattery(IQuizifyService service, int batteryId, string email, int quizId)
        {
            this.quizId = quizId;
            InitializeComponent();
            actualizar();
            this.service = service;
            this.email = email;
            this.batteryId = batteryId;

            ShowQuizName();
        }

        private void ShowQuizName()
        {
            batteryName.Text = service.GetNameBatteryById(batteryId);
        }

        private void EditBatteryButton_Click(object sender, RoutedEventArgs e)
        {
            EdicionBateria b = new EdicionBateria(service, email, batteryId, quizId);
            b.ShowDialog();
        }

        private void asignarPeso_Click(object sender, RoutedEventArgs e)
        {
            AsignarPuntuacion ap = new AsignarPuntuacion(service, service.GetNotaBateriaIdFromBatteryAndQuiz(quizId, batteryId));
            ap.ShowDialog();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            EditBatteryButton.Content = Idioma.info["EditBatteryButton"];
            asignarPesoButton.Content = Idioma.info["asignarPesoButton"];
        }
    }
}
