using UnityEngine;
using System.Collections;

public class MoveStates : MonoBehaviour {
	
	public enum MoveState { idle, aggressive_idle, running, attack };
	
	private Transform target;
	public int moveSpeed = 5;
	public int rotationSpeed = 2;
	public float sightRange = 20f;
	public float dangerRange = 15f;
	public float attackRange = 2f;
	public float sightAngle = 35.0f;
	public float attackSpeed = 1.0f;
	public bool attackBlocked = false;
	public bool enemySpotted = false;
	public int movingState = (int)MoveState.idle;
	private EnemyHealth enemyHealth;
	public GameObject player;

	Vector3 sightVector;
	Color sightColor = Color.green;


	
	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag ("Player").transform;
		enemyHealth = gameObject.GetComponent<EnemyHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
		DrawSight ();

		if (!enemyHealth.is_dead) {
			
			var _distance = Vector3.Distance (target.position, transform.position);

			var angle = Vector3.Angle(transform.forward, target.position - transform.position);
			
			//Jeśli gracz jest za daleko, to sobie po prostu siedź jakby nigdy nic
			if (_distance > sightRange) {
				movingState = (int)MoveState.idle;
				sightColor = Color.green;
			}

			else if (angle < sightAngle && angle > -sightAngle) {
				enemySpotted = true;

				// Jeśli gracz podejdzie za blisko, to uruchamiamy gotowość do ataku
				if (_distance <= sightRange && _distance > dangerRange) {
					
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
					movingState = (int)MoveState.aggressive_idle;

					sightColor = Color.yellow;
				}
				// Gracz się zbliża, dinuś czuje się zagrożony i biegnie aby go zjeść
				else if (_distance <= dangerRange && _distance > attackRange) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
					transform.position += transform.forward * moveSpeed * Time.deltaTime;
					movingState = (int)MoveState.running;
					sightColor = Color.red;
				}
				// Dino jest wystarczająco blisko żeby rozszarpać gracza
				else if (_distance < attackRange) {
					movingState = (int)MoveState.attack;
					if (!attackBlocked){
						if (target.GetComponent<PlayerAttack>().currentHealth > 0)
							target.GetComponent<PlayerAttack> ().TakeDamage (10);
						StartCoroutine(AttackPause());
					}
				}
			}

			else if (enemySpotted) {
				movingState = (int)MoveState.aggressive_idle;
				sightColor = Color.yellow;
			}
		}
	}

	void DrawSight() {
		sightVector = Vector3.Scale (transform.forward, new Vector3 (sightRange, 0.0f, sightRange));
		sightVector = Quaternion.Euler (0, sightAngle, 0) * sightVector;
		Debug.DrawRay (transform.position, sightVector, sightColor, 0.01f);
		sightVector = Quaternion.Euler (0, -2*sightAngle, 0) * sightVector;
		Debug.DrawRay (transform.position, sightVector, sightColor, 0.01f);
	}

	public IEnumerator AttackPause() {
		attackBlocked = true;
		yield return new WaitForSeconds(attackSpeed);
		attackBlocked = false;
		
	}
}
