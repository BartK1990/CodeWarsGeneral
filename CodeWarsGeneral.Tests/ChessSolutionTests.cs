﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral.Tests
{
    using CodeWarsGeneral;

    [TestFixture]
    public class SolutionSampleTest
    {
        [Test]
        public void MyTest()
        {
            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.pawnThreatensKing, 0),
              new[] { SampleTestCases.pawnThreatensKing[2] },
              "Pawn threatens king");

            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.rookThreatensKing, 0),
              new[] { SampleTestCases.rookThreatensKing[2] },
              "Rook threatens king");

            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.knightThreatensKing, 0),
              new[] { SampleTestCases.knightThreatensKing[2] },
              "Knight threatens king");

            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.bishopThreatensKing, 0),
              new[] { SampleTestCases.bishopThreatensKing[2] },
              "Bishop threatens king");

            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.queenThreatensKing1, 0),
              new[] { SampleTestCases.queenThreatensKing1[2] },
              "Queen threatens king");

            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.queenThreatensKing2, 0),
              new[] { SampleTestCases.queenThreatensKing2[2] },
              "Queen threatens king");

            CollectionAssert.AreEquivalent(ChessSolution.isCheck(SampleTestCases.doubleThreat, 0),
              new[] { SampleTestCases.doubleThreat[5], SampleTestCases.doubleThreat[6] },
              "Double threat");
        }
    }

    public class SampleTestCases
    {
        public static readonly Figure[] pawnThreatensKing = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Pawn, 1, new Pos(6, 5)),
        };
        public static readonly Figure[] rookThreatensKing = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Rook, 1, new Pos(1, 4)),
        };
        public static readonly Figure[] knightThreatensKing = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Knight, 1, new Pos(6, 2)),
        };
        public static readonly Figure[] bishopThreatensKing = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Bishop, 1, new Pos(3, 0)),
        };
        public static readonly Figure[] queenThreatensKing1 = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Queen, 1, new Pos(1, 4)),
        };
        public static readonly Figure[] queenThreatensKing2 = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Queen, 1, new Pos(4, 7)),
        };
        public static readonly Figure[] doubleThreat = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 4)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 5)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Bishop, 0, new Pos(7, 5)),
            new Figure(FigureType.Bishop, 1, new Pos(4, 1)),
            new Figure(FigureType.Rook, 1, new Pos(7, 2), new Pos(5, 2)),
        };
    }
}
