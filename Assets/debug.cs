using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    public GameObject showShop;
    private bool isShopActive;

    private void Start()
    {
        showShop.SetActive(false);
        isShopActive = false;
    }

    public void ToggleShop()
    {
        isShopActive = !isShopActive;
        showShop.SetActive(isShopActive);
    }
}
