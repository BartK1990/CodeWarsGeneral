using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Figure> listFig;
            // listFig = Solution.isCheck(pawnThreatensKing, 0);
            bool test = Solution.isMate(ChessTest2, 0);

            Console.ReadKey();
        }

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
        public static readonly Figure[] ChessTest = new[]
        {
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.Queen, 1, new Pos(4, 7), new Pos(0,3)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 3)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 4)),
            new Figure(FigureType.Pawn, 0, new Pos(5, 5)),
            new Figure(FigureType.Pawn, 0, new Pos(4, 6)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 7)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Bishop, 0, new Pos(7, 5)),
            new Figure(FigureType.Knight, 0, new Pos(7, 6)),
            new Figure(FigureType.Rook, 0, new Pos(7, 7)),
            new Figure(FigureType.Queen, 0, new Pos(7, 3)),
        };
        public static readonly Figure[] ChessTest2 = new[]
{
            new Figure(FigureType.King, 1, new Pos(0, 4)),
            new Figure(FigureType.Queen, 1, new Pos(7, 3), new Pos(6,2)),
            new Figure(FigureType.Rook, 1, new Pos(6, 3)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 4)),
            new Figure(FigureType.Pawn, 0, new Pos(6, 5)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Rook, 0, new Pos(7, 5)),
        };
    }
}
