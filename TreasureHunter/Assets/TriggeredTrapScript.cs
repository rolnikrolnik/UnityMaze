using UnityEngine;
using System.Collections;
using UnityEditor;

public class TriggeredTrapScript : MonoBehaviour {

    public Object FirePrefab;

	// Use this for initialization
	void Start () {
        FirePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/FireComplex.prefab", typeof(GameObject));
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped on a pressure plate!");
        Debug.Log("Activated the trap!");

        GameObject fire = Instantiate(FirePrefab, transform.position, transform.localRotation) as GameObject;
        Destroy(fire, 5.0F);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
