using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{

    public enum Result
    {
        Win,
        Loss,
        Tie
    }
    public struct Card
    {
        public Card(int value, char suit)
        {
            Value = value;
            Suit = suit;
        }
        public int Value;
        public char Suit;
    }
    public class CardComparer_byValue : IComparer<Card>
    {
        public int Compare(Card x, Card y)
        {
            if (x.Value > y.Value)
                return -1;
            if (x.Value < y.Value)
                return 1;
            return 0;
        }
    }
    public class CardEqualityComparer_byValue : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x.Value == y.Value;
        }
        public int GetHashCode(Card obj)
        {
            return obj.Value.GetHashCode();
        }
    }
    public class CardEqualityComparer_bySuit : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x.Suit == y.Suit;
        }
        public int GetHashCode(Card obj)
        {
            return obj.Suit.GetHashCode();
        }
    }

    public class PokerHand
    {
        private string cards;
        private Dictionary<char, int> values = new Dictionary<char, int>()
        {
            {'1', 1 },
            {'2', 2 },
            {'3', 3 },
            {'4', 4 },
            {'5', 5 },
            {'6', 6 },
            {'7', 7 },
            {'8', 8 },
            {'9', 9 },
            {'T', 10 },
            {'J', 11 },
            {'Q', 12 },
            {'K', 13 },
            {'A', 14 }
        };

        public PokerHand(string cards)
        {
            this.cards = cards;
        }

        public List<Card> ReadHand(PokerHand hand)
        {
            List<Card> returnHand = new List<Card>();
            foreach (string s in hand.cards.Split(' '))
            {
                returnHand.Add(new Card(values[s[0]], s[1]));
            }
            return returnHand;
        }
        public bool IfStraight(List<Card> cards)
        {
            CardComparer_byValue SortCardsByVal = new CardComparer_byValue();
            List<Card> cardsToTest = cards;
            cardsToTest.Sort(SortCardsByVal);
            int previousCardVal = cardsToTest[0].Value;
            for(int i=1; i< cardsToTest.Count; i++)
            {
                if (previousCardVal - 1 != cardsToTest[i].Value)
                    return false;
                previousCardVal = cardsToTest[i].Value;
            }
            return true;
        }

        //from 0(High Card) till 8(Straight Flush)
        public int HandStrength(List<Card> cards)
        {
            CardEqualityComparer_bySuit CompareEqualityBySuit = new CardEqualityComparer_bySuit();
            CardEqualityComparer_byValue CompareEqualityByVal = new CardEqualityComparer_byValue();
            int i = cards.Distinct(CompareEqualityBySuit).ToList().Count;
            int j = cards.Distinct(CompareEqualityByVal).ToList().Count;

            if ((cards.Distinct(CompareEqualityBySuit).ToList().Count == 1) && IfStraight(cards))
                return 8;
            if ((cards.Distinct(CompareEqualityByVal).ToList().Count == 2))
            {
                var cardsSortedByStrength = cards.OrderByDescending(n => cards.Count(x => x.Value == n.Value)).ThenByDescending(n => n.Value);
                List<Card> cardsSortedByStrengthList = cardsSortedByStrength.ToList();
                if (cardsSortedByStrengthList.Where(x => x.Value.Equals(cardsSortedByStrengthList[0].Value)).Count() == 4)
                    return 7;
                else
                    return 6;
            }
            if (cards.Distinct(CompareEqualityBySuit).ToList().Count == 1)
                return 5;
            if (IfStraight(cards))
                return 4;
            if ((cards.Distinct(CompareEqualityByVal).ToList().Count == 3))
            {
                var cardsSortedByStrength = cards.OrderByDescending(n => cards.Count(x => x.Value == n.Value)).ThenByDescending(n => n.Value);
                List<Card> cardsSortedByStrengthList = cardsSortedByStrength.ToList();
                if (cardsSortedByStrengthList.Where(x => x.Value.Equals(cardsSortedByStrengthList[0].Value)).Count() == 3)
                    return 3;
                else
                    return 2;
            }
            if ((cards.Distinct(CompareEqualityByVal).ToList().Count == 4))
                return 1;
            return 0;
        }


        public Result CompareWith(PokerHand hand)
        {
            CardComparer_byValue SortCardsByVal = new CardComparer_byValue();
            CardEqualityComparer_byValue CompareEqualityByVal = new CardEqualityComparer_byValue();
            List<Card> firstHand = ReadHand(this);
            List<Card> secondHand = ReadHand(hand);

            int HS1 = HandStrength(firstHand);
            int HS2 = HandStrength(secondHand);

            if (HS1 > HS2)
                return Result.Win;
            if (HS1 < HS2)
                return Result.Loss;

            var FHSortedByStrength = firstHand.OrderByDescending(n => firstHand.Count(x => x.Value == n.Value)).ThenByDescending(n => n.Value);
            List<Card> FHSortedByStrengthList = FHSortedByStrength.ToList();
            var SHSortedByStrength = secondHand.OrderByDescending(n => secondHand.Count(x => x.Value == n.Value)).ThenByDescending(n => n.Value);
            List<Card> SHSortedByStrengthList = SHSortedByStrength.ToList();
            for(int i=0; i<FHSortedByStrengthList.Count; i++)
            {
                if(FHSortedByStrengthList[i].Value > SHSortedByStrengthList[i].Value)
                    return Result.Win;
                if (FHSortedByStrengthList[i].Value < SHSortedByStrengthList[i].Value)
                    return Result.Loss;
            }
            return Result.Tie;
        }
    }

}
