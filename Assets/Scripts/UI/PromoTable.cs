using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoTable : MonoBehaviour
{
    [SerializeField] bool whiteSide = true;
    [SerializeField] Sprite[] whitePromoPieceImage;
    [SerializeField] Sprite[] blackPromoPieceImage;

    private string promoPiece;
    PiecePromoButton[] promoButtons;


    // Start is called before the first frame update
    void Start()
    {
        promoButtons = GetComponentsInChildren<PiecePromoButton>();

        foreach(PiecePromoButton promoButton in promoButtons)
        {
            if (whiteSide)
            {
                switch (promoButton.Piece)
                {
                    case PiecePromoButton.PromoPiece.QUEEN:
                        promoButton.GetComponent<Image>().sprite = whitePromoPieceImage[0]; break;
                    case PiecePromoButton.PromoPiece.ROOK:
                        promoButton.GetComponent<Image>().sprite = whitePromoPieceImage[1]; break;
                    case PiecePromoButton.PromoPiece.KNIGHT:
                        promoButton.GetComponent<Image>().sprite = whitePromoPieceImage[2]; break;
                    case PiecePromoButton.PromoPiece.BISHOP:
                        promoButton.GetComponent<Image>().sprite = whitePromoPieceImage[3]; break;
                }
            }
            else
            {
                switch (promoButton.Piece)
                {
                    case PiecePromoButton.PromoPiece.QUEEN:
                        promoButton.GetComponent<Image>().sprite = blackPromoPieceImage[0]; break;
                    case PiecePromoButton.PromoPiece.ROOK:
                        promoButton.GetComponent<Image>().sprite = blackPromoPieceImage[1]; break;
                    case PiecePromoButton.PromoPiece.KNIGHT:
                        promoButton.GetComponent<Image>().sprite = blackPromoPieceImage[2]; break;
                    case PiecePromoButton.PromoPiece.BISHOP:
                        promoButton.GetComponent<Image>().sprite = blackPromoPieceImage[3]; break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPromoPiece(string piece)
    {
        PlayBoard boardController = GameObject.Find("PlayBoard").GetComponent<PlayBoard>();
        if (boardController.WhiteTurn)
        {
            boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalPieceChange = piece;
        }
        else
        {
            boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalPieceChange = piece.ToUpper();
        }

        Destroy(gameObject);
    }
}
