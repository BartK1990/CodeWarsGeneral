using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{
    public class LcsClass
    {
        public static string Lcs(string a, string b)
        {
            // create C table and clear it
            int[,] C = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++)
                C[i, 0] = 0;
            for (int j = 0; j <= b.Length; j++)
                C[0, j] = 0;

            // calculate C table
            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    if (a[i - 1] == b[j - 1])
                        C[i, j] = C[i - 1, j - 1] + 1;
                    else
                        C[i, j] = Math.Max(C[i - 1, j], C[i, j - 1]);
                }
            }

            string lcs = backtrackLcs(C, a, b, a.Length, b.Length);
            return lcs;
        }

        public static string backtrackLcs(int[,] C, string a, string b, int i, int j)
        {
            if (i == 0 || j == 0)
                return "";
            if (a[i - 1] == b[j - 1])
                return backtrackLcs(C, a, b, i - 1, j - 1) + a[i - 1];
            if (C[i, j - 1] > C[i - 1, j])
                return backtrackLcs(C, a, b, i, j - 1);
            return backtrackLcs(C, a, b, i - 1, j);
        }
    }
}
