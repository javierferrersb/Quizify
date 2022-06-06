using Quizify.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizifyUI
{
    /// <summary>
    /// Lógica de interacción para GestionBateriaPreguntas.xaml
    /// </summary>
    public partial class SeleccionarBateria : Window
    {
        private IQuizifyService service;
        private string personEmail;
        private int qId;
        private string nombreHeader = "Nombre";
        private string tipoPHeader = "Tipo Pregunta";
        private string quizHeader = "Quiz";
        private string cursoHeader = "Curso";
        private List<Battery> items;
        private ICollectionView itemsView;
        private string questionVF = "Verdadero/Falso";
        private string questionOpen = "Abierta";
        private string questionMult = "Selección multiple";
        private string questionCor = "Correspondencia";
        private string questionPar = "Parametrizada";
        public SeleccionarBateria(IQuizifyService service, string personEmail)
        {
            this.personEmail = personEmail;
            InitializeComponent();
            actualizar();
            this.service = service;
            items = new List<Battery>();
            itemsView = new ListCollectionView(items);
            setBatteryListView();
            BatteryTable.CanUserAddRows = false;
            CreateAllBatteryColumns();
            if(BatteryTable.Items.Count > 0)
            {
                createBatteryBut.Visibility = Visibility.Hidden;
            }
        }

        private void CreateAllBatteryColumns()
        {
            BatteryTable.Columns.Clear();

            System.Windows.Controls.DataGridTextColumn Nombre = new System.Windows.Controls.DataGridTextColumn();
            System.Windows.Controls.DataGridTextColumn TipoP = new System.Windows.Controls.DataGridTextColumn();
            System.Windows.Controls.DataGridTextColumn Quiz = new System.Windows.Controls.DataGridTextColumn();
            System.Windows.Controls.DataGridTextColumn Curso = new System.Windows.Controls.DataGridTextColumn();


            Nombre.Header = nombreHeader;
            TipoP.Header = tipoPHeader;
            Quiz.Header = quizHeader;
            Curso.Header = cursoHeader;

            Nombre.IsReadOnly = true;
            TipoP.IsReadOnly = true;
            Quiz.IsReadOnly = true;
            Curso.IsReadOnly = true;


            Nombre.Binding = new Binding("Nombre");
            TipoP.Binding = new Binding("tipoPregunta");
            Quiz.Binding = new Binding("quizAsociado");
            Curso.Binding = new Binding("cursoAsociado");

            BatteryTable.Columns.Add(Nombre);
            BatteryTable.Columns.Add(TipoP);
            BatteryTable.Columns.Add(Quiz);
            BatteryTable.Columns.Add(Curso);

        }


        private void setBatteryListView()
        {
            List<List<String>> list = service.GetBatteryStringList();
            string tipo = null;
            foreach (List<String> listOfQuestionInfo in list)
            {
                if (service.getCountQuestionOfBattery(int.Parse(listOfQuestionInfo.ElementAt(0))) > 1)
                {
                    if (listOfQuestionInfo.ElementAt(3).Equals("Verdadero/Falso"))
                    {
                        tipo = questionVF;
                    }
                    else if (listOfQuestionInfo.ElementAt(3).Equals("Abierta"))
                    {
                        tipo = questionOpen;
                    }
                    else if (listOfQuestionInfo.ElementAt(3).Equals("Parametrizada"))
                    {
                        tipo = questionPar;
                    }
                    else if (listOfQuestionInfo.ElementAt(3).Equals("Correspondencia"))
                    {
                        tipo = questionCor;
                    }
                    else
                    {
                        tipo = questionMult;
                    }
                    Battery item = (new Battery
                    {
                        ID = int.Parse(listOfQuestionInfo.ElementAt(0)),
                        IDQuiz = int.Parse(listOfQuestionInfo.ElementAt(1)),
                        Nombre = listOfQuestionInfo.ElementAt(2),
                        tipoPregunta = tipo,
                        quizAsociado = listOfQuestionInfo.ElementAt(4),
                        cursoAsociado = listOfQuestionInfo.ElementAt(5)
                    });
                    items.Add(item);
                }
            }


            itemsView.Filter += new Predicate<object>(QuestionFilter);

            itemsView.Refresh();

            BatteryTable.ItemsSource = itemsView;
        }

        private bool QuestionFilter(Object item)
        {
            return item is Battery obj &&
                obj.cursoAsociado.ToLower().Contains(cursoText.Text.ToLower()) &&
                obj.quizAsociado.ToLower().Contains(quizText.Text.ToLower()) &&
                obj.tipoPregunta.ToLower().Contains(tipoPreguntaText.Text.ToLower()) &&
                obj.Nombre.ToLower().Contains(buscador.Text.ToLower());
        }

        private void createBattery_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void selectBattery_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();

        }

        private void DialogFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
        private void actualizar()
        {
            //Idioma.Controles(this);
            this.Title = Idioma.info["SelectBatTitulo"];
            nombreHeader = Idioma.info["nombreHeader"];
            tipoPHeader = Idioma.info["tipoPHeader"];
            quizHeader = Idioma.info["quizHeader"];
            cursoHeader = Idioma.info["cursoHeader"];
            findName.Content = Idioma.info["findByName"];
            filtroPregLabel.Content = Idioma.info["filtroPregLabel"];
            quizName.Content = Idioma.info["quizName"];
            filtroCursoLabel.Content = Idioma.info["filtroCursoLabel"];
            volverBut.Content = Idioma.info["volverBut"];
            createBatteryBut.Content = Idioma.info["createBatteryBut"];
            questionVF = Idioma.info["questionVF"];
            questionOpen = Idioma.info["questionOpen"];
            questionMult = Idioma.info["questionMult"];
            questionCor = Idioma.info["questionCor"];
            questionPar = Idioma.info["questionPar"];
        }

        private void buscador_TextChanged(object sender, TextChangedEventArgs e)
        {
            itemsView.Refresh();
        }
    }

}
