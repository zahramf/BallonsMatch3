﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* Apache License. Copyright (C) Bobardo Studio - All Rights Reserved.
 * Unauthorized publishing the plugin with different name is strictly prohibited.
 * This plugin is free and no one has right to sell it to others.
 * http://bobardo.com
 * http://opensource.org/licenses/Apache-2.0
 */

[RequireComponent(typeof(StoreHandler))]
public class InAppStore : MonoBehaviour
{
    public GameObject GmFaild, GmSuccess, GmLoading;
    public Product[] products;

    private int selectedProductIndex;

    void Start()
    {

    }

    public void purchasedSuccessful(Purchase purchase)
    {
        // purchase was successful, give user the pruduct

        switch (selectedProductIndex)
        {
            case 0:
                {
                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 5500);
                }

                break;
            case 1:
                {
                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 12000);
                }

                break;
            case 2:
                {
                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 25000);
                }

                break;
            case 3:
                {
                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 53000);
                }

                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }
        
    }

    public void purchasedFailed(int errorCode, string info)
    {
        // purchase failed. show user the proper message
        switch (errorCode)
        {
            case 1: // error connecting cafeBazaar
            case 2: // error connecting cafeBazaar
            case 4: // error connecting cafeBazaar
            case 5: // error connecting cafeBazaar

                break;
            case 6: // user canceled the purchase

                break;
            case 7: // purchase failed

                break;
            case 8: // failed to consume product. but the purchase was successful.

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

    }

    public void userHasThisProduct(Purchase purchase)
    {
        // user already has this product
        switch (selectedProductIndex)
        {
            case 0: // first product

                break;
            case 1: // second product

                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }
    }

    public void failToGetUserInventory(int errorCode, string info)
    {
        // user has not this product or some error happened
        switch (errorCode)
        {
            case 3:  // error connecting cafeBazaar
            case 10: // error connecting cafeBazaar

                break;
            case 9: // user didn't login to cafeBazaar

                break;
            case 11: // user has not this product

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

    }

    public void purchaseProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        Product product = products[productIndex];
        if (product.type == Product.ProductType.Consumable)
        {
            GetComponent<StoreHandler>().BuyAndConsume(product.productId);
        }
        else if (product.type == Product.ProductType.NonConsumable)
        {
            GetComponent<StoreHandler>().BuyProduct(product.productId);
        }
    }

    public void checkIfUserHasProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        GetComponent<StoreHandler>().CheckInventory(products[productIndex].productId);
    }

    public void CLoseFaildMsg()
    {
        GmFaild.SetActive(false);
    }

    public void CLoseSuccessMsg()
    {
        GmSuccess.SetActive(false);
    }

}

