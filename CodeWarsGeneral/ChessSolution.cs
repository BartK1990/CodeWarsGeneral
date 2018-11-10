using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarsGeneral
{
    public enum FigureType { Pawn, King, Queen, Rook, Knight, Bishop }

    //struct to make it convenient to work with cells
    public struct Pos
    {
        public readonly sbyte X;
        public readonly sbyte Y;

        public Pos(sbyte y, sbyte x)
        {
            X = x;
            Y = y;
        }
        public Pos(int y, int x)
        {
            X = (sbyte)x;
            Y = (sbyte)y;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Pos))
                return false;

            Pos pos = (Pos)obj;
            if ((this.X == pos.X) && (this.Y == pos.Y))
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return X ^ Y;
        }
    }

    public class Figure
    {
        public FigureType Type { get; }
        public byte Owner { get; }
        public Pos Cell { get; set; }
        public Pos? PrevCell { get; }

        public Figure(FigureType type, byte owner, Pos cell, Pos? prevCell = null)
        {
            Type = type;
            Owner = owner;
            Cell = cell;
            PrevCell = prevCell;
        }
    }

    public class ChessSolution
    {

        public const sbyte boardBoundary = 7;

        public static List<Pos> attackPos;

        // Returns an array of threats if the arrangement of 
        // the pieces is a check, otherwise null
        // Checks check for King of color which is defined by player argument
        public static List<Figure> isCheck(IList<Figure> pieces, int player)
        {
            List<Pos> attackPos = new List<Pos>();
            return null;
        }

        // Returns true if the arrangement of the
        // pieces is a check mate, otherwise false
        public static bool isMate(IList<Figure> pieces, int player)
        {
            return false;
        }

        public static List<Figure> AttackPos(IList<Figure> pieces, int player)
        {
            attackPos.Clear();
            List<Figure> checkFigures = new List<Figure>();
            List<Pos> attackPosTemp = new List<Pos>();
            Pos kingPos = new Pos(-1, -1);

            foreach(Figure piece in pieces)
            {
                if ((piece.Type == FigureType.King) && (piece.Owner == player))
                    kingPos = piece.Cell;
            }
            
            if (kingPos.X == (-1))
                return null;
                
            for (sbyte i=0; i < pieces.Count; i++)
            {
                if (pieces[i].Owner != (byte)player)
                    continue;
                if(pieces[i].Type == FigureType.Pawn)
                {
                    attackPosTemp = PawnAttackPos(pieces, i, player);
                }else if (pieces[i].Type == FigureType.Knight)
                {
                    attackPosTemp = KnightAttackPos(pieces, i);
                }
                else if (pieces[i].Type == FigureType.King)
                {
                    attackPosTemp = KingAttackPos(pieces, i);
                }
                else if (pieces[i].Type == FigureType.Bishop)
                {
                    attackPosTemp = BishopAttackPos(pieces, i);
                }
                else if (pieces[i].Type == FigureType.Queen)
                {
                    attackPosTemp = QueenAttackPos(pieces, i);
                }
                else if (pieces[i].Type == FigureType.Rook)
                {
                    attackPosTemp = RookAttackPos(pieces, i);
                }
                foreach(Pos p in attackPosTemp)
                {
                    if (!attackPos.Contains(p))
                        attackPos.Add(p);
                }
            }
            return checkFigures;
        }

        public static List<Pos> PawnAttackPos(IList<Figure> pieces, sbyte piecePos, int player, ref List<Figure> checkPieces)
        {

        }

        public static List<Pos> KnightAttackPos(IList<Figure> pieces, sbyte piecePos)
        {

        }

        public static List<Pos> KingAttackPos(IList<Figure> pieces, sbyte piecePos)
        {

        }

        public static List<Pos> QueenAttackPos(IList<Figure> pieces, sbyte piecePos)
        {

        }

        public static List<Pos> BishopAttackPos(IList<Figure> pieces, sbyte piecePos)
        {

        }

        public static List<Pos> RookAttackPos(IList<Figure> pieces, sbyte piecePos)
        {

        }
    }
}
