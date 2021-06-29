using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Kata
{
    public static long ListPosition(string value)
    {
        if (string.IsNullOrEmpty(value))
            return 0;
        CapitalLettersList capitalLettersList = new CapitalLettersList(value);
        Dictionary<byte, int> letterCountDictionary = new Dictionary<byte, int>();
        long listPosition = 0;

        int lettersCount = capitalLettersList.LettersList.Count;
        for (int c = 0; c < lettersCount; c++)
        {
            listPosition += capitalLettersList.CountNumberOfAnagramsTillLetter(capitalLettersList.LettersList[0]);
            capitalLettersList.RemoveFirstLetterAndRecalculate();
        }
        return listPosition + 1;
    }
}

public class CapitalLettersList
{
    public List<byte> LettersList { get; private set; }
    public Dictionary<byte, int> LetterCountDictionary { get; private set; }

    public CapitalLettersList(string lettersList)
    {
        LettersList = Encoding.ASCII.GetBytes(lettersList).ToList();
        CreateLetterCountSortedDictionary();
    }

    public void RemoveFirstLetterAndRecalculate()
    {
        LettersList.RemoveAt(0);
        CreateLetterCountSortedDictionary();
    }

    public long CountNumberOfAnagramsForLetter(byte letter)
    {
        if (LettersList.Count < 1)
            return 0;
        if (LetterCountDictionary == null)
            CreateLetterCountSortedDictionary();
        long nominator = Strong(LettersList.Count - 1);
        long denominator = 1;
        foreach (var g in LetterCountDictionary)
        {
            if (g.Value <= 1)
                continue;
            if (g.Key != letter)
                denominator *= Strong(g.Value);
            else
                denominator *= Strong(g.Value - 1);
        }
        return nominator / denominator;
    }

    public long CountNumberOfAnagramsTillLetter(byte letter)
    {
        long result = 0;
        foreach (var lcd in LetterCountDictionary)
        {
            if (lcd.Key >= letter)
                break;
            result += CountNumberOfAnagramsForLetter(lcd.Key);
        }
        return result;
    }

    private void CreateLetterCountSortedDictionary()
    {
        LetterCountDictionary = new Dictionary<byte, int>();
        var query = LettersList.Select(x => x)
            .GroupBy(s => s)
            .Select(g => new { Name = g.Key, Count = g.Count() }).OrderBy(o => o.Name);
        foreach (var q in query)
            LetterCountDictionary.Add(q.Name, q.Count);
    }

    private long Strong(long n)
    {
        if (n == 1)
            return 1;
        if (n > 1)
            return Strong(n - 1) * n;
        return -1;
    }
}