﻿using UnityEngine;
using System.Collections;

public class SpikeTrapScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped into a spike trap!");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
