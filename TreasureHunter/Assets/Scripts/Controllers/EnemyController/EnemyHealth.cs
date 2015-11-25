using UnityEngine;
using System;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Interfaces;

public class EnemyHealth : MonoBehaviour, IDamageable
{

	public bool is_dead = false;
	public HealthBar Healthbar;
    public GameObject EnemyRoot;

    public AudioClip DinosaurGettingHitAudioClip;

    private AudioSource _audioSource;

    private int _seconds;

    private float current_health = 1;

	void Start () 
    {
        current_health = 1;
        Healthbar.SetValue(current_health);
        _audioSource = GetComponent<AudioSource>();
        _seconds = DateTime.Now.Second;
	}

	public void TakeDamage(float amount) 
    {
        PlayGettingHitSound();

        current_health -= amount;
        Healthbar.SetValue(current_health);
        if (current_health <= 0)
        {
            Die();
        }
	}

	public void Die() 
    {
		is_dead = true;
        Healthbar.gameObject.SetActive(false);
	}

    public void DestroyObject()
    {
        Destroy(EnemyRoot);
    }

    private void PlayGettingHitSound()
    {
        var secondsNow = DateTime.Now.Second;

        if (Math.Abs(_seconds - secondsNow) >= 1)
        {
            _audioSource.PlayOneShot(DinosaurGettingHitAudioClip);
            _seconds = secondsNow;
        }
    }
}
