using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPiece : MonoBehaviour
{
    [SerializeField] private SOShopSelection shopSelection;
    public SOShopSelection ShopSelection
    {
        get
        {
            return shopSelection;
        }
        set
        {
            shopSelection = value;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (GetComponentInChildren<SpriteRenderer>() != null)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = shopSelection.icon;
        }

        if (transform.Find("itemText"))
        {
            GetComponentInChildren<TextMesh>().text = shopSelection.cost;
        }
    }
}
