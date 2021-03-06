﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //score variable for ui
    private int score = 0;
    
    //high score value for ui
    private int highScore = 0;

    //current level
    private int currentLevel = 1;

    //how many levels in the game. Hardcoded on this stage
    private int highestLevel = 4;

    //HUD manager
    HUDManager hudManager;

    public static GameManager instance {get; private set;}

    void Awake() {
        if (instance == null)
            instance = this;
 
        else if (instance != this) {
            instance.hudManager = FindObjectOfType<HUDManager>();
            Destroy(gameObject);
        }

        //don't destroy object
        DontDestroyOnLoad(gameObject);

        hudManager = FindObjectOfType<HUDManager>();
    }

    public void IncreaseScore(int amount) {
        score += amount;

        //updating HUD
        if (hudManager != null)
            hudManager.ResetHud();
        else
            print("GameManager.IncreaseScore: no hud manager!");

        if (score > highScore)
            highScore = score;
    }

    //Game Reset
    public void ResetGame() {
        score = 0;
        if (hudManager != null)
            hudManager.ResetHud();
        else
            Debug.Log("GameManager.ResetGame: no hud manager!");

        currentLevel = 1;

        //load Level1 Scene
        SceneManager.LoadScene("Level1");
    }

    public void IncreaseLevel() {
        //if there level further
        if (currentLevel < highestLevel)
            currentLevel++;
        else {
            //if not - looping the game
            currentLevel = 1;
        }

        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    public string GetScore() {
        return score.ToString();
    }

    public string GetHighScore() {
        return highScore.ToString();
    }

}

