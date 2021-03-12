using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Документоведение
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSet = new DataSet();
            var card = new DataTable("Карточки");
            var constructor = new DataTable("Команда");
            var car = new DataTable("Болид");

            dataSet.Tables.Add(card);

            var dataColumn = new DataColumn("НомерГонщика", Type.GetType("System.Int32"));
            dataSet.Tables["Карточки"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("ФИОГонщика", Type.GetType("System.String"));
            dataSet.Tables["Карточки"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("Конструктор", Type.GetType("System.String"));
            dataSet.Tables["Карточки"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("Болид", Type.GetType("System.String"));
            dataSet.Tables["Карточки"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("Возраст", Type.GetType("System.Int32"));
            dataSet.Tables["Карточки"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("СтранаРождения", Type.GetType("System.String"));
            dataSet.Tables["Карточки"].Columns.Add(dataColumn);

            var key = new DataColumn[2]
                {
                    dataSet.Tables["Карточки"].Columns["Конструктор"],
                    dataSet.Tables["Карточки"].Columns["ФИОГонщика"]
                };
            dataSet.Tables["Карточки"].PrimaryKey = key;

            dataSet.Tables.Add(car);

            dataColumn = new DataColumn("Болид", Type.GetType("System.String"));
            dataSet.Tables["Болид"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("СиловаяУстановка", Type.GetType("System.String"));
            dataSet.Tables["Болид"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("Производитель", Type.GetType("System.String"));
            dataSet.Tables["Болид"].Columns.Add(dataColumn);

            key = new DataColumn[3]
            {
                dataSet.Tables["Болид"].Columns["Болид"],
                dataSet.Tables["Болид"].Columns["СиловаяУстановка"],
                dataSet.Tables["Болид"].Columns["Производитель"]
            };
            dataSet.Tables["Болид"].PrimaryKey = key;

            dataSet.Tables.Add(constructor);

            dataColumn = new DataColumn("Конструктор", Type.GetType("System.String"));
            dataSet.Tables["Команда"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("ГонщикN1", Type.GetType("System.String"));
            dataSet.Tables["Команда"].Columns.Add(dataColumn);

            dataColumn = new DataColumn("ГонщикN2", Type.GetType("System.String"));
            dataSet.Tables["Команда"].Columns.Add(dataColumn);

            key = new DataColumn[3]
            {
                dataSet.Tables["Команда"].Columns["Конструктор"],
                dataSet.Tables["Команда"].Columns["ГонщикN1"],
                dataSet.Tables["Команда"].Columns["ГонщикN2"]
            };
            dataSet.Tables["Команда"].PrimaryKey = key;

            dataSet.Relations.Add("СвязьБолида", dataSet.Tables["Карточки"].Columns["Болид"],
                dataSet.Tables["Болид"].Columns["Болид"]);

            dataSet.Relations.Add("СвязьКоманды", dataSet.Tables["Карточки"].Columns["Конструктор"],
                dataSet.Tables["Команда"].Columns["Конструктор"]);

            dataSet.WriteXml("chek.xml", XmlWriteMode.WriteSchema);
        }
    }
}
