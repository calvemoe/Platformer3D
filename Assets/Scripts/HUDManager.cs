using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Text scoreLabel;

	// Use this for initialization
	void Start () {
        ResetHud();
	}

    public void ResetHud()
    {
        scoreLabel.text = "Score " + GameManager.instance.score;
    }
}
