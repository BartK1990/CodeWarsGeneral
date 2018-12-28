using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeWarsGeneral
{
    using System.Numerics;

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
}
