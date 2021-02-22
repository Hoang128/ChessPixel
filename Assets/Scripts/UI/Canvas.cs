using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    [SerializeField] GameObject promoTable;

    public GameObject PromoTable { get => promoTable; set => promoTable = value; }

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

}
