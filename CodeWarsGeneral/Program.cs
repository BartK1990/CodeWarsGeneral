using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{
    public class Program
    {
        static void Main(string[] args)
        {
            PokerHandTest("6S AD 7H 4S AS", "AH AC 5H 6H 7S");


        }

        public static void PokerHandTest(string hand, string opponentHand)
        {
            Result i = new PokerHand(hand).CompareWith(new PokerHand(opponentHand));
        }
    }
}
