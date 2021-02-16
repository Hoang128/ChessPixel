using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    GameObject cellUnder;
    // Start is called before the first frame update
    void Start()
    {
        cellUnder = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cell")
        {
            if (cellUnder == null)
            {
                cellUnder = collision.gameObject;
                cellUnder.GetComponent<Cell>().CanMoveTo = true;
            }
        }
    }

    private void OnDestroy()
    {
        cellUnder.GetComponent<Cell>().CanMoveTo = false;
    }
}
