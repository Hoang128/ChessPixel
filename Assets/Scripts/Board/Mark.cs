using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    Vector2Int coor;
    [SerializeField] bool isPromoMark = false;
    public Vector2Int Coor { get => coor; set => coor = value; }
    public bool IsPromoMark { get => isPromoMark; set => isPromoMark = value; }


    // Start is called before the first frame update
    void Awake()
    {
        coor = new Vector2Int(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
