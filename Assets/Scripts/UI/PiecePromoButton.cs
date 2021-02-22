using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecePromoButton : MonoBehaviour
{
    public enum PromoPiece {QUEEN, ROOK, KNIGHT, BISHOP, NONE};

    [SerializeField] PromoPiece piece;
    [SerializeField] Sprite pieceImage;

    Image imagePiece;

    public Sprite PieceImage { get => pieceImage; set => pieceImage = value; }
    public PromoPiece Piece { get => piece; set => piece = value; }

    // Start is called before the first frame update
    void Awake()
    {
        imagePiece = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
