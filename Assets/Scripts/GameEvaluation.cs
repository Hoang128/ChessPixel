using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvaluation
{
    public const int kingVal = 500;
    public const int queenVal = 10;
    public const int rookVal = 5;
    public const int knightVal = 3;
    public const int bishopVal = 3;
    public const int pawnVal = 1;

    public static int[,] cellVal = new int[8, 8]
    {
        { 28, 24, 25, 25, 25, 25, 24, 28},
        { 32, 27, 29, 29, 29, 29, 27, 32},
        { 34, 29, 33, 33, 33, 33, 29, 34},
        { 34, 29, 33, 35, 35, 33, 29, 34},
        { 34, 29, 33, 35, 35, 33, 29, 34},
        { 34, 29, 33, 33, 33, 33, 29, 34},
        { 32, 27, 29, 29, 29, 29, 27, 32},
        { 28, 24, 25, 25, 25, 25, 24, 28}
    };

public static long caculateBoardEvaluation(string[, ] boardCell)
    {
        long newEvaluation = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (boardCell[i, j] != "0")
                {
                    switch (boardCell[i, j])
                    {
                        case "k":   newEvaluation += kingVal; break;
                        case "q":   newEvaluation += queenVal; break;
                        case "b":   newEvaluation += bishopVal; break;
                        case "kn":  newEvaluation += knightVal; break;
                        case "r":   newEvaluation += rookVal; break;
                        case "p":   newEvaluation += pawnVal; break;

                        case "K":   newEvaluation -= kingVal; break;
                        case "Q":   newEvaluation -= queenVal; break;
                        case "B":   newEvaluation -= bishopVal; break;
                        case "KN":  newEvaluation -= knightVal; break;
                        case "R":   newEvaluation -= rookVal; break;
                        case "P":   newEvaluation -= pawnVal; break;
                    }

                    bool isWhite = RuleHandler.isWhitePiece(new Vector2Int(i, j), boardCell);
                    if (isWhite)
                        newEvaluation += GameEvaluation.cellVal[i, j];
                    else
                        newEvaluation -= GameEvaluation.cellVal[i, j];
                }
                else continue;
            }
        }

        return newEvaluation;
    }

    public static int[,] CellVal { get => cellVal; set => cellVal = value; }

    public static int KingVal => kingVal;

    public static int QueenVal => queenVal;

    public static int RookVal => rookVal;

    public static int KnightVal => knightVal;

    public static int BishopVal => bishopVal;

    public static int PawnVal => pawnVal;
}
