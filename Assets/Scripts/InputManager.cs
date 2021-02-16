using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] float fireRate = 1f;

    bool canTouch = true;
    Vector2 touchPoint;
    RaycastHit2D touchHit;

    // Start is called before the first frame update
    void Start()
    {
        
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
                        case "Marker":
                            {
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
                    Debug.Log("Hit none");
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
        //cell = touchHit.collider.gameObject;
        //Vector2 clickPoint = playBoard.GetComponent<PlayBoard>().ClickPoint;
        //Board board = playBoard.GetComponent<PlayBoard>().BoardStack.Peek();

        //if (clickPoint != new Vector2(-1, -1))
        //{
        //    if (canMoveTo == true)
        //    {
        //        if (playBoard.GetComponent<PlayBoard>().WhiteTurn)
        //        {
        //            playBoard.GetComponent<PlayBoard>().PlayerWhite.GetComponent<PlayerMgr>().FinalMovePlace = coor;
        //        }
        //        else
        //        {
        //            playBoard.GetComponent<PlayBoard>().PlayerBlack.GetComponent<PlayerMgr>().FinalMovePlace = coor;
        //        }
        //        canMoveTo = false;
        //    }
        //}
        //else
        //{
        //    if (piece != "0")
        //    {
        //        bool pieceIsWhite = RuleHandler.isWhitePiece(new Vector2(coor.x, coor.y), board.BoardCells);
        //        bool canMove = ((pieceIsWhite && playBoard.GetComponent<PlayBoard>().WhiteTurn)
        //            || (!pieceIsWhite && !playBoard.GetComponent<PlayBoard>().WhiteTurn));
        //        if (canMove)
        //            playBoard.GetComponent<PlayBoard>().ClickPoint = coor;
        //    }
        //}
    }
}
