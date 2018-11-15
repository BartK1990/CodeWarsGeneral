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

        public static List<Pos>[] piecesPos;
        public static Pos kingPos, oppositeKingPos;

        // Returns an array of threats if the arrangement of 
        // the pieces is a check, otherwise null
        // Checks check for King of color which is defined by player argument
        public static List<Figure> isCheck(IList<Figure> pieces, int player)
        {
            oppositeKingPos = new Pos(-1, -1);
            kingPos = new Pos(-1, -1);
            foreach (Figure piece in pieces)
            {
                if ((piece.Type == FigureType.King) && (piece.Owner == player))
                    kingPos = piece.Cell;
                if ((piece.Type == FigureType.King) && (piece.Owner != player))
                    oppositeKingPos = piece.Cell;
            }
            if (kingPos.X == (-1))
                return null;

            piecesPos = new List<Pos>[2];
            piecesPos[0] = new List<Pos>();
            piecesPos[1] = new List<Pos>();
            foreach (Figure piece in pieces)
            {
                piecesPos[piece.Owner].Add(piece.Cell);
            }

            List<Figure> checkPieces = new List<Figure>();
            List<Pos> attackPosTemp = new List<Pos>();
            foreach (Figure piece in pieces)
            {
                if (piece.Owner == (byte)player)
                    continue;
                attackPosTemp = PossibleMoves(piece, player, pieces);
                if (attackPosTemp.Contains(kingPos))
                    checkPieces.Add(piece);
            }
            return checkPieces;
        }

        // Returns true if the arrangement of the
        // pieces is a check mate, otherwise false
        public static bool isMate(IList<Figure> pieces, int player)
        {
            List<Figure> checkingPieces = isCheck(pieces, player);
            if (checkingPieces.Count < 1)
                return false;

            List<Figure> piecesTemp = new List<Figure>();
            piecesTemp.AddRange(pieces);

            for(int i = 0; i < piecesTemp.Count; i++)
            {
                if(piecesTemp[i].Owner == player)
                {
                    foreach(Pos p in PossibleMoves(piecesTemp[i], player, pieces))
                    {
                        Figure moveToCheck = new Figure(piecesTemp[i].Type, piecesTemp[i].Owner, p, piecesTemp[i].Cell);
                        Figure tempCapturePiece = piecesTemp[i];
                        Figure tempMovedPiece = piecesTemp[i];
                        int j;
                        for (j = 0; j < piecesTemp.Count; j++)
                        {
                            if (piecesTemp[j].Cell.Equals(moveToCheck.Cell) && (!piecesTemp[j].Cell.Equals(oppositeKingPos)))
                            {
                                tempCapturePiece = piecesTemp[j];
                                piecesTemp.Remove(piecesTemp[j]);
                                break;
                            }   
                        }
                        piecesTemp.Remove(tempMovedPiece);
                        piecesTemp.Insert(i, moveToCheck);
                        if (isCheck(piecesTemp, player).Count < 1)
                            return false;
                        piecesTemp.Remove(moveToCheck);
                        piecesTemp.Insert(i, tempMovedPiece);
                        if (tempCapturePiece != tempMovedPiece)
                            piecesTemp.Insert(j, tempCapturePiece);
                    }
                }
            }
            return true;
        }

        public static List<Pos> PossibleMoves(Figure piece, int player, IList<Figure> pieces)
        {
            List<Pos> possibleMoves = new List<Pos>();
            List<sbyte>[] possibleMovesTemp = { new List<sbyte>(), new List<sbyte>() };
            if ((piece.Type == FigureType.Bishop))
            {
                for (int direction = 0; direction < 4; direction++)
                {
                    switch (direction)
                    {
                        case 0:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 1:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 2:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 3:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(1);
                            break;
                    }
                }
            }
            if ((piece.Type == FigureType.Rook))
            {
                for (int direction = 0; direction < 4; direction++)
                {
                    switch (direction)
                    {
                        case 0:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(0);
                            break;
                        case 1:
                            possibleMovesTemp[0].Add(0);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 2:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(0);
                            break;
                        case 3:
                            possibleMovesTemp[0].Add(0);
                            possibleMovesTemp[1].Add(1);
                            break;
                    }
                }
            }
            if (piece.Type == FigureType.King || (piece.Type == FigureType.Queen))
            {
                for (int direction = 0; direction < 8; direction++)
                {
                    switch (direction)
                    {
                        case 0:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 1:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(0);
                            break;
                        case 2:
                            possibleMovesTemp[0].Add(0);
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 3:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 4:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 5:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 6:
                            possibleMovesTemp[0].Add(0);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 7:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(0);
                            break;
                    }
                }
            }
            if (piece.Type == FigureType.Knight)
            {
                for (int direction = 0; direction < 8; direction++)
                {
                    switch (direction)
                    {
                        case 0:
                            possibleMovesTemp[0].Add(2);
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 1:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(2);
                            break;
                        case 2:
                            possibleMovesTemp[0].Add(-2);
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 3:
                            possibleMovesTemp[0].Add(1);
                            possibleMovesTemp[1].Add(-2);
                            break;
                        case 4:
                            possibleMovesTemp[0].Add(2);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 5:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(2);
                            break;
                        case 6:
                            possibleMovesTemp[0].Add(-2);
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 7:
                            possibleMovesTemp[0].Add(-1);
                            possibleMovesTemp[1].Add(-2);
                            break;
                    }
                }
            }
            if ((piece.Type == FigureType.Pawn))
            {
                sbyte playerCorrection = ((piece.Owner == 0) ? (sbyte)-1 : (sbyte)1);
                for (int direction = 0; direction < 3; direction++)
                {
                    switch (direction)
                    {
                        case 0:
                            possibleMovesTemp[0].Add((sbyte)(1 * playerCorrection));
                            possibleMovesTemp[1].Add(1);
                            break;
                        case 1:
                            possibleMovesTemp[0].Add((sbyte)(1 * playerCorrection));
                            possibleMovesTemp[1].Add(-1);
                            break;
                        case 2:
                            possibleMovesTemp[0].Add((sbyte)(1 * playerCorrection));
                            possibleMovesTemp[1].Add(0);
                            break;
                    }
                }
            }

            // Add all possible moves
            if ((piece.Type == FigureType.Bishop) || (piece.Type == FigureType.Rook) || (piece.Type == FigureType.Queen))
            {
                for(int i = 0; i < possibleMovesTemp[0].Count; i++)
                {
                    sbyte x = piece.Cell.X, y = piece.Cell.Y;
                    y += possibleMovesTemp[0][i];
                    x += possibleMovesTemp[1][i];
                    while ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
                    {
                        Pos posToCheck = new Pos(y, x);
                        if (piecesPos[piece.Owner].Contains(posToCheck))
                        {
                            break;
                        }
                        else if (piecesPos[(piece.Owner == 0) ? 1 : 0].Contains(posToCheck))
                        {
                            possibleMoves.Add(posToCheck);
                            break;
                        }
                        else
                            possibleMoves.Add(posToCheck);
                        y += possibleMovesTemp[0][i];
                        x += possibleMovesTemp[1][i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < possibleMovesTemp[0].Count; i++)
                {
                    sbyte x = piece.Cell.X, y = piece.Cell.Y;
                    y += possibleMovesTemp[0][i];
                    x += possibleMovesTemp[1][i];
                    if ((x >= boardMinBoundary) && (x <= boardMaxBoundary) && (y >= boardMinBoundary) && (y <= boardMaxBoundary))
                    {
                        Pos posToCheck = new Pos(y, x);
                        if(piece.Type != FigureType.Pawn)
                        {
                            if (piecesPos[(piece.Owner == 0) ? 1 : 0].Contains(posToCheck))
                                possibleMoves.Add(posToCheck);
                            else if (!piecesPos[piece.Owner].Contains(posToCheck))
                                possibleMoves.Add(posToCheck);
                        }
                        else
                        {
                            if(x == piece.Cell.X)
                            {
                                if((!piecesPos[0].Contains(posToCheck)) && (!piecesPos[1].Contains(posToCheck)))
                                    possibleMoves.Add(posToCheck);
                            }
                            else
                            {
                                if(piecesPos[(piece.Owner == 0) ? 1 : 0].Contains(posToCheck))
                                    possibleMoves.Add(posToCheck);
                                //en passant
                                if ((!piecesPos[0].Contains(posToCheck)) && (!piecesPos[1].Contains(posToCheck)))
                                {
                                    Pos enPassantToCheck = new Pos(piece.Cell.Y, x);
                                    if(piecesPos[(piece.Owner == 0) ? 1 : 0].Contains(enPassantToCheck))
                                    {
                                        foreach (Figure figure in pieces)
                                        {
                                            if(figure.Cell.Equals(enPassantToCheck) && (figure.Type == FigureType.Pawn))
                                            {
                                                if(figure.PrevCell != null)
                                                {
                                                    if((Math.Abs(figure.Cell.X - figure.PrevCell.Value.X)) == 2)
                                                    {
                                                        possibleMoves.Add(posToCheck);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return possibleMoves;
        }
    }
}
