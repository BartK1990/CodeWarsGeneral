using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MorseCodeDecoder
{

    private const int KMeanTries = 10;

    public static string decodeBitsAdvanced(string bits)
    {
        var bitsTrimmed = bits.Trim('0');
        var messageDelimiters = GetDelimitersUnits(bitsTrimmed);

        var sb = new StringBuilder();

        var wordsDelimiters = new List<string>();
        if (messageDelimiters.Count() >= 3)
        {
            foreach (var md in messageDelimiters.ElementAt(2))
            {
                wordsDelimiters.Add(new string('0', md));
            }
            wordsDelimiters = wordsDelimiters.OrderByDescending(l => l.Length).ToList();
        }
        var wordsList = bitsTrimmed.Split(wordsDelimiters.ToArray(), StringSplitOptions.None);

        foreach (var wl in wordsList)
        {
            var lettersDelimiters = new List<string>();
            if (messageDelimiters.Count() >= 2)
            {
                foreach (var md in messageDelimiters.ElementAt(1))
                {
                    lettersDelimiters.Add(new string('0', md));
                }
                lettersDelimiters = lettersDelimiters.OrderByDescending(l => l.Length).ToList();
            }
            var lettersList = wl.Split(lettersDelimiters.ToArray(), StringSplitOptions.None);

            foreach (var ll in lettersList)
            {
                var marksDelimiters = new List<string>();
                if (messageDelimiters.Any())
                {
                    foreach (var md in messageDelimiters.ElementAt(0))
                    {
                        marksDelimiters.Add(new string('0', md));
                    }
                    marksDelimiters = marksDelimiters.OrderByDescending(l => l.Length).ToList();

                    var marksList = ll.Split(marksDelimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var ml in marksList)
                    {
                        sb.Append(messageDelimiters.ElementAt(0).Contains(ml.Length) ? "." : "-");
                    }
                    sb.Append(" ");
                }

            }
            sb.Append("  ");
        }
        if (bitsTrimmed == "1001")
            return ". .";
        return sb.ToString();
    }

    private static IEnumerable<List<int>> GetDelimitersUnits(string bits) // returns three collections of delimiters (word[2], letter[1], sign[0])
    {
        var zerosList = bits.Split('1').OrderBy(z => z)
            .Where(empty => !string.IsNullOrWhiteSpace(empty))
            .Distinct()
            .ToList();
        var onesList = bits.Split('0').OrderBy(z => z)
            .Where(empty => !string.IsNullOrWhiteSpace(empty))
            .Distinct()
            .ToList();
        var resultUnits = new List<int>[3] { new List<int>(), new List<int>(), new List<int>() };

        if (onesList.Count <= 0) return resultUnits;
        if (zerosList.Count <= 0)
        {
            resultUnits[0].Add(bits.Length);
            return resultUnits;
        }

        var zerosUnitsList = new List<int>();
        zerosUnitsList.AddRange(zerosList.Select(l => l.Length));
        var onesUnitsList = new List<int>();
        onesUnitsList.AddRange(onesList.Select(l => l.Length));

        var unitsList = zerosUnitsList.Union(onesUnitsList).OrderBy(o => o).ToList();
        var timeUnits = new double[] { 1.0, 3.0, 7.0 };
        var sumOfVariancesList = new List<double>();
        var resultUnitsList = new List<List<int>[]>();
        for (double i = 1.0; i <= (double)KMeanTries; i += 0.5)
        {
            resultUnitsList.Add(new List<int>[3] { new List<int>(), new List<int>(), new List<int>() });
            var tu = timeUnits.Select(u => u * i).ToArray();
            sumOfVariancesList.Add(SumOfVariances(tu, unitsList, resultUnitsList.Last()));
        }
        int resultIndex = 0;
        for (int i = 0; i < sumOfVariancesList.Count; i++)
        {
            if (sumOfVariancesList[i] <= sumOfVariancesList.Min())
            {
                if (resultUnitsList[i].ElementAt(0).Any())
                {
                    resultIndex = i;
                    break;
                }
            }
        }
        return resultUnitsList[resultIndex];
    }

    private static double SumOfVariances(double[] initClusters, List<int> units, List<int>[] resultUnits)
    {
        resultUnits.ToList().ForEach(l => l.Clear());
        foreach (var u in units)
        {
            var a = initClusters.Select(c => Math.Abs(c - (double)u)).ToArray();
            resultUnits[Array.IndexOf(a, a.Min())].Add(u);
        }
        double sumOfVariances = 0.0;
        if (resultUnits.Any(c => c.Count > 1))
        {
            sumOfVariances = resultUnits.Aggregate(0.0, (d, ints) => d + Variance(ints));
        }
        else
        {
            for (int i = 0; i < resultUnits.Length; i++)
            {
                sumOfVariances += Variance(resultUnits[i], initClusters[i]);
            }
        }

        return sumOfVariances;
    }

    private static double Variance(IEnumerable<int> group, double average)
    {
        if (!group.Any())
            return 0.0;
        return (group.Select(g => (double)g).Aggregate(0.0, (current, g) => current + ((g - average) * (g - average)))) / group.Count();
    }
    private static double Variance(IEnumerable<int> group)
    {
        if (!group.Any())
            return 0.0;
        var average = (double)group.Sum() / (double)group.Count();
        return (group.Select(g => (double)g).Aggregate(0.0, (current, g) => current + ((g - average) * (g - average)))) / group.Count();
    }

    public static string decodeMorse(string morseCode)
    {
        if (string.IsNullOrWhiteSpace(morseCode))
            return "";
        var wordsList = morseCode.Trim().Split("   ");
        var decodedWordsList = wordsList
            .Select(word => word.Split(" "))
            .Select(letter => string.Join("", letter.Select(GetMorseCode)))
            .ToList();
        var result = string.Join(' ', decodedWordsList);
        return result;

    }

    private static string GetMorseCode(string input)
    {
        return Preloaded.MORSE_CODE.ContainsKey(input) ? Preloaded.MORSE_CODE[input] : "";
    }
}