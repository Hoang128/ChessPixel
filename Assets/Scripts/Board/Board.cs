using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Board
{
    double evaluation;
    string[,] boardCell = new string[8, 8];
    bool blackCanNCastling = true;
    bool blackCanFCastling = true;
    bool whiteCanNCastling = true;
    bool whiteCanFCastling = true;
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

        blackCanNCastling = true;
        blackCanFCastling = true;
        whiteCanNCastling = true;
        whiteCanFCastling = true;
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

        blackCanNCastling = newBoard.blackCanNCastling;
        blackCanFCastling = newBoard.blackCanFCastling;
        whiteCanNCastling = newBoard.whiteCanNCastling;
        whiteCanFCastling = newBoard.whiteCanFCastling;

        if (boardCell[7, 0] != "r") whiteCanNCastling = false;
        if (boardCell[0, 0] != "r") whiteCanFCastling = false;
        if (boardCell[7, 7] != "R") blackCanNCastling = false;
        if (boardCell[0, 7] != "R") blackCanFCastling = false;
    }

    public void UpdateCells(string[,] newBoardCells)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCell[i, j] = newBoardCells[i, j];
            }
        }

        if (boardCell[7, 0] != "r") whiteCanNCastling = false;
        if (boardCell[0, 0] != "r") whiteCanFCastling = false;
        if (boardCell[7, 7] != "R") blackCanNCastling = false;
        if (boardCell[0, 7] != "R") blackCanFCastling = false;
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

    public double Evaluation { get => evaluation; set => evaluation = value; }
    public string[,] BoardCells { get => boardCell; set => boardCell = value; }
    public bool BlackCanNCastling { get => blackCanNCastling; set => blackCanNCastling = value; }
    public bool BlackCanFCastling { get => blackCanFCastling; set => blackCanFCastling = value; }
    public bool WhiteCanNCastling { get => whiteCanNCastling; set => whiteCanNCastling = value; }
    public bool WhiteCanFCastling { get => whiteCanFCastling; set => whiteCanFCastling = value; }
}
