using UnityEngine;
using System.Collections;
using UnityEditor;

public class TriggeredTrapScript : MonoBehaviour {

    public GameObject Fire;

    private bool isSomeoneStayOnTrigger = false;
	// Use this for initialization
	void Start () {
        Fire.SetActive(false);
	}

    void OnTriggerEnter(Collider other)
    {
        isSomeoneStayOnTrigger = true;
        Debug.Log("Stepped on a pressure plate!");
        Debug.Log("Activated the trap!");

        Fire.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        isSomeoneStayOnTrigger = true;
        StartCoroutine(WaitAndTakeDamage());
    }

    void OnTriggerExit(Collider other)
    {
        isSomeoneStayOnTrigger = false;
        StartCoroutine(WaitAndMakeFireInactive());
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
        if (!isSomeoneStayOnTrigger)
        {
            Fire.SetActive(false);
            Debug.Log("Made a fire trap inactive!");
        }
    }
}
