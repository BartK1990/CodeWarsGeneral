using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandsSortable
{
    class Program
    {
        static void Main(string[] args)
        {
            // Arrange
            var expected = new List<PokerHand> {
            new PokerHand("KS AS TS QS JS"),
            new PokerHand("2H 3H 4H 5H 6H"),
            new PokerHand("AS AD AC AH JD"),
            new PokerHand("JS JD JC JH 3D"),
            new PokerHand("2S AH 2H AS AC"),
            new PokerHand("AS 3S 4S 8S 2S"),
            new PokerHand("2H 3H 5H 6H 7H"),
            new PokerHand("2S 3H 4H 5S 6C"),
            new PokerHand("2D AC 3H 4H 5S"),
            new PokerHand("AH AC 5H 6H AS"),
            new PokerHand("2S 2H 4H 5S 4C"),
            new PokerHand("AH AC 5H 6H 7S"),
            new PokerHand("AH AC 4H 6H 7S"),
            new PokerHand("2S AH 4H 5S KC"),
            new PokerHand("2S 3H 6H 7S 9C")
        };
            var random = new Random((int)DateTime.Now.Ticks);
            var actual = expected.OrderBy(x => random.Next()).ToList();
            // Act
            Console.WriteLine("Before Sort:");
            for (var i = 0; i < expected.Count; i++)
                Console.WriteLine(expected[i].Cards + " - " + actual[i].Cards);

            // Sort
            actual.Sort();

            Console.WriteLine();
            Console.WriteLine("After Sort:");
            for (var i = 0; i < expected.Count; i++)
                Console.WriteLine(expected[i].Cards + " - " + actual[i].Cards);

            Console.ReadKey();
        }
    }
}
