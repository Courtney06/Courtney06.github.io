using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab.ProgressionModels;
using UnityEngine.UI;


public class PFLeaderboard : MonoBehaviour
{
    [SerializeField] TMP_Text msgbox;
    [SerializeField] GameObject PlayerData;
    [SerializeField] Transform Localscroll, Globalscroll, FriendScroll;
   
    [SerializeField] GameObject leaderboard;

    private string currentcountrycode;
    public void OnButtonGetLeaderboard()
    {
        var lbreq = new GetLeaderboardRequest
        {
            StatisticName = "highscore",
            StartPosition = 0,
            MaxResultsCount = 100,

        };

        PlayFabClientAPI.GetLeaderboard(lbreq, OnLeaderboardGet, OnError);

    }


    public void OpenLeaderboard()
    {
        leaderboard.SetActive(true);
    }

    public void closeleaderboard()
    {
        leaderboard.SetActive(false);
        foreach (Transform child in Globalscroll.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Localscroll.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void OnLeaderboardGet(GetLeaderboardResult r)
    {
        string LeaderboardStr = "Leaderboard\n";
        foreach (var item in r.Leaderboard)
        {
            GameObject playerdataprefab = Instantiate(PlayerData);
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = item.PlayFabId
            }, result =>
            {
               
                if (result.Data == null || !result.Data.ContainsKey("CountryCode")) Debug.Log("No CountryCode");
                else
                {
                     currentcountrycode = result.Data["CountryCode"].Value;
                    Debug.Log("Got User data:");
                }
            }, (error) =>
            {
                Debug.Log("Got Error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });

            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
               
            }, result =>
            {
                Debug.Log("Got User data:");
                if (result.Data == null || !result.Data.ContainsKey("CountryCode")) Debug.Log("No CountryCode");
                else
                {
                    Debug.Log("CountryCode: " + result.Data["CountryCode"].Value);
                    playerdataprefab.transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>().text = result.Data["CountryCode"].Value;

                    if (result.Data["CountryCode"].Value == currentcountrycode)
                    {
                        playerdataprefab.transform.parent = Localscroll.transform;
                        playerdataprefab.transform.localScale = Vector3.one;
                    }
                    else
                    {
                        playerdataprefab.transform.parent = Globalscroll.transform;
                        playerdataprefab.transform.localScale = Vector3.one;

                    }
                   
                }


            }, (error) =>
            {
                Debug.Log("Got Error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });

           
           

            Debug.Log(playerdataprefab.transform.GetChild(0));
            playerdataprefab.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = item.DisplayName;
            playerdataprefab.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = (item.StatValue).ToString();

            
            // You need to get the CountryCode using PlayerProfile API

        }
    }



    public void OnButtonSentLeaderboard(string Score)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<PlayFab.ClientModels.StatisticUpdate> // Fully qualified name
        {
            new PlayFab.ClientModels.StatisticUpdate
            {
                StatisticName = "highscore",
                Value = int.Parse(Score)
            }
        }
        };
        //UpdateMsg("SubmittingReport" + Score);
        PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult r)
    {
        Debug.Log("Successful leaderboard send:" + r.ToString());
    }

    void OnError(PlayFabError error)
    {
        msgbox.text = error.GenerateErrorReport();
    }

    void UpdateMsg(string msg)
    {
        msgbox.text = msg;
    }
}
