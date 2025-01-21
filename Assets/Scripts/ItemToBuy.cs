using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class ItemToBuy : MonoBehaviour
{
    public int price;
    public string name;

    public void Buyitem()
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = price

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, SubtractCoinSuccess, OnError);
    }

    void SubtractCoinSuccess(ModifyUserVirtualCurrencyResult result)
    {
        InventoryManager.instance.GetVirtualCurrencies();
        InventoryManager.instance.BuyItem("Ships", name, price);

        Destroy(this);
        //REMOVE THIS BUTTON
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("ERROR: " + error.ErrorMessage);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
