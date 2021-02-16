using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    bool isWhite = true;
    Vector2 finalMovePlace = new Vector2(-1, -1);

    public bool IsWhite { get => isWhite; set => isWhite = value; }
    public Vector2 FinalMovePlace { get => finalMovePlace; set => finalMovePlace = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Board Move(Vector2 piecePlace, Vector2 movePlace, Board board)
    {
        return (RuleHandler.movePiece(piecePlace, movePlace, board));
    }
}
