using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CodeWarsGeneral
{
    public class Kata
    {
        public static char[] specialChars = {'^', '/', '*', '+', '-'};

        public static double calculate(string s)
        {
            int leftBracketIndex, rightBracketIndex;
            string beforeLeftBracket, insideBrackets, afterRightBracket;
            // clear string
            string stringToCalculate = Regex.Replace(s, @"\s+", "");

            // Until there are brackets
            while (Regex.IsMatch(stringToCalculate, @"[\(\)]"))
            {
                rightBracketIndex = stringToCalculate.IndexOf(")");
                leftBracketIndex = stringToCalculate.LastIndexOf("(", rightBracketIndex);
                beforeLeftBracket = stringToCalculate.Substring(0, leftBracketIndex);
                insideBrackets = stringToCalculate.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
                afterRightBracket = stringToCalculate.Substring(rightBracketIndex + 1);

                stringToCalculate = beforeLeftBracket + calculateNoBrackets(insideBrackets) + afterRightBracket;
            }
            stringToCalculate = calculateNoBrackets(stringToCalculate);
            return double.Parse(stringToCalculate);
        }
        
        public static string calculateNoBrackets(string s)
        {
            double result=0;
            int specialIndex, previousSpecialIndex, nextSpecialIndex;
            string beforePreviousSpecialIndex="", afterNextSpecialIndex="";
            double decimalToCalculate1, decimalToCalculate2;

            for (int i = 0; i<specialChars.Length; i++)
            {
                while ((specialIndex = s.IndexOf(specialChars[i])) >= 0)
                {
                    if (specialIndex == 0)
                    {
                        specialIndex = s.IndexOf(specialChars[i], 1);
                        if(specialIndex == -1) // for negative
                        {
                            result = double.Parse(s);
                            break;
                        }
                    }
                    if ((previousSpecialIndex = s.LastIndexOfAny(specialChars, specialIndex - 1)) == -1)
                        beforePreviousSpecialIndex = "";
                    else // Check negative values
                    {
                        if (previousSpecialIndex == 0)
                            previousSpecialIndex = -1;
                        else
                        {
                            int previousPreviousSpecialIndex;
                            if ((previousPreviousSpecialIndex = s.LastIndexOfAny(specialChars, previousSpecialIndex - 1)) != -1)
                            {
                                if ((previousSpecialIndex - previousPreviousSpecialIndex) == 1)
                                    previousSpecialIndex = previousPreviousSpecialIndex;
                            }
                            else
                                beforePreviousSpecialIndex = s.Substring(0, previousSpecialIndex + 1);
                        }
                    }
                    if ((nextSpecialIndex = s.IndexOfAny(specialChars, specialIndex + 1)) == -1)
                        nextSpecialIndex = s.Length;
                    else // for negatives
                    {
                        if((nextSpecialIndex - specialIndex) == 1)
                        {
                            if ((nextSpecialIndex = s.IndexOfAny(specialChars, nextSpecialIndex + 1)) == -1)
                                nextSpecialIndex = s.Length;
                        }
                    }
                    afterNextSpecialIndex = s.Substring(nextSpecialIndex);
                    decimalToCalculate1 = double.Parse(s.Substring(previousSpecialIndex + 1, specialIndex - previousSpecialIndex - 1));
                    decimalToCalculate2 = double.Parse(s.Substring(specialIndex + 1, nextSpecialIndex - specialIndex - 1));
                    switch (i)
                    {
                        case 0:
                            result = Math.Pow(decimalToCalculate1, decimalToCalculate2);
                            break;
                        case 1:
                            result = decimalToCalculate1 / decimalToCalculate2;
                            break;
                        case 2:
                            result = decimalToCalculate1 * decimalToCalculate2;
                            break;
                        case 3:
                            result = decimalToCalculate1 + decimalToCalculate2;
                            break;
                        case 4:
                            result = decimalToCalculate1 - decimalToCalculate2;
                            break;
                    }
                    s = beforePreviousSpecialIndex + result.ToString() + afterNextSpecialIndex;
                }
            }
            return result.ToString();
        }
    }
}
