using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Документоведение_2
{
    [Serializable]
    public class Card
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Team { get; set; }
        public string Car { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        //public List<Card> List = new List<Card>();
        public ArrayList List = new ArrayList();

        public Card()
        {
            Number = 0;
            Name = "";
            Team = "";
            Car = "";
            Age = 0;
            Country = "";
        }

        public Card(int number, string name, string team, string car, int age, string country)
        {
            this.Number = number;
            this.Name = name;
            this.Team = team;
            this.Car = car;
            this.Age = age;
            this.Country = country;
        }

        public void Add()
        {
            List.Add(this);
        }

        
    }
}
