using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] public Sprite whiteKingImage;
    [SerializeField] public Sprite whiteQueenImage;
    [SerializeField] public Sprite whiteBishopImage;
    [SerializeField] public Sprite whiteKnightImage;
    [SerializeField] public Sprite whiteRookImage;
    [SerializeField] public Sprite whitePawnImage;
    [SerializeField] public Sprite blackKingImage;
    [SerializeField] public Sprite blackQueenImage;
    [SerializeField] public Sprite blackBishopImage;
    [SerializeField] public Sprite blackKnightImage;
    [SerializeField] public Sprite blackRookImage;
    [SerializeField] public Sprite blackPawnImage;

    Vector2Int coor;
    bool pieceVisible = true;
    SpriteRenderer sprRenderer;
    public bool PieceVisible { get => pieceVisible; set => pieceVisible = value; }

    
    // Start is called before the first frame update
    void Awake()
    {
        coor = new Vector2Int(0, 0);
        sprRenderer = GetComponent<SpriteRenderer>();
        transform.localPosition += new Vector3(0f, 0f, -5f);
        transform.localScale = new Vector3(0.2f, 0.2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePiece(string piece)
    {
        switch (piece)
        {
            case "0":       sprRenderer.sprite = null;                  break;

            case "k":       sprRenderer.sprite = whiteKingImage;        break;
            case "q":       sprRenderer.sprite = whiteQueenImage;       break;
            case "b":       sprRenderer.sprite = whiteBishopImage;      break;
            case "kn":      sprRenderer.sprite = whiteKnightImage;      break;
            case "r":       sprRenderer.sprite = whiteRookImage;        break;
            case "p":       sprRenderer.sprite = whitePawnImage;        break;

            case "K":       sprRenderer.sprite = blackKingImage;        break;
            case "Q":       sprRenderer.sprite = blackQueenImage;       break;
            case "B":       sprRenderer.sprite = blackBishopImage;      break;
            case "KN":      sprRenderer.sprite = blackKnightImage;      break;
            case "R":       sprRenderer.sprite = blackRookImage;        break;
            case "P":       sprRenderer.sprite = blackPawnImage;        break;
        }
    }
}
