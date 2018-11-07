using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{
    public class Sudoku
    {
        private int[][] sudokuArray;

        public Sudoku(int[][] sudokuData)
        {
            this.sudokuArray = sudokuData;
        }

        public bool IsValid()
        {
            int N = sudokuArray.Length;
            int sqrtN = (int)Math.Sqrt(N);
            int[] tempColumnArray = new int[N];
            int[] tempSquareArray = new int[N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (!sudokuArray[i].Contains(j + 1))
                        return false;
                    tempColumnArray[j] = sudokuArray[j][i];
                }
                for (int j = 0; j < N; j++)
                {
                    if (!tempColumnArray.Contains(j + 1))
                        return false;
                }
                int xStartPos = (i % sqrtN)*sqrtN;
                int yStartPos = (i / sqrtN)*sqrtN;
                int l = 0;
                for (int j= xStartPos; j < xStartPos + sqrtN; j++)
                {
                    for(int k = yStartPos; k < yStartPos + sqrtN; k++)
                    {
                        tempSquareArray[l] = sudokuArray[j][k];
                        l++;
                    }
                }
                if (!tempSquareArray.Contains(i + 1))
                    return false;
            }

            return true;
        }
    }
}
