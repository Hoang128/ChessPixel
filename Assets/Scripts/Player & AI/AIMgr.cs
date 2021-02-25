using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static RuleHandler;
using System.Linq;

public class AIMgr : MonoBehaviour
{
    public enum AIType { GREEDY, MINIMAX_1, MINIMAX_2, MINIMAX_3, MINIMAX_4, MINIMAX_5}

    AIType AI = AIType.MINIMAX_3;
    private Vector2Int finalPiecePlace;
    private Vector2Int finalMovePlace;
    private string finalPieceChange;
    private Stack<RuleHandler.MoveList> allMoveList;
    private bool whiteSide = true;
    private const double MAX_EVALUATION = 100000;
    private const double MIN_EVALUATION = -100000;
    private bool canMove = false;
    Thread findMoveThread;
    Board currentBoard;

    private GameObject boardController;

    public Vector2Int FinalPiecePlace { get => finalPiecePlace; set => finalPiecePlace = value; }
    public Vector2Int FinalMovePlace { get => finalMovePlace; set => finalMovePlace = value; }
    public string FinalPieceChange { get => finalPieceChange; set => finalPieceChange = value; }
    public Stack<MoveList> AllMoveList { get => allMoveList; set => allMoveList = value; }
    public bool WhiteSide { get => whiteSide; set => whiteSide = value; }
    public bool CanMove { get => canMove; set => canMove = value; }

    // Start is called before the first frame update
    void Awake()
    {
        finalPiecePlace = new Vector2Int(-1, -1);
        finalMovePlace = new Vector2Int(-1, -1);
        finalPieceChange = "0";
        boardController = GameObject.Find("PlayBoard");
    }

