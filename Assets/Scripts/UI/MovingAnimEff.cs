using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimEff : MonoBehaviour
{
    [SerializeField] private int dotNumber;
    [SerializeField] private float dotSpace;
    [SerializeField] private GameObject dotObject;

    private GameObject[] dots;
    private float waitTime = 1;
    private int dotEnable = 0;
    private bool canStartCoroutine = true;


    // Start is called before the first frame update
    void Start()
    {
        dots = new GameObject[dotNumber];

        for (int i = 0; i < dotNumber; i++)
        {
            dots[i] = GameObject.Instantiate(dotObject, new Vector3(transform.position.x + dotSpace * i, transform.position.y, 0), transform.rotation);
            dots[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canStartCoroutine)
        {
            if (dotEnable < dotNumber)
            {
                StartCoroutine(EnableDotAfter(waitTime, dotEnable));
                canStartCoroutine = false;
            }
            else
            {
                StartCoroutine(DisableAllDotsAfter(waitTime));
                canStartCoroutine = false;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
    }

    IEnumerator EnableDotAfter(float seconds, int dotNum)
    {
        yield return new WaitForSeconds(seconds);

        dots[dotNum].GetComponent<SpriteRenderer>().enabled = true;
        dotEnable++;
        canStartCoroutine = true;
    }

    IEnumerator DisableAllDotsAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        foreach(GameObject dot in dots)
        {
            dot.GetComponent<SpriteRenderer>().enabled = false;
        }
        dotEnable = 0;
        canStartCoroutine = true;
    }
}
