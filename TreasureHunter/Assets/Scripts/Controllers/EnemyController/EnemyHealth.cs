using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public const int max_health = 100;
	public int current_health;
	public bool is_dead = false;
	public UnityEngine.UI.Image healthbar;
	public GameObject canvas;

	public float healthBarLength;

	// Use this for initialization
	void Start () {
		current_health = max_health;
		healthBarLength = 1;
	}
	
	// Update is called once per frame
	public void updateHealthbar () {
		healthBarLength = 1*(current_health / (float)max_health);
		healthbar.rectTransform.sizeDelta = new Vector2 (healthBarLength, 0.2f);
	}

//	void OnGUI() {
//		if (view_health) {
//			GUI.Box (new Rect (10, 10, healthBarLength, 20), current_health + "/" + max_health);
//		}
//	}


	public void TakeDamage(int amount) {
		current_health -= amount;
		if (current_health <= 0)
			Die ();
	}

	public void Die() {
		is_dead = true;
		DestroyObject (canvas);

	}
}
