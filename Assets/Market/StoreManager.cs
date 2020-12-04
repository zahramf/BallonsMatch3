using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BazaarPlugin;
using System;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public string RSICode;
    public Products[] products;
    int productIndex = 0;
    [SerializeField]
    Text CoinTxt;
    [SerializeField]
    GameObject warningPanel;


    void Awake()
    {
        BazaarIAB.init(RSICode);
        CoinTxt.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    public void Pardakht(int index)
    {
        productIndex = index;
        BazaarIAB.purchaseProduct(products[productIndex].Id);
    }
    void OnEnable()
    {
        IABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        IABEventManager.purchaseFailedEvent += purchaseFailedEvent;
    }

    private void purchaseFailedEvent(string error)
    {
        warningPanel.SetActive(true);
    }

    private void purchaseSucceededEvent(BazaarPurchase purchase)
    {
        BazaarIAB.consumeProduct(products[productIndex].Id);
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin")+products[productIndex].Coin);
        CoinTxt.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    void OnDisable()
    {
        IABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        IABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
    }

    public void ClosePanel()
    {
        warningPanel.SetActive(false);
    }
}
[System.Serializable]
public class Products
{
    public string Id;
    public int Coin;

}