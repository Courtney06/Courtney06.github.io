using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab.ProgressionModels;



public class PFUserMgt : MonoBehaviour
{
    [SerializeField] TMP_Text msgbox;
    [SerializeField] TMP_InputField if_username,if_email,if_password, currentScore;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_InputField Log_Email, Log_Password;
    [SerializeField] GameObject displaynamewindow;
    [SerializeField] TMP_InputField Displaynameinput;
    [SerializeField] UIManager uimanager;


    
    public void OnButtonRegUser()
    {
        var regReq = new RegisterPlayFabUserRequest
        {
            Email = if_email.text,
            Password = if_password.text,
            Username = if_username.text,

        };
        PlayFabClientAPI.RegisterPlayFabUser(regReq, OnRegSucc, OnError);
    }

    void OnRegSucc(RegisterPlayFabUserResult r)
    {
        msgbox.text = "Register Success" + r.PlayFabId;
        
        string name = null;
       
    }

  
    void OnError(PlayFabError error)
    {
        msgbox.text = error.GenerateErrorReport();
    }

    public void OnButtonLogin()
    {
        var loginReg = new LoginWithEmailAddressRequest
        {
            Email = Log_Email.text,
            Password = Log_Password.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true, // Retrieve the player's profile, including the display name.
                 ProfileConstraints = new PlayerProfileViewConstraints
                 {
                     ShowLocations = true, // Request location data.
                     ShowDisplayName = true
                 }
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginReg, OnLoginSucc, OnError);
    }

    void OnLoginSucc(LoginResult r)
    {
        msgbox.text = "Login Success: " + r.PlayFabId;

        // Retrieve the display name from the player's profile.
        if (r.InfoResultPayload.PlayerProfile != null)
        {
            name = r.InfoResultPayload.PlayerProfile.DisplayName;

          
        }

        Debug.Log("Display Name: " + name);

        // If the display name is not set, prompt the player to input one.
        if (string.IsNullOrEmpty(name))
        {
            displaynamewindow.SetActive(true);
            uimanager.LoginUi.SetActive(false);
            Debug.Log("Prompting player to set a display name.");
        }
        else
        {
            // Display name is already set; proceed to the next level.
            LoadLevel();
        }

        var countrycode = r.InfoResultPayload.PlayerProfile.Locations[0].CountryCode;

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
         {
             { "CountryCode",  countrycode.ToString() }
         }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data xp");
            Debug.Log(error.GenerateErrorReport());
        });

    }

    public void SubmitDisplayName()
    {
        // Save the display name inputted by the player.
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = Displaynameinput.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult r)
    {
        Debug.Log("Display Name Updated Successfully: " + r.DisplayName);
        LoadLevel(); // Proceed to the next level after updating the display name.
    }

  
    void LoadLevel()
    {
        SceneManager.LoadScene("Menu 1");
    }

    public void OnLogOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadScene("Menu");
    }

    public void OnResetPassword()
    {
        var ResetPassReq = new SendAccountRecoveryEmailRequest
        {
            Email = if_email.text,
            TitleId = PlayFabSettings.TitleId
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(ResetPassReq, OnResetPassSucc, OnError);
    }

    void OnResetPassSucc(SendAccountRecoveryEmailResult r)
    {
        msgbox.text = "Recovery Email has been sent to your email";
    }

    void UpdateMsg(string msg)
    {
        msgbox.text = msg;
    }

    public void ClientGetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (result.Data == null || !result.Data.ContainsKey("MOTD")) UpdateMsg("No MOTD");
                else UpdateMsg("MOTD: " + result.Data["MOTD"]);
            },
            error =>
            {
                UpdateMsg("Got Error getting titleData");
                UpdateMsg(error.GenerateErrorReport());
            }
            );
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

