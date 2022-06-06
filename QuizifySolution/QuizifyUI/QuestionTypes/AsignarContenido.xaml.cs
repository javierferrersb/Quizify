using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Quizify.Services;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para AsignarContenido.xaml
    /// </summary>
    public partial class AsignarContenido : Window
    {
        private IQuizifyService service;

        private List<ContenidoItem> availableContenido;
        private int preguntaId;
        private PreguntaItem preg;
        private ContenidoItem contenido;

        private string contenidoHeader = "Contenido";
        private string contenidoNoSeleccionado = "Contenido no seleccionado";
        public AsignarContenido(IQuizifyService s, PreguntaItem preg)
        {
            InitializeComponent();
            actualizar();//A implementar

            this.service = s;
            this.preguntaId = preg.Id;
            this.preg = preg;
            string? contenidoPerteneciente = preg.Competencia;//service.GetContenidoByPregunta(preg.Id);
            if (contenidoPerteneciente != "")
            {
                ContenidoSelected.Text = contenidoPerteneciente;
            }
            else
            {
                ContenidoSelected.Text = contenidoNoSeleccionado;
            }
            availableContenido = new List<ContenidoItem>();


            generateColumns();
        }

        public void generateColumns()
        {
            ContenidosGrid.Columns.Clear();

            System.Windows.Controls.DataGridTextColumn contenidoCol = new();

            contenidoCol.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            contenidoCol.Header = contenidoHeader;

            contenidoCol.IsReadOnly = true;

            contenidoCol.Binding = new Binding("Nombre");

            ContenidosGrid.Columns.Add(contenidoCol);

            ContenidosGrid.MouseDoubleClick += QuestionsDataGrid_MouseDoubleClick;

            setContenidosToGrid();
        }

        private void setContenidosToGrid()
        {
            List<List<string>> aux = service.GetAllTopicsString();

            foreach(List<string> contenido in aux)
            {
                ContenidoItem cItem = (new ContenidoItem
                {
                    Id = int.Parse(contenido.ElementAt(0)),
                    Nombre = contenido.ElementAt(1)
                });
                availableContenido.Add(cItem);
            }
            ContenidosGrid.ItemsSource = availableContenido;
        }

        private void QuestionsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow? contentSelected = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (contentSelected == null) return;

            contenido = ((ContenidoItem)ContenidosGrid.SelectedItem);

            this.preg.Competencia = contenido.Nombre;
            ContenidoSelected.Text = contenido.Nombre;
        }

        private void GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            service.SetTopicToQuestion(contenido.Id, preguntaId);
        }


        private void actualizar()
        {
            //Idioma.Controles(this);
            contenidoHeader = Idioma.info["contenidoHeader"];
            contenidoNoSeleccionado = Idioma.info["contenidoNoSeleccionado"];
            SaveButton.Content = Idioma.info["saveButton"];
            contenidoLabel.Text = Idioma.info["contenidoLabel"];
        }

    }

    public struct ContenidoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

}
