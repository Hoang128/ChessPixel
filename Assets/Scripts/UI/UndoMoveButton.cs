using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoMoveButton : MonoBehaviour
{
    [SerializeField] GameObject playBoard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playBoard.GetComponent<PlayBoard>().BoardStack.Count > 2)
        {
            if (transform.GetComponent<Button>().interactable == false)
                transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            if (transform.GetComponent<Button>().interactable == true)
                transform.GetComponent<Button>().interactable = false;
        }
    }

    public void UndoMove()
    {
        playBoard.GetComponent<PlayBoard>().UndoMove();
    }
}
