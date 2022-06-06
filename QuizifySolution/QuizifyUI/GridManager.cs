using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace QuizifyUI
{
    public class GridManager
    {

        public static void AddGridColumn(DataGrid Grid, string columnHeader, string BindingString)
        {
            DataGridTextColumn col = new()
            {
                Header = columnHeader,
                IsReadOnly = true,
                Binding = new Binding(BindingString)
            };
            Grid.Columns.Add(col);
        }
    }
}
