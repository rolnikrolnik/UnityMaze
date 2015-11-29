using UnityEngine;
using System.Collections;
using System;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Interfaces;
using Treasure_Hunter.Managers;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour, IDamageable
{
    public HealthBar OVRHealthBar;
    public HealthBar StandaloneBar;
    public PlayerController Player;
    public Transform AttackLimit;
    public Transform AttackPosition;

    [HideInInspector]
    public float currentHealth;

    private AudioSource _playerAudioSource;
    private int _seconds;
    private bool dieAnimationFinished;

	void Start () 
    {
        currentHealth = 1;
        UpdateProgressBars();
        _playerAudioSource = Player.GetComponent<AudioSource>();

        _seconds = DateTime.Now.Second;
	}

	public void MakeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(AttackPosition.position, Vector3.Distance(transform.position, AttackLimit.position));
        List<IDamageable> hittedMonsters = new List<IDamageable>();
        foreach (Collider col in colliders)
        {
            EnemyHealth eh = col.GetComponentInParent<EnemyHealth>();
            if (eh != null)
            {
                if (!hittedMonsters.Exists(id=>id==eh))
                {
                    hittedMonsters.Add(eh);
                    eh.TakeDamage(0.4f);
                }
            }
        }
    }

	public void TakeDamage(float amount) 
    {
        if (Player.IsEnabled && currentHealth>0)
        {
            PlayOuchSound();
            currentHealth -= amount;
            UpdateProgressBars();
            if (currentHealth <= 0)
            {
                Die();
            }
        }
	}

    private void Die()
    {
        dieAnimationFinished = false;
        Player.DisablePlayer();
        Player.Animator.SetTrigger("Die");
        PlayerPrefsManager.Instance.Achievements.AddLostMaze(SceneManager.Instance.MazeManager.MazeType);
    }

    private void EndOFDieAnimation()
    {
        if (!dieAnimationFinished)
        {
            SceneManager.Instance.BackToBase();
            dieAnimationFinished = true;
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + Vector3.up * 4, Vector3.Distance(AttackLimit.position, transform.position));
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(AttackPosition.position, Vector3.Distance(transform.position, AttackLimit.position));
    }
}

