using UnityEngine;
using System;
using System.Collections;
using Treasure_Hunter.Enumerations;


public class AnimateDino : MonoBehaviour 
{
    public AudioClip DinosaurAttackingAudioClip;
    public AudioClip DinosaurDyingAudioClip;

    private AudioSource _audioSource;

    private int _seconds;

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
        _audioSource = GetComponent<AudioSource>();
        _seconds = DateTime.Now.Second;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (!dino_health.is_dead) {
			if (dino_movement.movingState == MoveState.idle) {
                if (!animation.IsPlaying("Allosaurus_Idle") && !animation.IsPlaying("Allosaurus_Attack02"))
                {
					animation.Stop ();
					animation.Play ("Allosaurus_Idle");
				}
            }
            else if (dino_movement.movingState == MoveState.aggressive_idle && !animation.IsPlaying("Allosaurus_Attack02"))
            {
				if (!animation.IsPlaying ("Allosaurus_IdleAggressive")) {
					animation.Stop ();
					animation.Play ("Allosaurus_IdleAggressive");
                    _audioSource.PlayOneShot(DinosaurAttackingAudioClip);
				}
            }
            else if (dino_movement.movingState == MoveState.running)
            {
				if (!animation.IsPlaying ("Allosaurus_Run")&&!animation.IsPlaying ("Allosaurus_Attack02")) 
                {
					animation.Stop ();
					animation.Play ("Allosaurus_Run");
				}
            }
            else if (dino_movement.movingState == MoveState.attack)
            {
				if (!animation.IsPlaying ("Allosaurus_Attack02")) 
                {
					animation.Stop ();
					animation.Play ("Allosaurus_Attack02");
                    _audioSource.PlayOneShot(DinosaurAttackingAudioClip);
				}
			}
		} else if (died == false) {
			animation.Play ("Allosaurus_Die");
            _audioSource.PlayOneShot(DinosaurDyingAudioClip);
			died = true;
		}
	}
}
