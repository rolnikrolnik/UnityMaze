using UnityEngine;
using System.Collections;

public class FireballTrapScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped into a fireball!");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
