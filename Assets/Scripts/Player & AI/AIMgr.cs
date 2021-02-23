using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMgr : MonoBehaviour
{
    private Vector2Int finalPiecePlace;
    private Vector2Int finalMovePlace;
    private string finalPieceChange;

    public Vector2Int FinalPiecePlace { get => finalPiecePlace; set => finalPiecePlace = value; }
    public Vector2Int FinalMovePlace { get => finalMovePlace; set => finalMovePlace = value; }
    public string FinalPieceChange { get => finalPieceChange; set => finalPieceChange = value; }

    // Start is called before the first frame update
    void Awake()
    {
        finalPiecePlace = new Vector2Int(-1, -1);
        finalMovePlace = new Vector2Int(-1, -1);
        finalPieceChange = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
