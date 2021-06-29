using System;
using System.Collections.Generic;
using System.Linq;

public struct Card : ICloneable
{
    public Card(int value, char suit)
    {
        Value = value;
        Suit = suit;
    }
    public int Value;
    public char Suit;

    public object Clone()
    {
        return this;
    }
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
public class PokerHand : IComparable<PokerHand>
{
    public string Cards { get; private set; }
    public int Strength { get; private set; }
    public List<Card> CardsList { get; set; }
    public List<Card> CardsListSortedByStrength { get; set; }

    private Dictionary<char, int> values = new Dictionary<char, int>()
        {
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
        Cards = cards;
        CardsList = ReadHand(this);
        Strength = HandStrength(CardsList);
        var cardsSortedByStrength = CardsList.OrderByDescending(n => CardsList.Count(x => x.Value == n.Value)).ThenByDescending(n => n.Value);
        CardsListSortedByStrength = cardsSortedByStrength.ToList();
    }
    public List<Card> ReadHand(PokerHand hand)
    {
        List<Card> returnHand = new List<Card>();
        foreach (string s in hand.Cards.Split(' '))
        {
            returnHand.Add(new Card(values[s[0]], s[1]));
        }
        return returnHand;
    }
    public bool IfStraight(List<Card> cards)
    {
        CardComparer_byValue SortCardsByVal = new CardComparer_byValue();
        CardEqualityComparer_byValue CompareEqualityByVal = new CardEqualityComparer_byValue();
        List<Card> cardsToTest = cards.Clone();
        bool aceReplaced = false;
        // For possibility of straight with ACE as 1
        int acePos = 0;
        if (cardsToTest.Contains(new Card(14, 'H'), CompareEqualityByVal) && cardsToTest.Contains(new Card(2, 'H'), CompareEqualityByVal))
        {
            for (acePos = 0; acePos < cardsToTest.Count; acePos++)
            {
                if (cardsToTest[acePos].Value == 14)
                {
                    cardsToTest.Add(new Card(1, cardsToTest[acePos].Suit));
                    cardsToTest.RemoveAt(acePos);
                    aceReplaced = true;
                    break;
                }
            }
        }
        cardsToTest.Sort(SortCardsByVal);
        int previousCardVal = cardsToTest[0].Value;
        for (int i = 1; i < cardsToTest.Count; i++)
        {
            if (previousCardVal - 1 != cardsToTest[i].Value)
                return false;
            previousCardVal = cardsToTest[i].Value;
        }
        // if straigth, leave ace 
        if (aceReplaced)
        {
            cards.Add(new Card(1, cardsToTest[acePos].Suit));
            cards.RemoveAt(acePos);
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
    public int CompareTo(PokerHand hand)
    {
        if (this.Strength > hand.Strength)
            return -1;
        if (this.Strength < hand.Strength)
            return 1;

        for (int i = 0; i < this.CardsListSortedByStrength.Count; i++)
        {
            if (this.CardsListSortedByStrength[i].Value > hand.CardsListSortedByStrength[i].Value)
                return -1;
            if (this.CardsListSortedByStrength[i].Value < hand.CardsListSortedByStrength[i].Value)
                return 1;
        }
        return 0;
    }
}

public static class Extensions
{
    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}