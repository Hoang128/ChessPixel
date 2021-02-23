using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    bool isWhite = true;
    Vector2Int finalMovePlace = new Vector2Int(-1, -1);
    string finalPieceChange = "0";

    public bool IsWhite { get => isWhite; set => isWhite = value; }
    public Vector2Int FinalMovePlace { get => finalMovePlace; set => finalMovePlace = value; }
    public string FinalPieceChange { get => finalPieceChange; set => finalPieceChange = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Board Move(Vector2Int piecePlace, Vector2Int movePlace, string newPiece, Board board)
    {
        return (RuleHandler.MovePiece(piecePlace, movePlace, newPiece, board));
    }
}
