using UnityEngine;
using System.Collections;

public class MoveStates : MonoBehaviour {
	
	public enum MoveState { idle, aggressive_idle, running, attack };
	
	private Transform target;
	public int move_speed = 5;
	public int rotation_speed = 2;
	public float sight_range = 20f;
	public float danger_range = 15f;
	public float attack_range = 2f;
	private Transform dino_transform;
	public int moving_state = (int)MoveState.idle;
	private EnemyHealth enemy_health;

	Vector3 _vec;
	Vector3 _tmp;
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag ("Player").transform;
		enemy_health = gameObject.GetComponent<EnemyHealth> ();
		dino_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!enemy_health.is_dead) {
			_vec = target.position;
			_tmp = new Vector3 ();
			
			var _distance = Vector3.Distance (target.position, dino_transform.position);
			
			//Jeśli gracz jest za daleko, to sobie po prostu siedź jakby nigdy nic
			if (_distance > sight_range) {
				moving_state = (int)MoveState.idle;
			}
			// Jeśli gracz podejdzie za blisko, to uruchamiamy gotowość do ataku
			else if (_distance <= sight_range && _distance > danger_range) {
				
				dino_transform.rotation = Quaternion.Slerp (dino_transform.rotation, Quaternion.LookRotation (target.position - dino_transform.position), rotation_speed * Time.deltaTime);
				moving_state = (int)MoveState.aggressive_idle;
			}
			// Gracz się zbliża, dinuś czuje się zagrożony i biegnie aby go zjeść
			else if (_distance <= danger_range && _distance > attack_range) {
				dino_transform.rotation = Quaternion.Slerp (dino_transform.rotation, Quaternion.LookRotation (target.position - dino_transform.position), rotation_speed * Time.deltaTime);
				dino_transform.position += dino_transform.forward * move_speed * Time.deltaTime;
				_tmp = dino_transform.position;
				//_tmp.y = 0.1f;
				dino_transform.position = _tmp;
				moving_state = (int)MoveState.running;
			}
			// Dino jest wystarczająco blisko żeby rozszarpać gracza
			else if (_distance < attack_range) {
				moving_state = (int)MoveState.attack;
			}
		}
	}
}
