using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PFTest : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {

        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "GettingStartedGuide",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call");

    }
  
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log("Something went wrong withyour first API call. :(");
        Debug.Log(error.GenerateErrorReport());
    }

 
}
