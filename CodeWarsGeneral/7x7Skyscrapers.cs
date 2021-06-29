using System;
using System.Collections.Generic;
using System.Linq;

public class Skyscrapers
{
    private const int Dimension = 7;
    private readonly int[][] _permutationsArrayInts;
    private readonly List<int>[,] _restrictedNumbers; // first row for rows, second for columns
    private int _fieldsSum = 0;

    public Skyscrapers()
    {
        _permutationsArrayInts = new int[Strong(Dimension)][];
        _restrictedNumbers = new List<int>[2, Dimension];
        for (int c = 0; c < _restrictedNumbers.GetLength(0); c++)
            for (int i = 0; i < _restrictedNumbers.GetLength(1); i++)
                _restrictedNumbers[c, i] = new List<int>();
        List<int> elements = new List<int>();
        for (int c = 1; c <= Dimension; c++)
            elements.Add((c));
        int index = 0;
        Permute(_permutationsArrayInts, ref index, elements, 0, Dimension - 1);
    }

    public static int[][] SolvePuzzle(int[] clues)
    {
        Skyscrapers skyscrapers = new Skyscrapers();

        int finishedSum = 0;
        for (int c = 1; c <= Dimension; c++)
            finishedSum += c;
        finishedSum *= Dimension; // Total sum of finished Puzzle

        int[][] fields = new int[Dimension][];
        for (int c = 0; c < fields.Length; c++)
            fields[c] = new int[Dimension];

        List<int[]>[,] possibleLinesLists = skyscrapers.CheckLines(fields, clues);
        while (skyscrapers._fieldsSum < finishedSum) // until a solution is found
            skyscrapers.CheckFields(fields, clues, possibleLinesLists);
        return fields;
    }

    private void CheckFields(int[][] fields, int[] clues, List<int[]>[,] possibleLinesLists)
    {
        List<int>[,] possibleFieldValuesLists = new List<int>[Dimension, Dimension];
        for (int i = 0; i < Dimension; i++)
        {
            for (int j = 0; j < Dimension; j++)
            {
                var rowField = Enumerable.Range(0, possibleLinesLists[0, i].Count)
                    .Select(x => (possibleLinesLists[0, i])[x][j]).Distinct();
                var columnField = Enumerable.Range(0, possibleLinesLists[1, j].Count)
                    .Select(x => (possibleLinesLists[1, j])[x][i]).Distinct();

                var result = rowField.Intersect(columnField).ToList();
                if (result.Count() == 1)
                    FillField(fields, i, j, result.First());
                possibleFieldValuesLists[i, j] = result;
            }
        }

        for (int c = 0; c < Dimension * 2; c++)
        {
            int direction = (c < Dimension ? 0 : 1);
            int position = (c < Dimension ? c : c - Dimension);
            for (int i = 0; i < possibleLinesLists[direction, position].Count; i++)
            {
                if (!MatchLineWithPossibleFieldsValues(possibleLinesLists[direction, position][i],
                    direction == 0
                        ? GetRow(possibleFieldValuesLists, position)
                        : GetColumn(possibleFieldValuesLists,
                            position)))
                    possibleLinesLists[direction, position].RemoveAt(i);
            }
        }
    }

    private List<int[]>[,] CheckLines(int[][] fields, int[] clues)
    {
        List<int[]>[,] possibleLinesLists = new List<int[]>[2, Dimension];
        int direction = 1;
        for (int c = 0; c < Dimension; c++) // columns
            possibleLinesLists[direction, c] =
                CheckLine(direction, c, fields, clues[c], clues[(Dimension * 3) - c - 1]);
        direction = 0;
        for (int c = 0; c < Dimension; c++) // rows
            possibleLinesLists[direction, c] = CheckLine(direction, c, fields, clues[Dimension * 4 - c - 1],
                clues[Dimension + c]);
        return possibleLinesLists;
    }

