using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject RegisterUi, LoginUi, StartButtons, AfterloginButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToRegister()
    {
        RegisterUi.SetActive(true);
        StartButtons.SetActive(false);
    }

    public void GoToLogin()
    {
        LoginUi.SetActive(true);
        StartButtons.SetActive(false);
        RegisterUi.SetActive(false);
    }

    public void LoggedInUI()
    {
        SceneManager.LoadScene("Menu 1");
      
    }
}
