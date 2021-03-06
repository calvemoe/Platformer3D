﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Vector3 initialPosition;

    //movement direction: 1 - down, -1 - up
    private int direction = 1;

    //range of movement by Y
    [SerializeField]
    private float rangeY = 4f;

    //enemy movement speed
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float speedDownMultiplier = 1.2f;

	// Use this for initialization
	void Start () {
        //save initial position for enemy
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float factor = direction < 0 ? speedDownMultiplier : 1f;

        //move enemy by Y
        float movementY = factor * speed * Time.deltaTime * direction;

        float newY = transform.position.y + movementY;

        if (Mathf.Abs(newY - initialPosition.y) > rangeY || newY < initialPosition.y)
            direction *= -1;
        else
            transform.position += new Vector3(0, movementY, 0);
	}
}
