using UnityEngine;
using System;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Interfaces;
using Treasure_Hunter.Enumerations;
using Treasure_Hunter.Managers;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region CLASS SETTINGS

    private const float SECONDS_TO_HIDE_HEALTHBAR = 5.0f;

    #endregion

    #region SCENE REFERENCES

    public HealthBar Healthbar;
    public GameObject EnemyRoot;
    public GameObject ProgressBarCanvas;
    public AudioClip DinosaurGettingHitAudioClip;

    #endregion

    public MonsterType Type;
	public bool is_dead = false;

    private AudioSource _audioSource;

    private int _seconds;

    private float current_health = 1;
    private float timer = 0;
    private bool healthShown = false;

    #region MONO BEHAVIOUR

    private void Start () 
    {
        current_health = 1;
        healthShown = false;
        Healthbar.SetValue(current_health);
        _audioSource = GetComponent<AudioSource>();
        _seconds = DateTime.Now.Second;
        ProgressBarCanvas.SetActive(false);
	}

    private void Update()
    {
        if(healthShown)
        {
            timer += Time.deltaTime;
            if(timer>=SECONDS_TO_HIDE_HEALTHBAR)
            {
                healthShown = false;
                ProgressBarCanvas.SetActive(false);
            }
        }
    }

    #endregion

    public void TakeDamage(float amount) 
    {
        if (current_health > 0)
        {
            healthShown = true;
            timer = 0;
            ProgressBarCanvas.SetActive(true);
            PlayGettingHitSound();
            current_health -= amount;
            Healthbar.SetValue(current_health);
            if (current_health <= 0)
            {
                Die();
            }
        }
	}

	public void Die() 
    {
		is_dead = true;
        Healthbar.gameObject.SetActive(false);
        PlayerPrefsManager.Instance.Achievements.AddKilledMonster(Type);
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
