using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{

    GameObject playBoard;

    Vector2Int coor;
    string piece;
    BoxCollider2D boxCol2D;

    public string Piece { get => piece; set => piece = value; }
    public Vector2Int Coor { get => coor; set => coor = value; }

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init()
    {
        coor.x = 0;
        coor.y = 0;

        playBoard = GameObject.Find("PlayBoard");

        boxCol2D = GetComponent<BoxCollider2D>();
    }

    private void OnDrawGizmos()
    {
        if (playBoard.GetComponent<PlayBoard>().BoardStack.Count > 0)
        {
            //string piece = playBoard.GetComponent<PlayBoard>().BoardStack.Peek().BoardCells[coor.x, coor.y];
            //Handles.Label(transform.position - new Vector3(2f, -2f, 0f), coor.x + ", " + coor.y);
            //Handles.Label(transform.position - new Vector3(2f, -1f, 0f), piece);
        }
    }
}
