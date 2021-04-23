using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace Документоведение_2
{
    /// <summary>
    /// Логика взаимодействия для DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public ObservableCollection<Card> MySource { get; set; }



        public DetailWindow(DataRow dataRow, DataSet dataSet)
        {
            InitializeComponent();
            MainDataGrid.DataContext = dataSet.Tables["Карточки"];
            


            this.MySource = new ObservableCollection<Card>
            {
                new Card
                {
                    Number = (int) dataRow[0],
                    Name = (string) dataRow[1],
                    Team = (string) dataRow[2],
                    Car = (string) dataRow[3],
                    Age = (int) dataRow[4],
                    Country = (string) dataRow[5]
                }
            };

            MainDataGrid.ItemsSource = this.MySource;
            ((DataGridTextColumn)MainDataGrid.Columns[0]).Binding = new Binding("Number");
            ((DataGridTextColumn)MainDataGrid.Columns[1]).Binding = new Binding("Name");
            ((DataGridTextColumn)MainDataGrid.Columns[2]).Binding = new Binding("Team");
            ((DataGridTextColumn)MainDataGrid.Columns[3]).Binding = new Binding("Car");
            ((DataGridTextColumn)MainDataGrid.Columns[4]).Binding = new Binding("Age");
            ((DataGridTextColumn)MainDataGrid.Columns[5]).Binding = new Binding("Country");

        }
    }
}
