using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] float fireRate = 1f;

    bool canTouch = true;
    Vector2 touchPoint;
    RaycastHit2D touchHit;
    GameObject playBoard;

    // Start is called before the first frame update
    void Start()
    {
        playBoard = GameObject.Find("PlayBoard");
    }

    // Update is called once per frame
    void Update()
    {
        TouchHandle();
    }

    void TouchHandle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canTouch)
            {
                touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchHit = Physics2D.Raycast(touchPoint, Vector2.zero);

                if (touchHit)
                {
                    switch (touchHit.collider.gameObject.tag)
                    {
                        case "Move Marker":
                        case "Capture Marker":
                        case "Promo Marker":
                            {
                                HandleMoveMarkTouch(touchHit.collider.gameObject);

                                Debug.Log("Handled marker touch!");
                            }
                            break;
                        case "Cell":
                            {
                                HandleCellTouch(touchHit.collider.gameObject);

                                Debug.Log("Handled cell touch!");
                            }   
                            break;
                    }
                }
                else
                {
                    if (playBoard.GetComponent<PlayBoard>().StateName == "Board Choose Move")
                    {
                        playBoard.GetComponent<PlayBoard>().ClickPoint = new Vector2Int(-1, -1);
                    }
                }

                canTouch = false;
                StartCoroutine(enableTouchAfter(fireRate));
            }
        }
    }

    IEnumerator enableTouchAfter(float seconds)
    {
        yield return new WaitForSeconds(fireRate);

        canTouch = true;
    }

    void HandleCellTouch(GameObject cell)
    {
        if (playBoard.GetComponent<PlayBoard>().StateName == "Board Idle")
        {
            if (cell.GetComponent<Cell>().Piece != "0")
            {
                bool whiteTurn = playBoard.GetComponent<PlayBoard>().WhiteTurn;
                bool whitePiece = RuleHandler.isWhitePiece(cell.GetComponent<Cell>().Coor, playBoard.GetComponent<PlayBoard>().BoardStack.Peek().BoardCells);

                if ((whiteTurn && whitePiece) || (!whiteTurn && !whitePiece))
                    playBoard.GetComponent<PlayBoard>().ClickPoint = cell.GetComponent<Cell>().Coor;
            }
        }
        else if (playBoard.GetComponent<PlayBoard>().StateName == "Board Choose Move")
        {
            playBoard.GetComponent<PlayBoard>().ClickPoint = new Vector2Int(-1, -1);
        }
    }

    void HandleMoveMarkTouch(GameObject mark)
    {
        if (playBoard.GetComponent<PlayBoard>().WhiteTurn)
        {
            if (playBoard.GetComponent<PlayBoard>().PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace == new Vector2Int(-1, -1))
            {
                Vector2Int move = mark.GetComponent<Mark>().Coor;
                playBoard.GetComponent<PlayBoard>().PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace = move;
            }
        }
        else
        {
            if (playBoard.GetComponent<PlayBoard>().PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace == new Vector2Int(-1, -1))
            {
                Vector2Int move = mark.GetComponent<Mark>().Coor;
                playBoard.GetComponent<PlayBoard>().PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace = move;
            }
        }
    }
}
