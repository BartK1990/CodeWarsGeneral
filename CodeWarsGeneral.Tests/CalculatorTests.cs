using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral.Tests
{
    using CodeWarsGeneral;

    [TestFixture]
    public class CalculatorTests
    {
        public bool close(double a, double b)
        {
            if (Math.Abs(a - b) < 0.000000001) return true;
            return false;
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(true, close(Kata.calculate("1 + 2"), 3));
        }
        [Test]
        public void Test2()
        {
            Assert.AreEqual(true, close(Kata.calculate("2*2"), 4));
        }
        [Test]
        public void Test3()
        {
            Assert.AreEqual(true, close(Kata.calculate("(((2+2*2)*2)+2)*6"), 84));
        }
        [Test]
        public void Test4()
        {
            Assert.AreEqual(true, close(Kata.calculate("5-(((2+2*2)*2)+2)*6"), -79));
        }
        [Test]
        public void Test5()
        {
            Assert.AreEqual(true, close(Kata.calculate("(5-(((2+2*2)*2)+2)))*6"), -42));
        }


    }

}

