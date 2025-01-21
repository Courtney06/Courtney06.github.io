using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PFDataMGr : MonoBehaviour
{
    [SerializeField] TMP_Text XPDisplay;
    [SerializeField] TMP_InputField XPInput;

    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { "XP", XPInput.text.ToString() },
            }
            },
            result => Debug.Log("Successfully updated user data"),
            error =>
            {
                Debug.Log("Got error setting user data xp");
                Debug.Log(error.GenerateErrorReport());
            });
        
            
    }

    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {

        }, result =>
        {
            Debug.Log("Got User data:");
            if (result.Data == null || !result.Data.ContainsKey("XP")) Debug.Log("No XP");
            else
            {
                Debug.Log("XP: " + result.Data["XP"].Value);
                XPDisplay.text = "XP:" + result.Data["XP"].Value;
            }
        }, (error) =>
        {
            Debug.Log("Got Error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
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
