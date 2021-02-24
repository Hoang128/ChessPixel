using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvaluation
{
    public const double kingVal = 1000;
    public const double queenVal = 100;
    public const double rookVal = 50;
    public const double knightVal = 30;
    public const double bishopVal = 30;
    public const double pawnVal = 10;

    public static double[,] cellVal = new double[8, 8]
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

    public static double[,] cellWhiteKingVal = new double[8, 8]
    {
        { 2, 2, -1, -2, -3, -3, -3, -3},
        { 3, 2, -2, -3, -4, -4, -4, -4},
        { 1, 0, -2, -3, -4, -4, -4, -4},
        { 0, 0, -2, -4, -5, -5, -5, -5},
        { 0, 0, -2, -4, -5, -5, -5, -5},
        { 1, 0, -2, -3, -4, -4, -4, -4},
        { 3, 2, -2, -3, -4, -4, -4, -4},
        { 2, 2, -1, -2, -3, -3, -3, -3}
    };

    public static double[,] cellWhiteQueenVal = new double[8, 8]
    {
        { -2  , -1  , -1  ,  0  , -0.5, -1  , -1, -2  },
        { -1  ,  0  ,  0.5,  0  ,  0  ,  0  ,  0, -1  },
        { -1  ,  0.5,  0.5,  0.5,  0.5,  0.5,  0, -1  },
        { -0.5,  0  ,  0.5,  0.5,  0.5,  0.5,  0, -0.5},
        { -0.5,  0  ,  0.5,  0.5,  0.5,  0.5,  0, -0.5},
        { -1  ,  0  ,  0.5,  0.5,  0.5,  0.5,  0, -1  },
        { -1  ,  0  ,  0  ,  0  ,  0  ,  0  ,  0, -1  },
        { -2  , -1  , -1  , -1  , -0.5, -1  , -1, -2  }
    };

    public static double[,] cellWhiteRookVal = new double[8, 8]
    {
        { 0  , -0.5, -0.5, -0.5, -0.5, -0.5, 0.5, 0},
        { 0  ,  0  ,  0  ,  0  ,  0  ,  0  , 1  , 0},
        { 0  ,  0  ,  0  ,  0  ,  0  ,  0  , 1  , 0},
        { 0.5,  0  ,  0  ,  0  ,  0  ,  0  , 1  , 0},
        { 0.5,  0  ,  0  ,  0  ,  0  ,  0  , 1  , 0},
        { 0  ,  0  ,  0  ,  0  ,  0  ,  0  , 1  , 0},
        { 0  ,  0  ,  0  ,  0  ,  0  ,  0  , 1  , 0},
        { 0  , -0.5, -0.5, -0.5, -0.5, -0.5, 0.5, 0}
    };

    public static double[,] cellWhiteBishopVal = new double[8, 8]
    {
        { -2, -1  , -1, -1, -1  , -1  , -1, -2},
        { -1,  0.5,  1,  0,  0.5,  0  ,  0, -1},
        { -1,  0  ,  1,  1,  0.5,  0.5,  0, -1},
        { -1,  0  ,  1,  1,  1  ,  1  ,  0, -1},
        { -1,  0  ,  1,  1,  1  ,  1  ,  0, -1},
        { -1,  0  ,  1,  1,  0.5,  0.5,  0, -1},
        { -1,  0.5,  1,  0,  0.5,  0  ,  0, -1},
        { -2, -1  , -1, -1, -1  , -1  , -1, -2}
    };

    public static double[,] cellWhiteKnightVal = new double[8, 8]
    {
        { -5, -4  , -3  , -3  , -3  , -3  , -4, -5},
        { -4, -2  ,  0.5,  0  ,  0.5,  0  , -2, -4},
        { -3,  0  ,  1  ,  1.5,  1.5,  1  ,  0, -3},
        { -3,  0.5,  1.5,  2  ,  2  ,  1.5,  0, -3},
        { -3,  0.5,  1.5,  2  ,  2  ,  1.5,  0, -3},
        { -3,  0  ,  1  ,  1.5,  1.5,  1  ,  0, -3},
        { -4, -2  ,  0.5,  0  ,  0.5,  0  , -2, -4},
        { -5, -4  , -3  , -3  , -3  , -3  , -4, -5}
    };

    public static double[,] cellWhitePawnVal = new double[8, 8]
    {
        { 0,  0.5,  0.5, 0, 0.5, 1, 5, 0},
        { 0,  1  , -0.5, 0, 0.5, 1, 5, 0},
        { 0,  1  , -1  , 0, 1  , 2, 5, 0},
        { 0, -2  ,  0  , 2, 2.5, 3, 5, 0},
        { 0, -2  ,  0  , 2, 2.5, 3, 5, 0},
        { 0,  1  , -1  , 0, 1  , 2, 5, 0},
        { 0,  1  , -0.5, 0, 0.5, 1, 5, 0},
        { 0,  0.5,  0.5, 0, 0.5, 1, 5, 0}
    };

    public static double caculateBoardEvaluation(string[, ] boardCell)
    {
        double newEvaluation = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (boardCell[i, j] != "0")
                {
                    switch (boardCell[i, j])
                    {
                        case "k":   newEvaluation += (kingVal + cellWhiteKingVal[i, j]); break;
                        case "q":   newEvaluation += (queenVal + cellWhiteQueenVal[i, j]); break;
                        case "b":   newEvaluation += (bishopVal + cellWhiteBishopVal[i, j]); break;
                        case "kn":  newEvaluation += (knightVal + cellWhiteKnightVal[i, j]); break;
                        case "r":   newEvaluation += (rookVal + cellWhiteRookVal[i, j]); break;
                        case "p":   newEvaluation += (pawnVal + cellWhitePawnVal[i, j]); break;

                        case "K":   newEvaluation -= (kingVal + cellWhiteKingVal[7 - i, 7 - j]); break;
                        case "Q":   newEvaluation -= (queenVal + cellWhiteQueenVal[7 - i, 7 - j]); break;
                        case "B":   newEvaluation -= (bishopVal + cellWhiteBishopVal[7 - i, 7 - j]); break;
                        case "KN":  newEvaluation -= (knightVal + cellWhiteKnightVal[7 - i, 7 - j]); break;
                        case "R":   newEvaluation -= (rookVal + cellWhiteRookVal[7 - i, 7 - j]); break;
                        case "P":   newEvaluation -= (pawnVal + cellWhitePawnVal[7 - i, 7 - j]); break;
                    }
                }
                else continue;
            }
        }

        return newEvaluation;
    }

    public static double[,] CellVal { get => cellVal; set => cellVal = value; }

    public static double KingVal => kingVal;

    public static double QueenVal => queenVal;

    public static double RookVal => rookVal;

    public static double KnightVal => knightVal;

    public static double BishopVal => bishopVal;

    public static double PawnVal => pawnVal;
}
