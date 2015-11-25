using UnityEngine;
using System.Collections;
using System;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Interfaces;

public class PlayerAttack : MonoBehaviour, IDamageable
{

    Collider[] colliders;
    public HealthBar OVRHealthBar;
    public HealthBar StandaloneBar;
    public PlayerController Player;

	public float attackRange = 4f;
    [HideInInspector]
    public float currentHealth;

    private AudioSource _playerAudioSource;
    private int _seconds;

	void Start () 
    {
        currentHealth = 1;
        UpdateProgressBars();
        _playerAudioSource = Player.GetComponent<AudioSource>();

        _seconds = DateTime.Now.Second;
	}

	void Update () 
    {
        if (Player.Attack)
        {
            colliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (Collider col in colliders)
            {
                EnemyHealth eh = col.GetComponent<EnemyHealth>();
                if (eh!= null)
                {
                    if (!eh.is_dead)
                    {
                        var relativePoint = transform.InverseTransformPoint(col.transform.position);
                        if (relativePoint.x <= 0.5 && relativePoint.x >= -0.5)
                        {
                            eh.TakeDamage(0.1f);
                        }
                    }

                }
            }
        }
	}

	public void TakeDamage(float amount) 
    {
        PlayOuchSound();
		currentHealth -= amount;
        UpdateProgressBars();
	}

    private void PlayOuchSound()
    {
        var secondsNow = DateTime.Now.Second;

        if (Math.Abs(_seconds - secondsNow) >= 2)
        {
            _playerAudioSource.PlayOneShot(Player.OuchAudioClip);
            _seconds = secondsNow;
        }
    }

    private void UpdateProgressBars()
    {
        if (OVRHealthBar != null)
        {
            OVRHealthBar.SetValue(currentHealth);
        }
        if (StandaloneBar != null)
        {
            StandaloneBar.SetValue(currentHealth);
        }
    }
}

