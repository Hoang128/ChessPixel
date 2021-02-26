using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    [SerializeField] GameObject promoTable;
    [SerializeField] GameObject resultTable;

    GameObject table = null;

    public GameObject PromoTable { get => promoTable; set => promoTable = value; }
    public GameObject ResultTable { get => resultTable; set => resultTable = value; }
    public GameObject Table { get => table; set => table = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePromoTable()
    {
        GameObject.Instantiate(PromoTable, transform, false);
    }

    public void CreateResultTable()
    {
        Table = GameObject.Instantiate(ResultTable, transform, false);
    }
}
