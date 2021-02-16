using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RuleHandler
{
    public struct MoveList
    {
        public Vector2 piecePlace;
        public Queue<Vector2> movePlace;
    }

    public static bool isWhitePiece(Vector2 point, string[,] boardCell)
    {
        int piecePlaceX = System.Convert.ToInt32(point.x);
        int piecePlaceY = System.Convert.ToInt32(point.y);
        return boardCell[piecePlaceX, piecePlaceY].Equals(boardCell[piecePlaceX, piecePlaceY].ToLower());
    }

    public static Board movePiece(Vector2 piecePlace, Vector2 movePlace, Board board)
    {
        Board newBoard = new Board(board);
        int piecePlaceX = System.Convert.ToInt32(piecePlace.x);
        int piecePlaceY = System.Convert.ToInt32(piecePlace.y);
        int movePlaceX = System.Convert.ToInt32(movePlace.x);
        int movePlaceY = System.Convert.ToInt32(movePlace.y);
        string piece = newBoard.BoardCells[piecePlaceX, piecePlaceY];
        newBoard.BoardCells[piecePlaceX, piecePlaceY] = "0";
        newBoard.BoardCells[movePlaceX, movePlaceY] = piece;
        newBoard.Evaluation = GameEvaluation.caculateBoardEvaluation(newBoard.BoardCells);

        return newBoard;
    }

    public static MoveList FindMove(Vector2 point, Board board, bool isWhite)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new Queue<Vector2>();

        int pointX = System.Convert.ToInt32(point.x);
        int pointY = System.Convert.ToInt32(point.y);

        if (isWhite)
        {
            switch (board.BoardCells[pointY, pointX])
                {
                    case "k": break;
                    case "q": break;
                    case "b": break;
                    case "kn": break;
                    case "r": break;
                    case "p": moveList = FindPawnMove(point, board); break;
                }
        }
        else
        {
            switch (board.BoardCells[pointY, pointX])
                {
                    case "K": break;
                    case "Q": break;
                    case "B": break;
                    case "KN": break;
                    case "R": break;
                    case "P": moveList = FindPawnMove(point, board); break;
                }
        }

        return moveList;
    }

    public static MoveList FindPawnMove(Vector2 point, Board board)
    {
        MoveList moveList = new MoveList();
        moveList.movePlace = new Queue<Vector2>();

        moveList.piecePlace = point;
        int pointX = System.Convert.ToInt32(point.x);
        int pointY = System.Convert.ToInt32(point.y);

        switch (board.BoardCells[pointY, pointX])
        {
            //White Side
            case "p":
                {
                    //Move
                    if (pointY == 1)
                    {
                        if (board.BoardCells[3, pointX] == "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2 (pointX, 3));
                        }
                    }
                    if (pointY < 7)
                    {
                        if (board.BoardCells[pointY + 1, pointX] == "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX, pointY + 1));
                        }
                    }

                    //Capture
                    if (pointX > 0)
                    {
                        if (board.BoardCells[pointY + 1, pointX - 1] != "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX - 1, pointY + 1));
                        }
                    }

                    if (pointX < 7)
                    {
                        if (board.BoardCells[pointY + 1, pointX + 1] != "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX + 1, pointY + 1));
                        }
                    }
                }   break;

            //Black Side
            case "P":
                {
                    //Move
                    if (point.y == 6)
                    {
                        if (board.BoardCells[pointX, 4] == "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX, 4));
                        }
                    }
                    if (point.y > 0)
                    {
                        if (board.BoardCells[pointX, pointY - 1] == "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX, pointY - 1));
                        }
                    }

                    //Capture
                    if (point.x > 0)
                    {
                        if (board.BoardCells[pointX - 1, pointY - 1] != "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX - 1, pointY - 1));
                        }
                    }

                    if (point.x < 7)
                    {
                        if (board.BoardCells[pointX + 1, pointY - 1] != "0")
                        {
                            moveList.movePlace.Enqueue(new Vector2(pointX + 1, pointY - 1));
                        }
                    }
                }   break;
        }

        return moveList;
    }
}
