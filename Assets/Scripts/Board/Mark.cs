using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    Vector2 coor;

    public Vector2 Coor { get => coor; set => coor = value; }

    // Start is called before the first frame update
    void Awake()
    {
        coor = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
