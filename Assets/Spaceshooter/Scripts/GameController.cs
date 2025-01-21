using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Vector3 positionAsteroid;
    public GameObject asteroid;
    public GameObject asteroid2;
    public GameObject asteroid3;
    public int hazardCount;
    public float startWait;
    public float spawnWait;
    public float waitForWaves;
    public Text scoreText;
    public Text gameOverText;
    public Text restartText;
    public Text mainMenuText;

    private bool restart;
    public static bool gameOver;
    private int score;
    private List<GameObject> asteroids;
    public GameObject PausedMenu;
    public PFLeaderboard usermanager;
    public InventoryManager inventorymanager;

    int PlayerLevel;
    int PlayerXp;


    private void Start() {
        asteroids = new List<GameObject> {
            asteroid,
            asteroid2,
            asteroid3
        };
        gameOverText.text = "";
        restartText.text = "";
        mainMenuText.text = "";
        restart = false;
        gameOver = false;
        score = 0;
        StartCoroutine(spawnWaves());
        updateScore();
    }

    private void Update() {
        Debug.Log(score);
        if (restart){
           
            if(Input.GetKey(KeyCode.R)){
                usermanager.OnButtonSentLeaderboard(score.ToString());
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } 
            else if(Input.GetKey(KeyCode.Q)){
                usermanager.OnButtonSentLeaderboard(score.ToString());
                SceneManager.LoadScene("Menu 1");
            }
        }
        if (gameOver) {
            restartText.text = "Press R to restart game";
            mainMenuText.text = "Press Q to go back to main menu";
            restart = true;
        }

        //InventoryManager.instance.GetVirtualCurrencies();
    }

    private IEnumerator spawnWaves(){
        yield return new WaitForSeconds(startWait);
        while(true){
            for (int i = 0; i < hazardCount;i++){
                Vector3 position = new Vector3(Random.Range(-positionAsteroid.x, positionAsteroid.x), positionAsteroid.y, positionAsteroid.z);
                Quaternion rotation = Quaternion.identity;
                Instantiate(asteroids[Random.Range(0,3)], position, rotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waitForWaves);
            if(gameOver){
                break;
            }
        }
    }

  
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenPaused()
    {
        PausedMenu.SetActive(true);
    }

    public void ClosePaused()
    {
        PausedMenu.SetActive(false);
    }


    public void gameIsOver(){
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void addScore(int score){
        this.score += score;
        InventoryManager.instance.GrantVirtualCurrency();
       
        updateScore();
    }

    void updateScore(){
        scoreText.text = "Score:" + score.ToString();
    }

}
