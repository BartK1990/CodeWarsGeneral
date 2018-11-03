using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{
    using System.Numerics;

    public class Fibonacci
    {
        public static BigInteger fib(int n)
        {
            if (n == 0) return BigInteger.Zero;
            int nPositive = n;
            if (n < 0) nPositive = 0 - n;
            BigInteger fib1 = 1, fib1_temp; // for k = 1
            BigInteger fib2 = 1, fib2_temp;
            BigInteger fib_temp;
            int k = 2;
            bool whileBreaker = true;
            bool direction = true;
            while(whileBreaker)
            {
                if (k >= nPositive)
                {
                    if ((nPositive - (k/2)) <= (k - nPositive))
                    {
                        direction = true;
                        break;
                    }
                    else
                    {
                        direction = false;
                        whileBreaker = false;
                    }
                }
                fib1_temp = fib1;
                fib2_temp = fib2;
                fib1 = fib1_temp * (2 * fib2_temp - fib1_temp);
                fib2 = (fib2_temp * fib2_temp) + (fib1_temp * fib1_temp);
                k *= 2;
            }
            k = (k / 2);
            if (k == nPositive)
                return ((nPositive % 2) == 0) ? 0-fib1 : fib1;
            k = direction ? k+1 : k;
            if (direction)
            {
                for (; k < nPositive; k++)
                {
                    fib_temp = fib2;
                    fib2 = fib1 + fib2;
                    fib1 = fib_temp;
                }
            }
            else
            {
                for (; k > nPositive; k--)
                {
                    fib_temp = fib1;
                    fib1 = fib2 - fib1;
                    fib2 = fib_temp;
                }
            }     
            if(((nPositive % 2) == 0) && (n<0)) // for negatives
                return direction ? 0-fib2 : 0-fib1;
            else
                return direction ? fib2 : fib1;
        }
    }
}
