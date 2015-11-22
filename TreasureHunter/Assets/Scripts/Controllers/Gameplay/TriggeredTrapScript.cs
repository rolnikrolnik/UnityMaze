using UnityEngine;
using System.Collections;
using UnityEditor;
using Treasure_Hunter.Interfaces;

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
        TakeDamageWithFire(other);
    }

    void OnTriggerStay(Collider other)
    {
        isSomeoneStayOnTrigger = true;
        TakeDamageWithFire(other);
    }

    void OnTriggerExit(Collider other)
    {
        isSomeoneStayOnTrigger = false;
        StartCoroutine(WaitAndMakeFireInactive());
    }

    void TakeDamageWithFire(Collider other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(0.05f * Time.deltaTime);
            Debug.Log("Took fire damage!");
        }
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
