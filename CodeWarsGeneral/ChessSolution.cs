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

    public class Solution
    {

        public const sbyte boardMaxBoundary = 7;
        public const sbyte boardMinBoundary = 0;

        public static List<Pos> attackPos;
        public static List<Pos> piecesPos;

        // Returns an array of threats if the arrangement of 
        // the pieces is a check, otherwise null
        // Checks check for King of color which is defined by player argument
        public static List<Figure> isCheck(IList<Figure> pieces, int player)
        {
            List<Figure> checkingPieces = AttackPos(pieces, player);
            return checkingPieces;
        }

        // Returns true if the arrangement of the
        // pieces is a check mate, otherwise false
        public static bool isMate(IList<Figure> pieces, int player)
        {
            List<Figure> checkingPieces = AttackPos(pieces, player);
            if (checkingPieces.Count < 1)
                return false;
            Figure myKing = new Figure(FigureType.King, 0, new Pos(-1, -1));
            foreach (Figure piece in pieces)
            {
                if ((piece.Type == FigureType.King) && (piece.Owner != player))
                    myKing = piece;
            }
            if (myKing.Cell.X == (-1))
                return false;

            sbyte xAdd = 0;
            sbyte yAdd = 0;
            for (int direction = 0; direction < 8; direction++)
            {
                switch (direction)
                {
                    case 0:
                        xAdd = 1;
                        yAdd = 1;
                        break;
                    case 1:
                        xAdd = 1;
                        yAdd = 0;
                        break;
                    case 2:
                        xAdd = 0;
                        yAdd = 1;
                        break;
                    case 3:
                        xAdd = -1;
                        yAdd = -1;
                        break;
                    case 4:
                        xAdd = 1;
                        yAdd = -1;
                        break;
                    case 5:
                        xAdd = -1;
                        yAdd = 1;
                        break;
                    case 6:
                        xAdd = 0;
                        yAdd = -1;
                        break;
                    case 7:
                        xAdd = -1;
                        yAdd = 0;
                        break;
                }
                sbyte x = myKing.Cell.X, y = myKing.Cell.Y;
                x += xAdd;
                y += yAdd;
                if ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
                {
                    Pos posToCheck = new Pos(y, x);
                    if ((!attackPos.Contains(posToCheck)) && (!(piecesPos.Contains(posToCheck))))
                        return false;

                }
            }
            return true;
        }

        public static List<Figure> AttackPos(IList<Figure> pieces, int player)
        {
            attackPos = new List<Pos>();
            List<Figure> checkPieces = new List<Figure>();
            List<Pos> attackPosTemp = new List<Pos>();

            Pos kingPos = new Pos(-1, -1);
            foreach (Figure piece in pieces)
            {
                if ((piece.Type == FigureType.King) && (piece.Owner == player))
                    kingPos = piece.Cell;
            }
            if (kingPos.X == (-1))
                return null;

            piecesPos = new List<Pos>();
            foreach (Figure piece in pieces)
            {
                piecesPos.Add(piece.Cell);
            }

            foreach(Figure piece in pieces)
            {
                if (piece.Owner == (byte)player)
                    continue;
                if (piece.Type == FigureType.Pawn)
                {
                    attackPosTemp = PawnAttackPos(piece, player, ref checkPieces, kingPos);
                } else if (piece.Type == FigureType.Knight)
                {
                    attackPosTemp = KnightAttackPos(piece, ref checkPieces, kingPos);
                }
                else if (piece.Type == FigureType.King)
                {
                    attackPosTemp = KingAttackPos(piece, ref checkPieces, kingPos);
                }
                else if (piece.Type == FigureType.Bishop)
                {
                    attackPosTemp = BishopAttackPos(piece, ref checkPieces, kingPos);
                }
                else if (piece.Type == FigureType.Queen)
                {
                    attackPosTemp = QueenAttackPos(piece, ref checkPieces, kingPos);
                }
                else if (piece.Type == FigureType.Rook)
                {
                    attackPosTemp = RookAttackPos(piece, ref checkPieces, kingPos);
                }
                foreach (Pos p in attackPosTemp)
                {
                    if (!attackPos.Contains(p))
                        attackPos.Add(p);
                }
            }
            return checkPieces;
        }

        public static List<Pos> PawnAttackPos(Figure piece,int player, ref List<Figure> checkPieces, Pos kingPos)
        {
            sbyte playerCorrection;
            playerCorrection = ((player == 0) ? (sbyte)1 : (sbyte)-1);
            sbyte xAdd = 0;
            sbyte yAdd = 0;
            List<Pos> attackPosPiece = new List<Pos>();
            for (int direction = 0; direction < 2; direction++)
            {
                switch (direction)
                {
                    case 0:
                        xAdd = 1;
                        yAdd = 1;
                        break;
                    case 1:
                        xAdd = -1;
                        yAdd = 1;
                        break;
                }
                yAdd *= playerCorrection;
                sbyte x = piece.Cell.X, y = piece.Cell.Y;
                x += xAdd;
                y += yAdd;
                if ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
                {
                    Pos posToCheck = new Pos(y, x);
                    if (posToCheck.Equals(kingPos))
                        checkPieces.Add(piece);
                    attackPosPiece.Add(posToCheck);
                }
            }
            return attackPosPiece;
        }

        public static List<Pos> KnightAttackPos(Figure piece, ref List<Figure> checkPieces, Pos kingPos)
        {
            sbyte xAdd = 0;
            sbyte yAdd = 0;
            List<Pos> attackPosPiece = new List<Pos>();
            for (int direction = 0; direction < 8; direction++)
            {
                switch (direction)
                {
                    case 0:
                        xAdd = 2;
                        yAdd = 1;
                        break;
                    case 1:
                        xAdd = 1;
                        yAdd = 2;
                        break;
                    case 2:
                        xAdd = -2;
                        yAdd = 1;
                        break;
                    case 3:
                        xAdd = 1;
                        yAdd = -2;
                        break;
                    case 4:
                        xAdd = 2;
                        yAdd = -1;
                        break;
                    case 5:
                        xAdd = -1;
                        yAdd = 2;
                        break;
                    case 6:
                        xAdd = -2;
                        yAdd = -1;
                        break;
                    case 7:
                        xAdd = -1;
                        yAdd = -2;
                        break;
                }
                sbyte x = piece.Cell.X, y = piece.Cell.Y;
                x += xAdd;
                y += yAdd;
                if ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
                {
                    Pos posToCheck = new Pos(y, x);
                    if (posToCheck.Equals(kingPos))
                        checkPieces.Add(piece);
                    attackPosPiece.Add(posToCheck);
                }
            }
            return attackPosPiece;
        }

        public static List<Pos> KingAttackPos(Figure piece, ref List<Figure> checkPieces, Pos kingPos)
        {
            sbyte xAdd = 0;
            sbyte yAdd = 0;
            List<Pos> attackPosPiece = new List<Pos>();
            for (int direction = 0; direction < 8; direction++)
            {
                switch (direction)
                {
                    case 0:
                        xAdd = 1;
                        yAdd = 1;
                        break;
                    case 1:
                        xAdd = 1;
                        yAdd = 0;
                        break;
                    case 2:
                        xAdd = 0;
                        yAdd = 1;
                        break;
                    case 3:
                        xAdd = -1;
                        yAdd = -1;
                        break;
                    case 4:
                        xAdd = 1;
                        yAdd = -1;
                        break;
                    case 5:
                        xAdd = -1;
                        yAdd = 1;
                        break;
                    case 6:
                        xAdd = 0;
                        yAdd = -1;
                        break;
                    case 7:
                        xAdd = -1;
                        yAdd = 0;
                        break;
                }
                sbyte x = piece.Cell.X, y = piece.Cell.Y;
                x += xAdd;
                y += yAdd;
                if ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
                {
                    Pos posToCheck = new Pos(y, x);
                    if (posToCheck.Equals(kingPos))
                        checkPieces.Add(piece);
                    attackPosPiece.Add(posToCheck);
                }
            }
            return attackPosPiece;
        }

        public static List<Pos> QueenAttackPos(Figure piece, ref List<Figure> checkPieces, Pos kingPos)
        {
            List<Pos> attackPosPiece = new List<Pos>();
            attackPosPiece.AddRange(BishopAttackPos(piece, ref checkPieces, kingPos));
            attackPosPiece.AddRange(RookAttackPos(piece, ref checkPieces, kingPos));
            return attackPosPiece;
        }

        public static List<Pos> BishopAttackPos(Figure piece, ref List<Figure> checkPieces, Pos kingPos)
        {
            sbyte xAdd = 0;
            sbyte yAdd = 0;
            List<Pos> attackPosPiece = new List<Pos>();
            for (int direction = 0; direction < 4; direction++)
            {
                switch (direction)
                {
                    case 0:
                        xAdd = -1;
                        yAdd = -1;
                        break;
                    case 1:
                        xAdd = 1;
                        yAdd = -1;
                        break;
                    case 2:
                        xAdd = -1;
                        yAdd = 1;
                        break;
                    case 3:
                        xAdd = 1;
                        yAdd = 1;
                        break;
                }
                RookBishopAttackPos(piece, ref checkPieces, kingPos, ref attackPosPiece, xAdd, yAdd);
            }
            return attackPosPiece;
        }

        public static List<Pos> RookAttackPos(Figure piece, ref List<Figure> checkPieces, Pos kingPos)
        {
            sbyte xAdd = 0;
            sbyte yAdd = 0;
            List<Pos> attackPosPiece = new List<Pos>();
            for (int direction = 0; direction < 4; direction++)
            {
                switch (direction)
                {
                    case 0:
                        xAdd = -1;
                        yAdd = 0;
                        break;
                    case 1:
                        xAdd = 0;
                        yAdd = -1;
                        break;
                    case 2:
                        xAdd = 1;
                        yAdd = 0;
                        break;
                    case 3:
                        xAdd = 0;
                        yAdd = 1;
                        break;
                }
                RookBishopAttackPos(piece,ref checkPieces, kingPos,ref attackPosPiece, xAdd, yAdd);
            }
            return attackPosPiece;
        }

        public static void RookBishopAttackPos(Figure piece, ref List<Figure> checkPieces, Pos kingPos, 
            ref List<Pos> attackPosPiece, sbyte xAdd, sbyte yAdd)
        {
            sbyte x = piece.Cell.X, y = piece.Cell.Y;
            x += xAdd;
            y += yAdd;
            while ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
            {
                Pos posToCheck = new Pos(y, x);
                if (posToCheck.Equals(kingPos))
                {
                    attackPosPiece.Add(posToCheck);
                    checkPieces.Add(piece);
                    break;
                }
                else if (piecesPos.Contains(posToCheck))
                {
                    attackPosPiece.Add(posToCheck);
                    break;
                }
                else
                    attackPosPiece.Add(posToCheck);
                x += xAdd;
                y += yAdd;
            }
        }
    }
}
