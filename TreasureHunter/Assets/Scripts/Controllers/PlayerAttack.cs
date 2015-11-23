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
    private float _timeSinceLastOuch;

	void Start () 
    {
        currentHealth = 1;
        UpdateProgressBars();
        _playerAudioSource = Player.GetComponent<AudioSource>();
        _timeSinceLastOuch = 0.0F;
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
        if (_timeSinceLastOuch == 0.0F)
        {
            _playerAudioSource.PlayOneShot(Player.OuchAudioClip);
        }

        _timeSinceLastOuch += Time.deltaTime;

        if (_timeSinceLastOuch >= 1.5F)
        {
            _timeSinceLastOuch = 0.0F;
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

