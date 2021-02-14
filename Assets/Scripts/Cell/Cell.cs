using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public struct Coordinate
    {
        public int x;
        public int y;
    }

    GameObject playBoard;

    Coordinate coor;
    string piece;

    public string Piece { get => piece; set => piece = value; }

    public void SetCoor(int x, int y)
    {
        coor.x = x;
        coor.y = y;
    }

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
    }

    private void OnDrawGizmos()
    {
        string piece = playBoard.GetComponent<PlayBoard>().BoardStack.Peek().BoardCells[coor.x, coor.y];
        Handles.Label(transform.position - new Vector3(2f, -2f, 0f), coor.x + ", " + coor.y);
        Handles.Label(transform.position - new Vector3(2f, -1f, 0f), piece);
    }
}
