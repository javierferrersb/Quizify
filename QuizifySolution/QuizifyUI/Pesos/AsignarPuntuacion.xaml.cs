using Quizify.Services;
using System;
using System.Windows;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para AsignarPuntuación.xaml
    /// </summary>
    public partial class AsignarPuntuacion : Window
    {
        private IQuizifyService service;
        private int idNB;
        private string confirmChanges = "Confirmar cambios";
        private string sureToSave = "¿Estás seguro de guardar los cambios?";
        public AsignarPuntuacion(IQuizifyService service, int idB)
        {
            this.service = service;
            InitializeComponent();
            this.idNB = idB;
            difMode.Text = selectMode2(service.GetDificultadNotaBateriaById(idNB));
            mark.Text = selectMark2(service.GetNotaNotaBateriaById(idNB));
            actualizar();
            this.idNB = idB;
            difMode.Text = selectMode2(service.GetDificultadNotaBateriaById(idNB));
            mark.Text = selectMark2(service.GetNotaNotaBateriaById(idNB));
        }

        private int selectMode(String option)
        {
            if (option.Equals(MuyBaja.Content.ToString())) return 0;
            if (option.Equals(Baja.Content.ToString())) return 1;
            if (option.Equals(Media.Content.ToString())) return 2;
            if (option.Equals(Alta.Content.ToString())) return 3;
            return 4;
        }
        private String selectMode2(int option)
        {
            switch (option)
            {
                case 0:
                    return MuyBaja.Content.ToString();
                case 1:
                    return Baja.Content.ToString();
                case 2:
                    return Media.Content.ToString();
                case 3:
                    return Alta.Content.ToString();
                default:
                    return MuyAlta.Content.ToString();
            }
        }

        private double selectMark(String option)
        {
            switch (option)
            {
                case "0.5":
                    return 0.5;
                case "0.75":
                    return 0.75;
                case "1":
                    return 1;
                case "1.5":
                    return 1.5;
                default:
                    return 2;
            }
        }

        private string selectMark2(double option)
        {
            switch (option)
            {
                case 0.5:
                    return "0.5";
                case 0.75:
                    return "0.75";
                case 1:
                    return "1";
                case 1.5:
                    return "1.5";
                default:
                    return "2";
            }
        }

        private void saveChange(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmResult = MessageBox.Show(confirmChanges, sureToSave, MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                service.SetNotaBateriaWeight(idNB, selectMode(difMode.Text), selectMark(mark.Text));
                Close();
            }
            else
            {
                Close();
            }
        }

        private void cancelChange(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["PuntuacionTitulo"];
            DificultLabel.Content = Idioma.info["DificultLabel"];
            Points.Content = Idioma.info["Points"];
            confirmChanges = Idioma.info["confirmChanges"];
            sureToSave = Idioma.info["sureToSave"];
            MuyAlta.Content = Idioma.info["MuyAlta"];
            Alta.Content = Idioma.info["Alta"];
            Media.Content = Idioma.info["Media"];
            Baja.Content = Idioma.info["Baja"];
            MuyBaja.Content = Idioma.info["MuyBaja"];
            cancelButton.Content = Idioma.info["cancelButton"];
            saveButton.Content = Idioma.info["saveButton"];
        }
    }
}