    void Start()
    {
        currentBoard = boardController.GetComponent<PlayBoard>().BoardStack.Peek();
        //FindBestMove(boardController.GetComponent<PlayBoard>().BoardStack.Peek(),whiteSide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFindMoveThread(Board currentBoard, bool whiteSide)
    {
        findMoveThread = new Thread
            (() => { FindBestMove(currentBoard, whiteSide); });
        findMoveThread.Start();
    }

    public void FindBestMove(Board currentBoard, bool whiteSide)
    {
        int gameState = GetGameState(currentBoard);
        if (gameState != 0)
            return;
        else
        {
            switch (AI)
            {
                case AIType.GREEDY: FindBestMoveGreedy(currentBoard, whiteSide); break;
                case AIType.MINIMAX_1: FindBestMoveMinimax(currentBoard, whiteSide, 1); break;
                case AIType.MINIMAX_2: FindBestMoveMinimax(currentBoard, whiteSide, 2); break;
                case AIType.MINIMAX_3: FindBestMoveMinimaxAlphaBetaPruning(currentBoard, whiteSide, 3); break;
                case AIType.MINIMAX_4: FindBestMoveMinimax(currentBoard, whiteSide, 4); break;
                case AIType.MINIMAX_5: FindBestMoveMinimax(currentBoard, whiteSide, 5); break;
            }
        }

        canMove = true;
    }

    void FindBestMoveGreedy(Board currentBoard, bool whiteSide)
    {
        double bestEvaluation = 0;

        if (whiteSide)
            bestEvaluation = MIN_EVALUATION;
        else
            bestEvaluation = MAX_EVALUATION;

        Stack<MoveList> allMoves = new Stack<MoveList>();
        allMoves = GetAllMoveInBoard(currentBoard, whiteSide);
        Debug.Log("moveable pieces = " + allMoves.Count);
        string logSide = "White";
        if (!whiteSide)
            logSide = "Black";

        foreach(MoveList pieceMoveList in allMoves)
        {
            foreach(Vector2Int tempMovePlace in pieceMoveList.movePlace)
            {
                Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                string tempPieceChange = "0";
                if (pieceMoveList.pieceAfterMove.Count != 0)
                {
                    tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(tempMovePlace)];
                }

                Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);

                Board tempBoard = new Board(currentBoard);
                tempBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);
                if (whiteSide)
                {
                    if (tempBoard.Evaluation > bestEvaluation)
                    {
                        bestEvaluation = tempBoard.Evaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log(logSide + " choose move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);
                    }
                }
                else
                {
                    if (tempBoard.Evaluation < bestEvaluation)
                    {
                        bestEvaluation = tempBoard.Evaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log(logSide + " choose move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);
                    }
                }
            }
        }

        if (finalPieceChange == "0")
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalMovePlace.x + ", " + finalMovePlace.y);
        else
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalPieceChange + " " + finalMovePlace.x + ", " + finalMovePlace.y);
        Debug.Log("evaluation = " + bestEvaluation);
    }

    void FindBestMoveMinimax(Board currentBoard, bool whiteSide, int depth)
    {
        double bestEvaluation = 0;

        if (whiteSide)
            bestEvaluation = MIN_EVALUATION;
        else
            bestEvaluation = MAX_EVALUATION;

        Stack<MoveList> allMoves = new Stack<MoveList>();
        allMoves = GetAllMoveInBoard(currentBoard, whiteSide);
        Debug.Log("moveable pieces = " + allMoves.Count);
        string logSide = "White";
        if (!whiteSide)
            logSide = "Black";

        foreach (MoveList pieceMoveList in allMoves)
        {
            foreach (Vector2Int tempMovePlace in pieceMoveList.movePlace)
            {
                Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                string tempPieceChange = "0";
                if (pieceMoveList.pieceAfterMove.Count != 0)
                {
                    tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(tempMovePlace)];
                }

                Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);

                Board tempBoard = new Board(currentBoard);
                tempBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);
                double moveEvaluation = Minimax(tempBoard, depth - 1, !whiteSide);
                if (whiteSide)
                {
                    if (moveEvaluation > bestEvaluation)
                    {
                        bestEvaluation = moveEvaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log(logSide + " choose move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);
                    }
                }
                else
                {
                    if (moveEvaluation < bestEvaluation)
                    {
                        bestEvaluation = moveEvaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log(logSide + " choose move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);
                    }
                }
            }
        }

        if (finalPieceChange == "0")
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalMovePlace.x + ", " + finalMovePlace.y);
        else
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalPieceChange + " " + finalMovePlace.x + ", " + finalMovePlace.y);
        Debug.Log("evaluation = " + bestEvaluation);
    }

    void FindBestMoveMinimaxAlphaBetaPruning(Board currentBoard, bool whiteSide, int depth)
    {
        double bestEvaluation = 0;

        if (whiteSide)
            bestEvaluation = MIN_EVALUATION;
        else
            bestEvaluation = MAX_EVALUATION;

        Stack<MoveList> allMoves = new Stack<MoveList>();
        allMoves = GetAllMoveInBoard(currentBoard, whiteSide);
        Debug.Log("moveable pieces = " + allMoves.Count);
        string logSide = "White";
        if (!whiteSide)
            logSide = "Black";

        foreach (MoveList pieceMoveList in allMoves)
        {
            foreach (Vector2Int tempMovePlace in pieceMoveList.movePlace)
            {
                Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                string tempPieceChange = "0";
                if (pieceMoveList.pieceAfterMove.Count != 0)
                {
                    tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(tempMovePlace)];
                }

                Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);

                Board tempBoard = new Board(currentBoard);
                tempBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);
                double moveEvaluation = MinimaxAlphaBetaPruning(tempBoard, depth - 1, !whiteSide, MIN_EVALUATION, MAX_EVALUATION);
                if (whiteSide)
                {
                    if (moveEvaluation > bestEvaluation)
                    {
                        bestEvaluation = moveEvaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log(logSide + " choose move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);
                    }
                }
                else
                {
                    if (moveEvaluation < bestEvaluation)
                    {
                        bestEvaluation = moveEvaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log(logSide + " choose move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);
                    }
                }
            }
        }

        if (finalPieceChange == "0")
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalMovePlace.x + ", " + finalMovePlace.y);
        else
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalPieceChange + " " + finalMovePlace.x + ", " + finalMovePlace.y);
        Debug.Log("evaluation = " + bestEvaluation);
    }

    double Minimax(Board board, int depth, bool whiteSide)
    {
        if (depth == 0)
            return board.Evaluation;
        else
        {
            Board currentBoard = new Board(board);
            Stack<MoveList> allMoves = new Stack<MoveList>();
            allMoves = GetAllMoveInBoard(currentBoard, whiteSide);
            Debug.Log("moveable pieces = " + allMoves.Count);
            string logSide = "White";
            if (!whiteSide)
                logSide = "Black";

            if (allMoves.Count == 0)
            {
                int gameState = GetGameState(currentBoard);
                if (Mathf.Abs(gameState) == 2) return 0;
                else if (gameState == 1)
                {
                    if (whiteSide)
                        return MAX_EVALUATION;
                    else
                        return MIN_EVALUATION;
                }
                else if (gameState == -1)
                {
                    if (whiteSide)
                        return MIN_EVALUATION;
                    else
                        return MAX_EVALUATION;
                }
            }

            if (whiteSide)
            {
                double bestEvaluation = MIN_EVALUATION;

                foreach (MoveList pieceMoveList in allMoves)
                {
                    foreach (Vector2Int movePlace in pieceMoveList.movePlace)
                    {
                        Board tempBoard = new Board(currentBoard);
                        Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                        Vector2Int tempMovePlace = movePlace;
                        string tempPieceChange = "0";
                        if (pieceMoveList.pieceAfterMove.Count != 0)
                        {
                            tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(movePlace)];
                        }
                        Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);

                        Board tempChildBoard = new Board(tempBoard);
                        tempChildBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);

                        double currentEvaluation = Minimax(tempChildBoard, depth - 1, !whiteSide);
                        if (currentEvaluation > bestEvaluation)
                        {
                            bestEvaluation = currentEvaluation;
                        }
                    }
                }

                Debug.Log("evaluation = " + bestEvaluation);
                return bestEvaluation;
            }
            else
            {
                double bestEvaluation = MAX_EVALUATION;

                foreach (MoveList pieceMoveList in allMoves)
                {
                    foreach (Vector2Int movePlace in pieceMoveList.movePlace)
                    {
                        Board tempBoard = new Board(currentBoard);
                        Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                        Vector2Int tempMovePlace = movePlace;
                        string tempPieceChange = "0";
                        if (pieceMoveList.pieceAfterMove.Count != 0)
                        {
                            tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(movePlace)];
                        }
                        Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);

                        Board tempChildBoard = new Board(tempBoard);
                        tempChildBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);

                        double currentEvaluation = Minimax(tempChildBoard, depth - 1, !whiteSide);
                        if (currentEvaluation < bestEvaluation)
                        {
                            bestEvaluation = currentEvaluation;
                        }
                    }
                }

                Debug.Log("evaluation = " + bestEvaluation);
                return bestEvaluation;
            }
        }
    }

    double MinimaxAlphaBetaPruning(Board board, int depth, bool whiteSide, double alpha, double beta)
    {
        if (depth == 0)
            return board.Evaluation;
        else
        {
            Board currentBoard = new Board(board);
            Stack<MoveList> allMoves = new Stack<MoveList>();
            allMoves = GetAllMoveInBoard(currentBoard, whiteSide);
            Debug.Log("moveable pieces = " + allMoves.Count);
            string logSide = "White";
            if (!whiteSide)
                logSide = "Black";

            if (allMoves.Count == 0)
            {
                int gameState = GetGameState(currentBoard);
                if (Mathf.Abs(gameState) == 2) return 0;
                else if (gameState == 1)
                {
                    if (whiteSide)
                        return MAX_EVALUATION;
                    else
                        return MIN_EVALUATION;
                }
                else if (gameState == -1)
                {
                    if (whiteSide)
                        return MIN_EVALUATION;
                    else
                        return MAX_EVALUATION;
                }
            }

            if (whiteSide)
            {
                double bestEvaluation = MIN_EVALUATION;

                foreach (MoveList pieceMoveList in allMoves)
                {
                    foreach (Vector2Int movePlace in pieceMoveList.movePlace)
                    {
                        Board tempBoard = new Board(currentBoard);
                        Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                        Vector2Int tempMovePlace = movePlace;
                        string tempPieceChange = "0";
                        if (pieceMoveList.pieceAfterMove.Count != 0)
                        {
                            tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(movePlace)];
                        }
                        Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);

                        Board tempChildBoard = new Board(tempBoard);
                        tempChildBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);

                        double currentEvaluation
                            = MinimaxAlphaBetaPruning(tempChildBoard, depth - 1, !whiteSide, alpha, beta);
                        if (currentEvaluation > bestEvaluation)
                        {
                            bestEvaluation = currentEvaluation;
                        }
                        if (bestEvaluation > alpha)
                        {
                            alpha = bestEvaluation;
                        }
                        if (beta <= alpha)
                            break;
                    }
                }

                Debug.Log("evaluation = " + bestEvaluation);
                return bestEvaluation;
            }
            else
            {
                double bestEvaluation = MAX_EVALUATION;

                foreach (MoveList pieceMoveList in allMoves)
                {
                    foreach (Vector2Int movePlace in pieceMoveList.movePlace)
                    {
                        Board tempBoard = new Board(currentBoard);
                        Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                        Vector2Int tempMovePlace = movePlace;
                        string tempPieceChange = "0";
                        if (pieceMoveList.pieceAfterMove.Count != 0)
                        {
                            tempPieceChange = pieceMoveList.pieceAfterMove[pieceMoveList.movePlace.IndexOf(movePlace)];
                        }
                        Debug.Log(logSide + " search move: " + currentBoard.BoardCells[tempPiecePlace.x, tempPiecePlace.y] + " from " + tempPiecePlace.x + ", " + tempPiecePlace.y + " to " + tempPieceChange + " " + tempMovePlace.x + ", " + tempMovePlace.y);
                        Debug.Log("evaluation = " + bestEvaluation);

                        Board tempChildBoard = new Board(tempBoard);
                        tempChildBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);

                        double currentEvaluation 
                            = MinimaxAlphaBetaPruning(tempChildBoard, depth - 1, !whiteSide, alpha, beta);
                        if (currentEvaluation < bestEvaluation)
                        {
                            bestEvaluation = currentEvaluation;
                        }
                        if (bestEvaluation < beta)
                        {
                            beta = bestEvaluation;
                        }
                        if (beta <= alpha)
                            break;
                    }
                }

                Debug.Log("evaluation = " + bestEvaluation);
                return bestEvaluation;
            }
        }
    }

    int GetGameState(Board currentBoard)
    {
        Vector2Int whiteKingPlace = new Vector2Int(-1, -1);
        Vector2Int blackKingPlace = new Vector2Int(-1, -1);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (currentBoard.BoardCells[i, j] == "k")
                {
                    whiteKingPlace = new Vector2Int(i, j);
                    break;
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (currentBoard.BoardCells[i, j] == "K")
                {
                    blackKingPlace = new Vector2Int(i, j);
                    break;
                }
            }
        }

        Stack<MoveList> allMoves = new Stack<MoveList>();
        allMoves = GetAllMoveInBoard(currentBoard, true);
        if (allMoves.Count == 0)
        {
            if (IsCheckedCell(whiteKingPlace, currentBoard, true))
            {
                Debug.Log("Black checkmate!");
                Debug.Log("White lose!");
                return -1;
            }
            else
            {
                Debug.Log("Can't move any piece!");
                Debug.Log("White save the game! draw!");
                return -2;
            }
        }

        allMoves.Clear();
        allMoves = GetAllMoveInBoard(currentBoard, false);
        if (allMoves.Count == 0)
        {
            if (IsCheckedCell(blackKingPlace, currentBoard, true))
            {
                Debug.Log("White checkmate!");
                Debug.Log("Black lose!");
                return 1;
            }
            else
            {
                Debug.Log("Can't move any piece!");
                Debug.Log("Black save the game! draw!");
                return 2;
            }
        }

        return 0;
    }

    Stack<MoveList> GetAllMoveInBoard(Board currentBoard, bool whiteSide)
    {
        allMoveList = new Stack<MoveList>();
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (currentBoard.BoardCells[i, j] != "0")
                {
                    bool isWhitePiece = RuleHandler.isWhitePiece(new Vector2Int(i, j), currentBoard.BoardCells);
                    if ((whiteSide && isWhitePiece) || (!whiteSide && !isWhitePiece))
                    {
                        MoveList moveList = new MoveList();
                        moveList = RuleHandler.FindPieceMove(new Vector2Int(i, j), currentBoard, isWhitePiece);
                        if (moveList.movePlace.Count != 0)
                            allMoveList.Push(moveList);
                    }
                }
            }
        }

        return allMoveList;
    }    
}
