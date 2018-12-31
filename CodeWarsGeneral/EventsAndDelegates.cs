using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
public class Program
{
    static List<string> messagesSent = new List<string>();

    // Metoda dzięki, której wyświetlimy wszystkie informacje w konsoli
    static void DisplayInformation(object source, PersonEventArgs e)
    {
        Console.WriteLine(e.Name + " ");
        messagesSent.Add(e.Name);
    }

    static void Main(string[] args)
    {
        List<string> peopleList = new List<string>()
            {
                "Peter", "Mike", "Peter", "Bob", "Peter", "Peter", "Bob", "Mike", "Bob", "Peter", "Peter", "Mike", "Bob"
            };

        if (messagesSent.Count > 0)
            messagesSent.Clear();

        Publisher publisher = new Publisher();
        publisher.ContactNotify += DisplayInformation;
        publisher.CountMessages(peopleList);

        Console.ReadKey();
    }
}
*/

namespace CodeWarsGeneral
{
    public class PersonEventArgs : EventArgs
    {
        private string name;
        public string Name { get { return name; } set { name = value; } }
        public PersonEventArgs(string name)
        {
            this.name = name;
        }
    }

    public class Publisher
    {
        public delegate void ContactHandler(object source, PersonEventArgs e);

        public event ContactHandler ContactNotify;

        public void CountMessages(List<string> peopleList)
        {
            Dictionary<string, int> namesOccurences = new Dictionary<string, int>();
            foreach (string person in peopleList)
            {
                if (!namesOccurences.ContainsKey(person))
                {
                    namesOccurences.Add(person, 1);
                }
                else
                {
                    namesOccurences[person]++;
                    if (namesOccurences[person] > 2)
                    {
                        OnContactNotify(person); // Notify subscribers
                        namesOccurences[person] = 0;
                    }
                }
            }
        }

        public void OnContactNotify(string person)
        {
            if (ContactNotify != null)
                ContactNotify(this, new PersonEventArgs(person));
        }
    }


    public class EventsAndDelegates
    {

    }
}
