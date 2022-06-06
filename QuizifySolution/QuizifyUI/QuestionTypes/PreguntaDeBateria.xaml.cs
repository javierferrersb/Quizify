using Quizify.Services;
using System.Windows.Controls;

namespace QuizifyUI
{

    /// <summary>
    /// Lógica de interacción para PreguntaDeBatería.xaml
    /// </summary>
    public partial class PreguntaDeBateria : Page
    {
        private IQuizifyService service;
        public PreguntaDeBateria(IQuizifyService ser, int batteryId)
        {
            InitializeComponent();
            this.service = ser;
            QuestionStatement.Text = service.GetNameBatteryById(batteryId);
            QuestionType.Text = service.GetQuestionTypeFromBateria(batteryId).ToString();

        }
    }
}
