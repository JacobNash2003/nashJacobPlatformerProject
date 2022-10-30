using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float Timer;
    public float FTimer;
    public int Lives = 3;
    public TMP_Text TextBoxT;
    public TMP_Text TextBoxL;
    public TMP_Text TextBoxFT;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject LoseLifeScreen;
    public GameObject SpawnLocation;
    public GameObject Character;
    public GameObject Darkness;

    // Start is called before the first frame update
    void Start()
    {
        Timer = 0f;
        FTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKey(KeyCode.R))
        {
            Character.transform.position = SpawnLocation.transform.position;
            LoseLifeScreen.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        StopWatch();
        StoppedTime();
    }

    public void StopWatch()
    {
        Timer = Timer + Time.deltaTime;
        TextBoxT.text = "Timer: " + Timer;
    }
    public void UpdateLives()
    {
        Lives--;
        TextBoxL.text = "Lives: " + Lives;
        ResetGame();
        if (Lives == 0)
        {
            LoseGame();
            LoseLifeScreen.SetActive(false);
        }    
    }

    public void IncreaseLife()
    {
        Lives++;
        TextBoxL.text = "Lives: " + Lives;
    }

    public void DecreaseLife()
    {
        Lives -= 2;
        TextBoxL.text = "Lives: " + Lives;
        ResetGame();
    }

    public void WinGame()
    {
        WinScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void LoseGame()
    {
        LoseScreen.SetActive(true);
        Destroy(TextBoxT);
    }

    public void ResetGame()
    {
        LoseLifeScreen.SetActive(true);
    }

    public void StoppedTime()
    {
        FTimer = FTimer + Time.deltaTime;
        TextBoxFT.text = "FinalTime: " + FTimer;
    }
}