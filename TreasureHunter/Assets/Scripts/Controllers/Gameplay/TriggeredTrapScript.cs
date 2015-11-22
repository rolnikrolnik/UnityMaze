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

    void OnTriggerStay(Collider other)
    {
        StartCoroutine(WaitAndTakeDamage());
    }

    void OnTriggerExit(Collider other)
    {
        StartCoroutine(WaitAndMakeFireInactive());
    }

	// Update is called once per frame
	void Update () {
	
	}

    void TakeDamageWithFire()
    {
        // TODO
    }

    IEnumerator WaitAndTakeDamage()
    {
        Debug.Log("Waiting for 1 second");
        yield return new WaitForSeconds(1.0F);
        TakeDamageWithFire();
        Debug.Log("Took damage!");
    }

    IEnumerator WaitAndMakeFireInactive()
    {
        Debug.Log("Waiting for 5 seconds...");
        yield return new WaitForSeconds(5.0F);
        FirePrefab.SetActive(false);
        Debug.Log("Made a fire trap inactive!");
    }
}
