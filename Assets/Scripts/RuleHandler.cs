﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RuleHandler
{
    public enum MoveType
    {
        NORMAL,
        CASTLING,
        PROMO
    };
    public struct MoveList
    {
        public Vector2Int piecePlace;
        public List<Vector2Int> movePlace;
        public List<string> pieceAfterMove;
    }

    public static bool isWhitePiece(Vector2Int point, string[,] boardCell)
    {
        int piecePlaceX = System.Convert.ToInt32(point.x);
        int piecePlaceY = System.Convert.ToInt32(point.y);
        return boardCell[piecePlaceX, piecePlaceY].Equals(boardCell[piecePlaceX, piecePlaceY].ToLower());
    }

    public static Board MovePiece(Vector2Int piecePlace, Vector2Int movePlace, string newPiece, Board board)
    {
        Board newBoard = new Board(board);
        string piece = newBoard.BoardCells[piecePlace.x, piecePlace.y];
        if ((piece == "K") || (piece == "k"))
        {
            if ((movePlace.x - piecePlace.x) == 2)
            {
                newBoard.BoardCells[7, piecePlace.y] = "0";
                if (piece == "K")
                    newBoard.BoardCells[5, piecePlace.y] = "R";
                else
                    newBoard.BoardCells[5, piecePlace.y] = "r";
            }
            else if ((movePlace.x - piecePlace.x) == -2)
            {
                newBoard.BoardCells[0, piecePlace.y] = "0";
                if (piece == "K")
                    newBoard.BoardCells[3, piecePlace.y] = "R";
                else
                    newBoard.BoardCells[3, piecePlace.y] = "r";
            }

            if (piece == "k")
            {
                newBoard.WhiteCanNCastling = false;
                newBoard.WhiteCanFCastling = false;
            }
            else if (piece == "K")
            {
                newBoard.BlackCanNCastling = false;
                newBoard.BlackCanFCastling = false;
            }
        }

        if (piece == "r")
        {
            if ((piecePlace.x == 0) && (newBoard.WhiteCanFCastling == true))
                newBoard.WhiteCanFCastling = false;
            if ((piecePlace.x == 7) && (newBoard.WhiteCanNCastling == true))
                newBoard.WhiteCanNCastling = false;
        }
        if (piece == "R")
        {
            if ((piecePlace.x == 0) && (newBoard.BlackCanFCastling == true))
                newBoard.BlackCanFCastling = false;
            if ((piecePlace.x == 7) && (newBoard.BlackCanNCastling == true))
                newBoard.BlackCanNCastling = false;
        }

        newBoard.BoardCells[piecePlace.x, piecePlace.y] = "0";
        if (newPiece == "0")
            newBoard.BoardCells[movePlace.x, movePlace.y] = piece;
        else
            newBoard.BoardCells[movePlace.x, movePlace.y] = newPiece;
        newBoard.Evaluation = GameEvaluation.caculateBoardEvaluation(newBoard.BoardCells);

        return newBoard;
    }

    public static MoveList FindPieceMove(Vector2Int point, Board board, bool isWhite)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();

        if (isWhite)
        {
            switch (board.BoardCells[point.x, point.y])
                {
                    case "k":       moveList = FindKingMove(point, board); break;
                    case "q":       moveList = FindQueenMove(point, board); break;
                    case "b":       moveList = FindBishopMove(point, board); break;
                    case "kn":      moveList = FindKnightMove(point, board); break;
                    case "r":       moveList = FindRookMove(point, board); break;
                    case "p":       moveList = FindPawnMove(point, board); break;
                }
        }
        else
        {
            switch (board.BoardCells[point.x, point.y])
                {
                    case "K":       moveList = FindKingMove(point, board); break;
                    case "Q":       moveList = FindQueenMove(point, board); break;
                    case "B":       moveList = FindBishopMove(point, board); break;
                    case "KN":      moveList = FindKnightMove(point, board); break;
                    case "R":       moveList = FindRookMove(point, board); break;
                    case "P":       moveList = FindPawnMove(point, board); break;
                }
        }

        return moveList;
    }

    public static MoveList FindKingMove(Vector2Int point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();
        moveList.piecePlace = point;
        bool isWhiteKing = isWhitePiece(point, board.BoardCells);

        if (IsEnableToMove(point, point + new Vector2Int(1, 0), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(1, 0), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(1, 0));

        if (IsEnableToMove(point, point + new Vector2Int(-1, 0), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(-1, 0), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(-1, 0));

        if (IsEnableToMove(point, point + new Vector2Int(0, 1), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(0, 1), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(0, 1));

        if (IsEnableToMove(point, point + new Vector2Int(0, -1), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(0, -1), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(0, -1));

        if (IsEnableToMove(point, point + new Vector2Int(1, 1), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(1, 1), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(1, 1));

        if (IsEnableToMove(point, point + new Vector2Int(-1, 1), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(-1, 1), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(-1, 1));

        if (IsEnableToMove(point, point + new Vector2Int(-1, -1), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(-1, -1), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(-1, -1));

        if (IsEnableToMove(point, point + new Vector2Int(1, -1), "0", true, board) > 0)
            if (!IsCheckedCell(point + new Vector2Int(1, -1), board, isWhiteKing))
                moveList.movePlace.Add(point + new Vector2Int(1, -1));

        if (!IsCheckedCell(point, board, true))
        {
            if (isWhiteKing)
            {
                if (board.WhiteCanNCastling)
                {
                    bool castlingMove = true;
                    for (int i = 1; i <= 2; i++)
                    {
                        if (IsEnableToMove(point, point + new Vector2Int(i, 0), "0", false, board) <= 0)
                        {
                            castlingMove = false;
                            break;
                        }
                    }
                    if (castlingMove)
                    {
                        moveList.movePlace.Add(point + new Vector2Int(2, 0));
                    }
                }

                if (board.WhiteCanFCastling)
                {
                    bool castlingMove = true;
                    for (int i = 1; i <= 2; i++)
                    {
                        if (IsEnableToMove(point, point - new Vector2Int(i, 0), "0", false, board) <= 0)
                        {
                            castlingMove = false;
                            break;
                        }
                    }
                    if (board.BoardCells[point.x - 3, point.y] != "0")
                        castlingMove = false;
                    if (castlingMove)
                    {
                        moveList.movePlace.Add(point - new Vector2Int(2, 0));
                    }
                }
            }
            else
            {
                if (board.BlackCanNCastling)
                {
                    bool castlingMove = true;
                    for (int i = 1; i <= 2; i++)
                    {
                        if (IsEnableToMove(point, point + new Vector2Int(i, 0), "0", false, board) <= 0)
                        {
                            castlingMove = false;
                            break;
                        }
                    }
                    if (castlingMove)
                    {
                        moveList.movePlace.Add(point + new Vector2Int(2, 0));
                    }
                }
                if (board.BlackCanFCastling)
                {
                    bool castlingMove = true;
                    for (int i = 1; i <= 2; i++)
                    {
                        if (IsEnableToMove(point, point - new Vector2Int(i, 0), "0", false, board) <= 0)
                        {
                            castlingMove = false;
                            break;
                        }
                    }
                    if (board.BoardCells[point.x - 3, point.y] != "0")
                        castlingMove = false;
                    if (castlingMove)
                    {
                        moveList.movePlace.Add(point - new Vector2Int(2, 0));
                    }
                }
            }
        }

        return moveList;
    }

    public static bool IsCheckedCell(Vector2Int point, Board board, bool IsWhiteKing)
    {
        if (IsWhiteKing)
        {
            //Horizontal & Vertical Lines
            int hDir = 1;
            for (int space = 1; space <= (7 - point.x); space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y] == "R")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = -1;
            for (int space = 1; space <= point.x; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y] == "R")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            int vDir = 1;
            for (int space = 1; space <= (7 - point.y); space++)
            {
                if (board.BoardCells[point.x, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x, point.y + vDir * space] == "R")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            vDir = -1;
            for (int space = 1; space <= point.y; space++)
            {
                if (board.BoardCells[point.x, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x, point.y + vDir * space] == "R")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            //Cross Lines
            hDir = 1;
            vDir = 1;
            int spaceLimit = Mathf.Min(7 - point.x, 7 - point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "B")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = -1;
            vDir = 1;
            spaceLimit = Mathf.Min(point.x, 7 - point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "B")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = 1;
            vDir = -1;
            spaceLimit = Mathf.Min(7 - point.x, point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "B")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = -1;
            vDir = -1;
            spaceLimit = Mathf.Min(point.x, point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "B")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "Q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "k")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            //L Shapes
            if (((point.x + 2) <= 7) && ((point.y + 1) <= 7) && (board.BoardCells[point.x + 2, point.y + 1] == "KN"))
                return true;
            if (((point.x + 1) <= 7) && ((point.y + 2) <= 7) && (board.BoardCells[point.x + 1, point.y + 2] == "KN"))
                return true;

            if (((point.x + 2) <= 7) && ((point.y - 1) >= 0) && (board.BoardCells[point.x + 2, point.y - 1] == "KN"))
                return true;
            if (((point.x + 1) <= 7) && ((point.y - 2) >= 0) && (board.BoardCells[point.x + 1, point.y - 2] == "KN"))
                return true;

            if (((point.x - 2) >= 0) && ((point.y + 1) <= 7) && (board.BoardCells[point.x - 2, point.y + 1] == "KN"))
                return true;
            if (((point.x - 1) >= 0) && ((point.y + 2) <= 7) && (board.BoardCells[point.x - 1, point.y + 2] == "KN"))
                return true;

            if (((point.x - 2) >= 0) && ((point.y - 1) >= 0) && (board.BoardCells[point.x - 2, point.y - 1] == "KN"))
                return true;
            if (((point.x - 1) >= 0) && ((point.y - 2) >= 0) && (board.BoardCells[point.x - 1, point.y - 2] == "KN"))
                return true;

            //Pawn check
            if (((point.x - 1) >= 0) && ((point.y + 1) <= 7) && board.BoardCells[point.x - 1, point.y + 1] == "P")
                return true;
            if (((point.x + 1) <= 7) && ((point.y + 1) <= 7) && board.BoardCells[point.x + 1, point.y + 1] == "P")
                return true;

        }
        else
        {
            //Horizontal & Vertical Lines
            int hDir = 1;
            for (int space = 1; space <= (7 - point.x); space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y] == "r")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = -1;
            for (int space = 1; space <= point.x; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y] == "r")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            int vDir = 1;
            for (int space = 1; space <= (7 - point.y); space++)
            {
                if (board.BoardCells[point.x, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x, point.y + vDir * space] == "r")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            vDir = -1;
            for (int space = 1; space <= point.y; space++)
            {
                if (board.BoardCells[point.x, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x, point.y + vDir * space] == "r")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x, point.y + vDir * space] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            //Cross Lines
            hDir = 1;
            vDir = 1;
            int spaceLimit = Mathf.Min(7 - point.x, 7 - point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "b")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = -1;
            vDir = 1;
            spaceLimit = Mathf.Min(point.x, 7 - point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "b")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = 1;
            vDir = -1;
            spaceLimit = Mathf.Min(7 - point.x, point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "b")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            hDir = -1;
            vDir = -1;
            spaceLimit = Mathf.Min(point.x, point.y);
            for (int space = 1; space <= spaceLimit; space++)
            {
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "0") continue;
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "b")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "q")
                {
                    return true;
                }
                else if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] == "K")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            //L Shapes
            if (((point.x + 2) <= 7) && ((point.y + 1) <= 7) && (board.BoardCells[point.x + 2, point.y + 1] == "kn"))
                return true;
            if (((point.x + 1) <= 7) && ((point.y + 2) <= 7) && (board.BoardCells[point.x + 1, point.y + 2] == "kn"))
                return true;

            if (((point.x + 2) <= 7) && ((point.y - 1) >= 0) && (board.BoardCells[point.x + 2, point.y - 1] == "kn"))
                return true;
            if (((point.x + 1) <= 7) && ((point.y - 2) >= 0) && (board.BoardCells[point.x + 1, point.y - 2] == "kn"))
                return true;

            if (((point.x - 2) >= 0) && ((point.y + 1) <= 7) && (board.BoardCells[point.x - 2, point.y + 1] == "kn"))
                return true;
            if (((point.x - 1) >= 0) && ((point.y + 2) <= 7) && (board.BoardCells[point.x - 1, point.y + 2] == "kn"))
                return true;

            if (((point.x - 2) >= 0) && ((point.y - 1) >= 0) && (board.BoardCells[point.x - 2, point.y - 1] == "kn"))
                return true;
            if (((point.x - 1) >= 0) && ((point.y - 2) >= 0) && (board.BoardCells[point.x - 1, point.y - 2] == "kn"))
                return true;

            //Pawn check
            if (((point.x - 1) >= 0) && ((point.y - 1) >= 0) && board.BoardCells[point.x - 1, point.y - 1] == "p")
                return true;
            if (((point.x + 1) <= 7) && ((point.y - 1) >= 0) && board.BoardCells[point.x + 1, point.y - 1] == "p")
                return true;
        }

        return false;
    }

    public static MoveList FindQueenMove(Vector2Int point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();
        moveList.piecePlace = point;

        MoveList tempMoveList = new MoveList();
        tempMoveList.movePlace = new List<Vector2Int>();
        tempMoveList = FindBishopMove(point, board);
        foreach(Vector2Int movePlace in tempMoveList.movePlace)
        {
            moveList.movePlace.Add(movePlace);
        }

        tempMoveList.movePlace.Clear();

        tempMoveList = FindRookMove(point, board);
        foreach (Vector2Int movePlace in tempMoveList.movePlace)
        {
            moveList.movePlace.Add(movePlace);
        }

        return moveList;
    }

    public static MoveList FindRookMove(Vector2Int point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();

        moveList.piecePlace = point;
        int hDir = 1;
        int space = 1;
        bool canMoveNext = true;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(hDir * space, 0), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(hDir * space, 0));
                if (board.BoardCells[point.x + hDir * space, point.y] != "0")
                    break;
            }    
            else
            {
                if (cellState == 0) break;
                
            }    

            space++;
        }

        hDir = -1;
        space = 1;
        
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(hDir * space, 0), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(hDir * space, 0));
                if (board.BoardCells[point.x + hDir * space, point.y] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }

        int vDir = 1;
        space = 1;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(0, vDir * space), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(0, vDir * space));
                if (board.BoardCells[point.x, point.y + vDir * space] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }

        vDir = -1;
        space = 1;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(0, vDir * space), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(0, vDir * space));
                if (board.BoardCells[point.x, point.y + vDir * space] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }
        return moveList;
    }

    public static MoveList FindBishopMove(Vector2Int point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();

        moveList.piecePlace = point;

        int hDir = 1;
        int vDir = 1;
        int space = 1;
        bool canMoveNext = true;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(hDir * space, vDir * space), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(hDir * space, vDir * space));
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }

        hDir = -1;
        vDir = 1;
        space = 1;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(hDir * space, vDir * space), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(hDir * space, vDir * space));
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }

        hDir = 1;
        vDir = -1;
        space = 1;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(hDir * space, vDir * space), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(hDir * space, vDir * space));
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }

        hDir = -1;
        vDir = -1;
        space = 1;
        while (canMoveNext)
        {
            int cellState = IsEnableToMove(point, point + new Vector2Int(hDir * space, vDir * space), "0", true, board);
            if (cellState > 0)
            {
                moveList.movePlace.Add(point + new Vector2Int(hDir * space, vDir * space));
                if (board.BoardCells[point.x + hDir * space, point.y + vDir * space] != "0")
                    break;
            }
            else
            {
                if (cellState == 0) break;
                
            }

            space++;
        }

        return moveList;
    }

    public static MoveList FindKnightMove(Vector2Int point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();

        moveList.piecePlace = point;

        if (IsEnableToMove(point, point + new Vector2Int(-1, -2), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(-1, -2));

        if (IsEnableToMove(point, point + new Vector2Int(+1, -2), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(+1, -2));

        if (IsEnableToMove(point, point + new Vector2Int(+1, +2), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(+1, +2));

        if (IsEnableToMove(point, point + new Vector2Int(-1, +2), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(-1, +2));

        if (IsEnableToMove(point, point + new Vector2Int(-2, -1), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(-2, -1));

        if (IsEnableToMove(point, point + new Vector2Int(+2, -1), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(+2, -1));

        if (IsEnableToMove(point, point + new Vector2Int(+2, +1), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(+2, +1));

        if (IsEnableToMove(point, point + new Vector2Int(-2, +1), "0", true, board) > 0)
            moveList.movePlace.Add(point + new Vector2Int(-2, +1));

        return moveList;
    }

    public static MoveList FindPawnMove(Vector2Int point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new List<Vector2Int>();
        moveList.pieceAfterMove = new List<string>();

        moveList.piecePlace = point;

        switch (board.BoardCells[point.x, point.y])
        {
            //White Side
            case "p":
                {
                    //2 Step Move
                    if (point.y == 1)
                    {
                        if (IsEnableToMove(point, new Vector2Int(point.x, 3), "0", false, board) > 0)
                        {
                            if (IsEnableToMove(point, new Vector2Int(point.x, 2), "0", false, board) > 0)
                                moveList.movePlace.Add(new Vector2Int(point.x, 3));
                        }
                    }

                    //Normal Moves
                    if (IsEnableToMove(point, point + new Vector2Int(0, 1), "0", false, board) > 0)
                    {
                        if (point.y < 6)
                            moveList.movePlace.Add(point + new Vector2Int(0, 1));
                        if (point.y == 6)
                        {
                            moveList.movePlace.Add(point + new Vector2Int(0, 1));
                            moveList.pieceAfterMove.Add("q");
                            moveList.movePlace.Add(point + new Vector2Int(0, 1));
                            moveList.pieceAfterMove.Add("r");
                            moveList.movePlace.Add(point + new Vector2Int(0, 1));
                            moveList.pieceAfterMove.Add("b");
                            moveList.movePlace.Add(point + new Vector2Int(0, 1));
                            moveList.pieceAfterMove.Add("kn");
                        }
                    }

                    //Capture
                    if (IsEnableToMove(point, point + new Vector2Int(-1, 1), "0", true, board) > 0)
                    {
                        if (board.BoardCells[point.x - 1, point.y + 1] != "0")
                        {
                            if (point.y < 6)
                                moveList.movePlace.Add(point + new Vector2Int(-1, 1));
                            if (point.y == 6)
                            {
                                moveList.movePlace.Add(point + new Vector2Int(-1, 1));
                                moveList.pieceAfterMove.Add("q");
                                moveList.movePlace.Add(point + new Vector2Int(-1, 1));
                                moveList.pieceAfterMove.Add("r");
                                moveList.movePlace.Add(point + new Vector2Int(-1, 1));
                                moveList.pieceAfterMove.Add("b");
                                moveList.movePlace.Add(point + new Vector2Int(-1, 1));
                                moveList.pieceAfterMove.Add("kn");
                            }
                        }
                    }

                    //Promo
                    if (IsEnableToMove(point, point + new Vector2Int(1, 1), "0", true, board) > 0)
                    {
                        if (board.BoardCells[point.x + 1, point.y + 1] != "0")
                        {
                            if (point.y < 6)
                                moveList.movePlace.Add(point + new Vector2Int(1, 1));
                            if (point.y == 6)
                            {
                                moveList.movePlace.Add(point + new Vector2Int(1, 1));
                                moveList.pieceAfterMove.Add("q");
                                moveList.movePlace.Add(point + new Vector2Int(1, 1));
                                moveList.pieceAfterMove.Add("r");
                                moveList.movePlace.Add(point + new Vector2Int(1, 1));
                                moveList.pieceAfterMove.Add("b");
                                moveList.movePlace.Add(point + new Vector2Int(1, 1));
                                moveList.pieceAfterMove.Add("kn");
                            }
                        }
                    }
                }   break;

            //Black Side
            case "P":
                {
                    // 2 Step Move
                    if (point.y == 6)
                    {
                        if (IsEnableToMove(point, new Vector2Int(point.x, 4), "0", false, board) > 0)
                        {
                            if (IsEnableToMove(point, new Vector2Int(point.x, 5), "0", false, board) > 0)
                                moveList.movePlace.Add(new Vector2Int(point.x, 4));
                        }
                    }

                    if (IsEnableToMove(point, point + new Vector2Int(0, -1), "0", false, board) > 0)
                    {
                        if (point.y > 1)
                            moveList.movePlace.Add(point + new Vector2Int(0, -1));
                        if (point.y == 1)
                        {
                            moveList.movePlace.Add(point + new Vector2Int(0, -1));
                            moveList.pieceAfterMove.Add("Q");
                            moveList.movePlace.Add(point + new Vector2Int(0, -1));
                            moveList.pieceAfterMove.Add("R");
                            moveList.movePlace.Add(point + new Vector2Int(0, -1));
                            moveList.pieceAfterMove.Add("B");
                            moveList.movePlace.Add(point + new Vector2Int(0, -1));
                            moveList.pieceAfterMove.Add("KN");
                        }
                    }

                    //Capture
                    if (IsEnableToMove(point, point + new Vector2Int(-1, -1), "0", true, board) > 0)
                    {
                        if (board.BoardCells[point.x - 1, point.y - 1] != "0")
                        {
                            if (point.y > 1)
                                moveList.movePlace.Add(point + new Vector2Int(-1, -1));
                            if (point.y == 1)
                            {
                                moveList.movePlace.Add(point + new Vector2Int(-1, -1));
                                moveList.pieceAfterMove.Add("Q");
                                moveList.movePlace.Add(point + new Vector2Int(-1, -1));
                                moveList.pieceAfterMove.Add("R");
                                moveList.movePlace.Add(point + new Vector2Int(-1, -1));
                                moveList.pieceAfterMove.Add("B");
                                moveList.movePlace.Add(point + new Vector2Int(-1, -1));
                                moveList.pieceAfterMove.Add("KN");
                            }
                        }
                    }

                    //Normal Moves
                    if (IsEnableToMove(point, point + new Vector2Int(1, 1), "0", true, board) > 0)
                    {
                        if (board.BoardCells[point.x + 1, point.y - 1] != "0")
                        {
                            if (point.y > 1)
                                moveList.movePlace.Add(point + new Vector2Int(1, -1));
                            if (point.y == 1)
                            {
                                moveList.movePlace.Add(point + new Vector2Int(1, -1));
                                moveList.pieceAfterMove.Add("Q");
                                moveList.movePlace.Add(point + new Vector2Int(1, -1));
                                moveList.pieceAfterMove.Add("R");
                                moveList.movePlace.Add(point + new Vector2Int(1, -1));
                                moveList.pieceAfterMove.Add("B");
                                moveList.movePlace.Add(point + new Vector2Int(1, -1));
                                moveList.pieceAfterMove.Add("KN");
                            }
                        }
                    }
                }   break;
        }

        return moveList;
    }

    public static int IsEnableToMove(Vector2Int piecePlace, Vector2Int newPlace, string newPiece, bool isCaptureMove, Board board)
    {
        int canMove = 0;
        bool whitePiece = isWhitePiece(piecePlace, board.BoardCells);
        if ((newPlace.x >= 0) && (newPlace.x <= 7) && (newPlace.y >= 0) && (newPlace.y <= 7))
        {
            if ((board.BoardCells[newPlace.x, newPlace.y] == "0"))
                canMove = 1;
            else
            {
                if (isCaptureMove)
                {
                    bool otherWhite = isWhitePiece(new Vector2Int(newPlace.x, newPlace.y), board.BoardCells);
                    if (whitePiece != otherWhite)
                        canMove = 2;
                }
            }
        }

        //Check if the move is not a self-destruction _(:3JZ)_
        if (canMove > 0)
        {
            Board newBoard = new Board(MovePiece(piecePlace, newPlace, newPiece, board));
            if (whitePiece)
            {
                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        if (newBoard.BoardCells[i, j] == "k")
                        {
                            if (IsCheckedCell(new Vector2Int(i, j), newBoard, whitePiece))
                            {
                                canMove = -1;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        if (newBoard.BoardCells[i, j] == "K")
                        {
                            if (IsCheckedCell(new Vector2Int(i, j), newBoard, whitePiece))
                            {
                                canMove = -1;
                            }
                        }
                    }
                }
            }
        }    

        return canMove;
    }
}
