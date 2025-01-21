using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField] TextMeshProUGUI Msg, Coins;
    public List<GameObject> shopbuttons;
    [SerializeField] GameObject shopUI;
    public List<GameObject> Vehicles;
    public GameObject CharacterList;
    [SerializeField] GameObject Shop, Userdata;

    void Start()
    {
       // GetVirtualCurrencies();
        instance = this;

        foreach (Transform child in shopUI.transform)
        {
            shopbuttons.Add(child.gameObject);
        }

        foreach (Transform child in CharacterList.transform)
        {
            
            Vehicles.Add(child.gameObject);
        }
    }

    public void OpenShop()
    {
        Shop.SetActive(true);
    }
    public void CloseShop()
    {
        Shop.SetActive(false);
    }

    public void OpenUserData()
    {
        Userdata.SetActive(true);
    }
    public void CloseUserData()
    {
        Userdata.SetActive(false);
    }

    void UpdateMsg(string msg) //to display in console and messagebox
    {
        Debug.Log(msg);
        Msg.text+=msg+'\n';
    }
    public void OnError(PlayFabError e)
    {
        Debug.Log(e.GenerateErrorReport());
    }
    public void LoadScene(string scn){
        UnityEngine.SceneManagement.SceneManager.LoadScene(scn);
    }
    public void GrantVirtualCurrency()
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = 1
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnGrantVirtualCurrencySuccess, OnError);
    }

    void OnGrantVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Currency Granted");
        GetVirtualCurrencies();
    }

    public void GetVirtualCurrencies(){ //get player virtual currency
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                Debug.Log("GetVirtualCurrency");
                int coins = r.VirtualCurrency["CN"]; //replace CN with your currency
                Coins.text = "Coins:" + coins;
            }, OnError);
    }
    
    public void GetCatalog(){ //get list of items that can be bought
        var catreq=new GetCatalogItemsRequest{
            CatalogVersion="Ships" //update catalog name
        };
        PlayFabClientAPI.GetCatalogItems(catreq,
        result=>{
            List<CatalogItem> items=result.Catalog;
            UpdateMsg("Catalog Items");
            foreach(CatalogItem i in items){
                UpdateMsg(i.DisplayName+","+i.VirtualCurrencyPrices["CN"]); 
                //change "CN" to virtual currency type
            }
            },OnError);
    }

    public void GetPlayerInventory(){ //items in user inventory
        var UserInv=new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(UserInv,
        result=>{
            List<ItemInstance> ii=result.Inventory;
            //UpdateMsg("Player Inventory");
            foreach(ItemInstance i in ii){
                Debug.Log(i.DisplayName+","+i.ItemId+","+i.ItemInstanceId);
                Debug.Log("Test");
                for (int j = 0; j < shopbuttons.Count; j++)
                {
                    if (shopbuttons[j] != null)
                    {
                        if (shopbuttons[j].GetComponent<ItemToBuy>().name == i.ItemId)
                        {
                            Destroy(shopbuttons[j]);
                            shopbuttons[j] = null;

                        }
                    }
                   
                }

                for (int j = 0; j < Vehicles.Count; j++)
                {
                    if (Vehicles[j] != null)
                    {
                        if (Vehicles[j].GetComponent<VehicleBoughtScript>().name == i.ItemId)
                        {

                            Vehicles[j].GetComponent<VehicleBoughtScript>().Owned = true;

                        }
                    }

                }

            }
            },OnError);

    }

    public void BuyItem(string catalog, string item, int price){ //buy item
        var buyreq=new PurchaseItemRequest{
            //current sample is hardcoded, should make it more dynamic
            CatalogVersion=catalog, 
            ItemId=item, //replace with your item id
            VirtualCurrency="CN",
            Price= price
        };
        PlayFabClientAPI.PurchaseItem(buyreq,
            result=>{Debug.Log("Bought!");},
            OnError);

    }
}
//Sell items (use cloud scripts)
//https://github.com/MicrosoftDocs/playfab-docs/blob/docs/playfab-docs/features/data/playerdata/player-inventory.md

