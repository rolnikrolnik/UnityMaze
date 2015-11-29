using UnityEngine;
using System.Collections;
using Treasure_Hunter.Managers;

public class MoveStates : MonoBehaviour {
	
	public enum MoveState { idle, aggressive_idle, running, attack };

	public int moveSpeed = 5;
	public int rotationSpeed = 2;
	public float sightRange = 20f;
	public float dangerRange = 15f;
	public float attackRange = 2f;
	public float sightAngle = 35.0f;
	public float attackSpeed = 1.0f;
	public float targetingTime = 5.0f;
	public float idleAfterRunTime = 5.0f;
	public bool attackBlocked = false;
	public bool enemySpotted = false;
	public int movingState = (int)MoveState.idle;
	public EnemyHealth enemyHealth;
    private GameObject player { get { return SceneManager.Instance.MazeManager.Player.gameObject; } }
    private Transform target { get { return SceneManager.Instance.MazeManager.Player.transform; } }
	private NavMeshPath navPath;
	public bool targetEnemy = false;
	public int currentPathCornerIndex = 0;
	public bool followingPath = false;
	private Vector3 startingPosition;
	private Quaternion startingRotation;
	public int pathLength;
	private bool first = true;

	Vector3 sightVector;
	Color sightColor = Color.green;

	void Start() {
		navPath = new NavMeshPath ();
	}
	
	// Update is called once per frame
	void Update () {

		if (first && target != null) {
			NavMesh.CalculatePath (transform.position, target.position, NavMesh.AllAreas, navPath);
			transform.rotation = Quaternion.LookRotation (transform.position - navPath.corners [1]);
			navPath.ClearCorners ();
			
			startingPosition = transform.position;
			startingRotation = transform.rotation;
			first = false;
		}

		DrawSight ();
		
		for (int i = 0; i < navPath.corners.Length - 1; i++) {
			Debug.DrawLine (navPath.corners [i], navPath.corners [i+1], Color.magenta, 0.01f, false);		
		}

		if (!enemyHealth.is_dead) {
			pathLength = navPath.corners.Length;
			
			var _distance = Vector3.Distance (target.position, transform.position);
			var angle = Vector3.Angle(-transform.forward, target.position - transform.position);
			
			//Jeśli gracz jest za daleko, to sobie po prostu siedź jakby nigdy nic
			if (_distance > sightRange) {
				movingState = (int)MoveState.idle;
				sightColor = Color.green;
				if (enemySpotted) { StartCoroutine(TargetEnemy()); followingPath = true; enemySpotted = false; }

				if (followingPath) {
					if (navPath.corners.Length-1 > currentPathCornerIndex) {
						transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (transform.position - navPath.corners[currentPathCornerIndex+1]), rotationSpeed * Time.deltaTime);
						transform.position += -transform.forward * moveSpeed * Time.deltaTime;
						movingState = (int)MoveState.running;
						
						if (Mathf.Abs(transform.position.x - navPath.corners[currentPathCornerIndex+1].x) <= 1.0f &&
						    Mathf.Abs(transform.position.z - navPath.corners[currentPathCornerIndex+1].z) <= 1.0f) 
							currentPathCornerIndex++;
					}
				}
			}

			else if (angle < sightAngle && angle > -sightAngle) {
				enemySpotted = true;
				if (followingPath) { StopCoroutine(TargetEnemy()); followingPath = false; targetEnemy = false; currentPathCornerIndex = 0;}
				
				// Jeśli gracz podejdzie za blisko, to uruchamiamy gotowość do ataku
				if (_distance <= sightRange && _distance > dangerRange) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (transform.position - target.position), rotationSpeed * Time.deltaTime);
					movingState = (int)MoveState.aggressive_idle;
					sightColor = Color.yellow;

				}
				// Gracz się zbliża, dinuś czuje się zagrożony i biegnie aby go zjeść
				else if (_distance <= dangerRange && _distance > attackRange) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (transform.position - target.position), rotationSpeed * Time.deltaTime);
					transform.position += -transform.forward * moveSpeed * Time.deltaTime;
					movingState = (int)MoveState.running;
					sightColor = Color.red;
				}

				// Dino jest wystarczająco blisko żeby rozszarpać gracza
				else if (_distance < attackRange) {
					movingState = (int)MoveState.attack;
					if (!attackBlocked){
						if (target.GetComponent<PlayerAttack>().currentHealth > 0)
							target.GetComponent<PlayerAttack> ().TakeDamage (0.025f);
						StartCoroutine(AttackPause());
					}
				}
			}

			else if (enemySpotted) {
				movingState = (int)MoveState.aggressive_idle;
				sightColor = Color.yellow;

			}

			if (targetEnemy) 
				NavMesh.CalculatePath (transform.position, target.position, NavMesh.AllAreas, navPath);

			if (navPath.corners.Length-1 == currentPathCornerIndex) {
				NavMesh.CalculatePath (transform.position, startingPosition, NavMesh.AllAreas, navPath);
				followingPath = true;
				currentPathCornerIndex = 0;

				if (Mathf.Abs(transform.position.x - startingPosition.x) <= 1.0f &&
				    Mathf.Abs(transform.position.z - startingPosition.z) <= 1.0f) {
					transform.rotation = startingRotation;
					navPath.ClearCorners();
					followingPath = false;
					currentPathCornerIndex = 0;
				}
			}
				

		}
	}

	void DrawSight() {
		sightVector = Vector3.Scale (-transform.forward, new Vector3 (sightRange, 0.0f, sightRange));
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

	public IEnumerator TargetEnemy() {
		targetEnemy = true;
		yield return new WaitForSeconds (targetingTime);
		targetEnemy = false;
	}
}
