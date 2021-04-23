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

namespace Документоведение_2
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
       
        public AddWindow()
        {
            InitializeComponent();
        }

        public void ReturnData(out Card result)
        {
            var card = new Card
            {
                Number = Convert.ToInt32(NumberTextBox.Text),
                Name = NameTextBox.Text,
                Team = TeamTextBox.Text,
                Car = CarTextBox.Text,
                Age = Convert.ToInt32(AgeTextBox.Text),
                Country = CountryTextBox.Text
            };
            result = card;
        }

        private void SenButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
