using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Board
{
    long evaluation;
    string[,] boardCell = new string[8, 8];
    bool canNCastling = true;
    bool canFCastling = true;
    /*
     * Cell assign:
     * white side: Normal characters
     * black side: Upcase characters
     * chess piece signs:
     * --> King: k & K
     * --> Queen: q & Q
     * --> Rook: r & R
     * --> Knight: kn & KN
     * --> Bishop: b & B
     * --> Pawm: p & P
     */

    public Board()
    {
        evaluation = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCell[i, j] = "0";
            }
        }
    }

    public Board(string[, ] newBoardCell)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCell[i, j] = newBoardCell[i, j];
            }
        }

        evaluation = GameEvaluation.caculateBoardEvaluation(boardCell);
    }

    public Board(Board newBoard)
    {
        evaluation = newBoard.evaluation;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCell[i, j] = newBoard.boardCell[i, j];
            }
        }
    }

    public Board(int newEvaluation, string[,] newBoardCells)
    {
        evaluation = newEvaluation;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCell[i, j] = newBoardCells[i, j];
            }
        }
    }

    public void Log()
    {
        string DebugLine = "";
        for (int i = 0; i < 8; i++)
        {
            
            for (int j = 0; j < 8; j++)
            {
                DebugLine += boardCell[i, j];

                if (boardCell[i, j].Length == 2)
                {
                    DebugLine += " ";
                }
                else
                    DebugLine += "  ";
            }
            DebugLine += "\n";
        }
        DebugLine += "\n";
        DebugLine += evaluation;
        Debug.Log(DebugLine);
    }

    public long Evaluation { get => evaluation; set => evaluation = value; }
    public string[,] BoardCells { get => boardCell; set => boardCell = value; }
    public bool CanNCastling { get => canNCastling; set => canNCastling = value; }
    public bool CanFCastling { get => canFCastling; set => canFCastling = value; }
}
