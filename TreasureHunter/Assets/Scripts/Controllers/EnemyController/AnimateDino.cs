using UnityEngine;
using System.Collections;

public class AnimateDino : MonoBehaviour {
	private Animation animation;
	private GameObject player;
	private MoveStates dino_movement;
	private EnemyHealth dino_health;
	bool died = false;
	
	// Use this for initialization
	void Start () {
		animation = GetComponent<Animation>();
		dino_movement = gameObject.GetComponent<MoveStates> ();
		dino_health = gameObject.GetComponent<EnemyHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!dino_health.is_dead) {
			if (dino_movement.moving_state == (int)MoveStates.MoveState.idle) {
				if (!animation.IsPlaying ("Allosaurus_Idle")) {
					animation.Stop ();
					animation.Play ("Allosaurus_Idle");
				}
			} else if (dino_movement.moving_state == (int)MoveStates.MoveState.aggressive_idle) {
				if (!animation.IsPlaying ("Allosaurus_IdleAggressive")) {
					animation.Stop ();
					animation.Play ("Allosaurus_IdleAggressive");
				}
			} else if (dino_movement.moving_state == (int)MoveStates.MoveState.running) {
				if (!animation.IsPlaying ("Allosaurus_Run")) {
					animation.Stop ();
					animation.Play ("Allosaurus_Run");
				}
			} else if (dino_movement.moving_state == (int)MoveStates.MoveState.attack) {
				if (!animation.IsPlaying ("Allosaurus_Attack02")) {
					animation.Stop ();
					animation.Play ("Allosaurus_Attack02");
				}
			}
		} else if (died == false) {
			animation.Play ("Allosaurus_Die");
			died = true;
		}
	}
}
