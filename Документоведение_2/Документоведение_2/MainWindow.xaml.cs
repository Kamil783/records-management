using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;

namespace Документоведение_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public List<Card> Cards = new List<Card>();
        readonly DataSet _dataSet = new DataSet();
        public MainWindow()
        {
            InitializeComponent();
            _dataSet.ReadXml("chek.xml", XmlReadMode.ReadSchema);
            MainDataGrid.DataContext = _dataSet.Tables["Карточки"];
            MainDataGrid.ItemsSource = _dataSet.Tables["Карточки"].DefaultView;
            ((DataGridTextColumn)MainDataGrid.Columns[0]).Binding = new Binding("НомерГонщика");
            ((DataGridTextColumn)MainDataGrid.Columns[1]).Binding = new Binding("ФИОГонщика");
            ((DataGridTextColumn)MainDataGrid.Columns[2]).Binding = new Binding("Конструктор");
            ((DataGridTextColumn)MainDataGrid.Columns[3]).Binding = new Binding("Болид");
        }

        private Card NewCard(int number, string name)
        {
            var dataRow = _dataSet.Tables["Карточки"].Rows.Find(new object[] { (object)number, (object)name });
            var card = new Card(
                (int)dataRow["НомерГонщика"],
                (string)dataRow["ФИОГонщика"],
                (string)dataRow["Конструктор"],
                (string)dataRow["Болид"],
                (int)dataRow["Возраст"],
                (string)dataRow["СтранаРождения"]
                );

            card.Add();

            return card;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var aw = new AddWindow();
            aw.ShowDialog();

            aw.ReturnData(out var card);

            var newRow = _dataSet.Tables["Карточки"].NewRow();
            // newRow["Номер"] = dataSet.Tables["Карточки"].Rows.Count + 1;
            newRow["НомерГонщика"] = card.Number;
            newRow["ФИОГонщика"] = card.Name;
            newRow["Конструктор"] = card.Team;
            newRow["Болид"] = card.Car;
            newRow["Возраст"] = card.Age;
            newRow["СтранаРождения"] = card.Country;

            _dataSet.Tables["Карточки"].Rows.Add(newRow);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(MainDataGrid.SelectedIndex == -1)
                return;
            //var number =  MainDataGrid.SelectedCells[0].Item.ToString();
            //var name = MainDataGrid.SelectedCells[1].Item.ToString();
            //var firstSelectedCellContent = this.MainDataGrid.Columns[0].GetCellContent(this.MainDataGrid.SelectedItem);
            //var firstSelectedCell = firstSelectedCellContent != null ? firstSelectedCellContent.Parent as DataGridCell : null;
            //var number = MainDataGrid.SelectedIndex;
            var cells = (DataRowView) MainDataGrid.SelectedItem;
           // int index = MainDataGrid.CurrentCell.Column.DisplayIndex;
            var number = cells.Row.ItemArray[0].ToString();
            var name = cells.Row.ItemArray[1].ToString();
            var dataRow = _dataSet.Tables["Карточки"].Rows.Find(new object[]{(object)number, (object) name});
            dataRow.Delete();
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex == -1)
                return;
            var cells = (DataRowView)MainDataGrid.SelectedItem;
            var number = cells.Row.ItemArray[0].ToString();
            var name = cells.Row.ItemArray[1].ToString();
            var dataRow = _dataSet.Tables["Карточки"].Rows.Find(new object[] { (object)number, (object)name });
            var dw = new DetailWindow(dataRow, _dataSet);
            dw.Show();
        }

        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex == -1)
                return;
            var cells = (DataRowView)MainDataGrid.SelectedItem;
            var number = Convert.ToInt32(cells.Row.ItemArray[0].ToString());
            var name = cells.Row.ItemArray[1].ToString();
            var card = NewCard(number, name);

            var app = new Application();
            var doc = app.Documents.Add();
            var paragraph = doc.Content.Paragraphs.Add();

            paragraph.Range.Text = "№ " + number + " " + name;
            paragraph.Range.Font.Bold = 1;
            paragraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Format.SpaceAfter = 20;
            paragraph.Range.InsertParagraphAfter();

            paragraph = doc.Content.Paragraphs.Add();
            paragraph.Range.Text = "Конструктор: " + card.Team;
            paragraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Format.SpaceAfter = 20;
            paragraph.Range.InsertParagraphAfter();

            paragraph = doc.Content.Paragraphs.Add();
            paragraph.Range.Text = "Болид: " + card.Car;
            paragraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Format.SpaceAfter = 20;
            paragraph.Range.InsertParagraphAfter();

            paragraph = doc.Content.Paragraphs.Add();

            var tab = doc.Tables.Add(paragraph.Range, card.List.Count + 1, 6);
            tab.Borders.Enable = 1;
            tab.Cell(1,1).Range.Text = "Номер";
            tab.Cell(1, 2).Range.Text = "Имя";
            tab.Cell(1, 3).Range.Text = "Конструктор";
            tab.Cell(1, 4).Range.Text = "Болид";
            tab.Cell(1, 5).Range.Text = "Возраст";
            tab.Cell(1, 6).Range.Text = "Страна Рождения";

            for (int i = 0; i < card.List.Count; i++)
            {
                tab.Cell(i + 2, 1).Range.Text = (card.List[i] as Card)?.Number.ToString();
                tab.Cell(i + 2, 2).Range.Text = (card.List[i] as Card)?.Name;
                tab.Cell(i + 2, 3).Range.Text = (card.List[i] as Card)?.Team;
                tab.Cell(i + 2, 4).Range.Text = (card.List[i] as Card)?.Car;
                tab.Cell(i + 2, 5).Range.Text = (card.List[i] as Card)?.ToString();
                tab.Cell(i + 2, 6).Range.Text = (card.List[i] as Card)?.Country;
            }

            doc.Save();

            app.Visible = true;
        }

        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex == -1)
                return;
            var cells = (DataRowView)MainDataGrid.SelectedItem;
            var number = Convert.ToInt32(cells.Row.ItemArray[0].ToString());
            var name = cells.Row.ItemArray[1].ToString();
            var card = NewCard(number, name);

            var dlg = new SaveFileDialog
            {
                FileName = "Документ", DefaultExt = ".xml", Filter = "Xml документ (.xml)|*.xml"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result != true) return;
            var fileStream = new FileStream(dlg.FileName, FileMode.Create);

            var ser = new SoapFormatter();
            ser.Serialize(fileStream, card);
            fileStream.Close();

            MessageBox.Show("Сформирован файл " +dlg.FileName, "Файл сохранен", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void VerificationButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex == -1)
                return;
            var cells = (DataRowView)MainDataGrid.SelectedItem;
            var number = Convert.ToInt32(cells.Row.ItemArray[0].ToString());
            var name = cells.Row.ItemArray[1].ToString();
            var card = NewCard(number, name);

            var ser = new SoapFormatter();
            var memStream = new MemoryStream();

            ser.Serialize(memStream, card);
            memStream.Seek(0, SeekOrigin.Begin);

            var message = new byte[memStream.Length];
            memStream.Read(message, 0, (int)memStream.Length);

            try
            {
                var binReader = new BinaryReader(new FileStream("card" + number + ".dat", FileMode.Open));

                var key = binReader.ReadString();
                var iSign = binReader.ReadInt32();
                var bSign = binReader.ReadBytes(iSign);

                binReader.Close();

                var dsa = new DSACryptoServiceProvider();
                dsa.FromXmlString(key);

                if (dsa.VerifyData(message, bSign))
                    MessageBox.Show("Верификация карточки пройдена", "Результат верификации", MessageBoxButton.OK,
                        MessageBoxImage.Asterisk);
                else
                    MessageBox.Show("Верификация карточки не пройдена", "Результат верификации", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
            catch 
            {
                MessageBox.Show("Верификация карточки не пройдена", "Результат верификации", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SignatureButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex == -1)
                return;
            var cells = (DataRowView)MainDataGrid.SelectedItem;
            var number = Convert.ToInt32(cells.Row.ItemArray[0].ToString());
            var name = cells.Row.ItemArray[1].ToString();
            var card = NewCard(number, name);

            var ser = new SoapFormatter();
            var memStream = new MemoryStream();

            ser.Serialize(memStream, card);
            memStream.Seek(0, SeekOrigin.Begin);

            var message = new byte[memStream.Length];
            memStream.Read(message, 0, (int)memStream.Length);

            var dsa = new DSACryptoServiceProvider();
            var signature = dsa.SignData(message);
            var key = dsa.ToXmlString(true);

            try
            {

                var binWriter = new BinaryWriter(new FileStream("card" + card.Number + ".dat", FileMode.Create));

                binWriter.Write(key);
                binWriter.Write(signature.Length);
                binWriter.Write(signature);
                binWriter.Close();

                MessageBox.Show("Создана цифровая подпись", "Цифровая подпись", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось создать цифровую подпись", "Цифровая подпись", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _dataSet.WriteXml("chek.xml", XmlWriteMode.WriteSchema);
        }
    }


   
}
