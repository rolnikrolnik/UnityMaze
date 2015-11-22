using UnityEngine;
using System.Collections;
using System;

public class PlayerAttack : MonoBehaviour {

	public float attackRange = 4f;
	Collider[] colliders;
	float viewTime;
	public int currentHealth;
	public int maxHealth = 100;
	public UnityEngine.UI.Image healthbar;
	public UnityEngine.UI.Text healthText;
	public float healthBarLength;



	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		healthText.text = Convert.ToString (100 * (float)currentHealth / (float)maxHealth) + "%";
	}

	public void updateHealthbar () {
		healthBarLength = 100*((float)currentHealth / (float)maxHealth);
		healthbar.rectTransform.sizeDelta = new Vector2 (healthBarLength, 10);
		healthText.text = Convert.ToString (100 * (float)currentHealth / (float)maxHealth) + "%";

	}

	// Update is called once per frame
	void Update () {
		updateHealthbar ();
		colliders = Physics.OverlapSphere (transform.position, attackRange);
		foreach (Collider col in colliders) {
			if (col && col.tag == "Monster") {
				EnemyHealth eh = col.GetComponent<EnemyHealth> ();
				if (!eh.is_dead) {
					var relativePoint = transform.InverseTransformPoint (col.transform.position);
					if (relativePoint.x <= 0.5 && relativePoint.x >= -0.5) {
						if (Input.GetMouseButtonDown(0)) {
							eh.TakeDamage (5);
//							eh.updateHealthbar();
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

	public void TakeDamage(int amount) {
		currentHealth -= amount;
	}
}

