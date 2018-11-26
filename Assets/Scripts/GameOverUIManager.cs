﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour {

    public Text scoreValue;
    public Text highScoreValue;

	// Use this for initialization
	void Start () {
        scoreValue.text = GameManager.instance.score.ToString();
        highScoreValue.text = GameManager.instance.highScore.ToString();
	}
	
	public void RestartGame()
    {
        print("restartGAme");
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("Level1");
    }
}
