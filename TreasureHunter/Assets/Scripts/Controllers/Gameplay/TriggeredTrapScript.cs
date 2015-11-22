using UnityEngine;
using System.Collections;
using UnityEditor;

public class TriggeredTrapScript : MonoBehaviour {

    public GameObject FirePrefab;

	// Use this for initialization
	void Start () {
        FirePrefab.SetActive(false);
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped on a pressure plate!");
        Debug.Log("Activated the trap!");

        FirePrefab.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        StartCoroutine(Wait());
    }

	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Wait()
    {
        Debug.Log("Waiting for 5 seconds...");
        yield return new WaitForSeconds(5.0F);
        FirePrefab.SetActive(false);
    }
}
