using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public float attack_range = 4f;
	Collider[] colliders;
	float view_time;

	bool healthbar_exists = false;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		colliders = Physics.OverlapSphere (transform.position, attack_range);
		foreach (Collider col in colliders) {
			if (col && col.tag == "Monster") {
				EnemyHealth eh = col.GetComponent<EnemyHealth> ();
				if (!eh.is_dead) {
					var relativePoint = transform.InverseTransformPoint (col.transform.position);
					if (relativePoint.x <= 0.5 && relativePoint.x >= -0.5) {
						if (Input.GetMouseButtonDown(0)) {
							eh.TakeDamage (5);
							eh.updateHealthbar();
						}
					}
				}

			}
		}	

		var all_enemies = GameObject.FindGameObjectsWithTag ("Monster");


		foreach (GameObject enemy in all_enemies) {
			EnemyHealth eh = enemy.GetComponent<EnemyHealth> ();
			var relativePoint = transform.InverseTransformPoint (enemy.transform.position);
		}

	}
}

