using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Vector2 coor;
    [SerializeField] bool isWhitePiece = true;

    // Start is called before the first frame update
    void Start()
    {
        coor = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
