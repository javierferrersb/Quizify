using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuizifyUI.Pesos
{
    /// <summary>
    /// Lógica de interacción para AsignarCompetencia.xaml
    /// </summary>
    public partial class AsignarCompetencia : Window
    {
        public AsignarCompetencia()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