    private List<int[]>
        CheckLine(int direction, int lineNumber, int[][] fields, int leftClue,
            int rightClue) // direction 0 - row, 1 - column
    {
        List<int[]> possibileLines = new List<int[]>();
        int[] line = (direction == 0 ? GetRow(fields, lineNumber) : GetColumn(fields, lineNumber));
        foreach (int[] permutation in _permutationsArrayInts)
        {
            if (!MatchPermuteToLine(line, permutation))
                continue;
            bool continueFlag = false;
            for (int c = 0; c < Dimension; c++) // crossing lines
            {
                if (line[c] > 0) continue; // restriction rules do not apply to already filled places
                if (_restrictedNumbers[direction == 0 ? 1 : 0, c].Contains(permutation[c]))
                {
                    continueFlag = true;
                    break;
                }
            }

            if (continueFlag)
                continue;
            if (leftClue > 0)
                if (leftClue != NumberOfVisibleSkyscrapers(false, permutation))
                    continue;
            if (rightClue > 0)
                if (rightClue != NumberOfVisibleSkyscrapers(true, permutation))
                    continue;

            possibileLines.Add(permutation); // if pass everything add to possible permutations
        }

        for (int c = 0; c < Dimension; c++)
        {
            if (line[c] > 0) continue; // if field is already filled we don't have to bother
            var result = Enumerable.Range(0, possibileLines.Count)
                .Select(x => possibileLines[x][c]).Distinct();
            int cordX, cordY;
            if (direction == 0)
            {
                cordX = lineNumber;
                cordY = c;
            }
            else
            {
                cordX = c;
                cordY = lineNumber;
            }

            if (result.Count() == 1)
            {
                var resultInt = result.First();
                FillField(fields, cordX, cordY, resultInt);
            }
        }

        return possibileLines;
    }

    private void FillField(int[][] fields, int cordX, int cordY, int fieldValue)
    {
        if (fields[cordX][cordY] == 0)
        {
            fields[cordX][cordY] = fieldValue;
            _fieldsSum += fieldValue;
            _restrictedNumbers[0, cordX].Add(fieldValue);
            _restrictedNumbers[1, cordY].Add(fieldValue);
        }
    }

    private bool MatchLineWithPossibleFieldsValues(int[] possibleLine, List<int>[] possibleFieldValuesList)
    {
        int length;
        if ((length = possibleLine.Length) == possibleFieldValuesList.Length)
        {
            for (int c = 0; c < length; c++)
            {
                if (!possibleFieldValuesList[c].Contains(possibleLine[c]))
                    return false;
            }
        }

        return true;
    }

    public T[] GetColumn<T>(T[][] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix[0].Length)
            .Select(x => matrix[x][columnNumber])
            .ToArray();
    }

    public T[] GetRow<T>(T[][] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.Length)
            .Select(x => matrix[rowNumber][x])
            .ToArray();
    }

    public T[] GetColumn<T>(T[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
            .Select(x => matrix[x, columnNumber])
            .ToArray();
    }

    public T[] GetRow<T>(T[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
            .Select(x => matrix[rowNumber, x])
            .ToArray();
    }

    private bool MatchPermuteToLine(int[] line, int[] permutation)
    {
        bool result = true;
        if (line.Length == permutation.Length)
        {
            for (int c = 0; c < line.Length; c++)
            {
                if (line[c] > 0)
                {
                    if (line[c] != permutation[c])
                        result = false;
                }
            }
        }

        return result;
    }

    private void Permute(int[][] permutations, ref int index, IList<int> elements, int l, int r)
    {
        if (l == r)
        {
            permutations[index] = new int[Dimension];
            for (int c = 0; c < elements.Count; c++)
                permutations[index][c] = elements[c];
            index++;
        }

        for (int i = l; i <= r; i++)
        {
            Swap<int>(elements, l, i);
            Permute(permutations, ref index, elements, l + 1, r);
            Swap<int>(elements, l, i);
        }
    }

    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    public static int
        NumberOfVisibleSkyscrapers(bool pointOfView,
            int[] skyscrapers) // pointOfView determines from which direction to check(false - normal, true - reversed)
    {
        int visibleSkyscrapers = 0;
        int c = (!pointOfView ? 0 : Dimension - 1);
        int height = 0;
        for (int i = 0; i < Dimension; i++)
        {
            if (skyscrapers[c] >= height)
                visibleSkyscrapers++;
            height = (height > skyscrapers[c] ? height : skyscrapers[c]);
            if (skyscrapers[c] >= Dimension)
                break;
            c = (!pointOfView ? c + 1 : c - 1);
        }

        return visibleSkyscrapers;
    }

    public static int Strong(int input) // returns Strong of integer larger than 0
    {
        if (input <= 0)
            return 0;
        int result = 1;
        for (int c = 1; c <= input; c++)
        {
            result *= c;
        }

        return result;
    }
}