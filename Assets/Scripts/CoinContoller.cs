using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContoller : MonoBehaviour {

    public float rotationSpeed = 100;
	
	// Update is called once per frame
	void Update () {
        float angleRotation = rotationSpeed + Time.deltaTime;
        transform.Rotate(Vector3.up * angleRotation, Space.World);
	}
}
