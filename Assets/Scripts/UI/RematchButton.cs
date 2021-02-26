using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RematchButton : MonoBehaviour
{
    GameObject playBoard;

    // Start is called before the first frame update
    void Start()
    {
        playBoard = GameObject.Find("PlayBoard");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rematch()
    {
        playBoard.GetComponent<PlayBoard>().ResetBoard();
        Destroy(gameObject);
    }
}
