using UnityEngine;
using System.Collections;

public class CleanPlayerPrefs : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
