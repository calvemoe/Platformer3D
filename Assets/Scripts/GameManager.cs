using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int score = 0;

    public int highScore = 0;

    //current level
    public int currentLevel = 1;

    //how many levels in the game
    public int highestLevel = 2;

    //HUD manager
    HUDManager hudManager;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            instance.hudManager = FindObjectOfType<HUDManager>();
            Destroy(gameObject);
        }

        //don't destroy object
        DontDestroyOnLoad(gameObject);

        hudManager = FindObjectOfType<HUDManager>();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;

        //updating HUD
        if (hudManager != null)
            hudManager.ResetHud();
        else
            print("no hud manager!");
        print("After REsetHud. Score should be: " + score);

        if (score > highScore)
        {
            highScore = score;
        }
    }

    //Game Reset
    public void ResetGame()
    {
        score = 0;
        if (hudManager != null)
            hudManager.ResetHud();
        else
            print("no hud manager!");
        currentLevel = 1;

        //load Level1 Scene
        SceneManager.LoadScene("Level1");
    }

    public void IncreaseLevel()
    {
        //if there level further
        if (currentLevel < highestLevel)
        {
            currentLevel++;
        }
        else
        {
            //if not - looping the game
            currentLevel = 1;
        }

        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}

