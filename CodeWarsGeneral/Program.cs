using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeWarsGeneral
{
    public class Program
    {
        static void Main(string[] args)
        {
            double result;

            result = Evaluator.Evaluate("12* 123/-(-5 + 2)");

            Console.ReadKey();
        }
    }
}
