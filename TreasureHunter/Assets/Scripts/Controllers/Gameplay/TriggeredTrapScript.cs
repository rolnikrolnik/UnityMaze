using UnityEngine;
using System.Collections;
using Treasure_Hunter.Interfaces;

public class TriggeredTrapScript : MonoBehaviour {

    public GameObject Fire;

    public AudioClip FireBurningAudioClip;

    public AudioClip PressurePlateStepAudioClip;

    private AudioSource _audioSource;

    private float _timeSinceLastFireBurning;

    private bool isSomeoneStayOnTrigger = false;
	// Use this for initialization
	void Start () {
        Fire.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _timeSinceLastFireBurning = 0.0F;
	}

    void OnTriggerEnter(Collider other)
    {
        _audioSource.PlayOneShot(PressurePlateStepAudioClip, 2.0F);
        isSomeoneStayOnTrigger = true;
        Debug.Log("Stepped on a pressure plate!");
        Debug.Log("Activated the trap!");
        Fire.SetActive(true);
        TakeDamageWithFire(other);
    }

    void OnTriggerStay(Collider other)
    {
        isSomeoneStayOnTrigger = true;
        TakeDamageWithFire(other);
    }

    void OnTriggerExit(Collider other)
    {
        isSomeoneStayOnTrigger = false;
        StartCoroutine(WaitAndMakeFireInactive());
        _timeSinceLastFireBurning = 0.0F;
    }

    void TakeDamageWithFire(Collider other)
    {
        PlayFireBurningSound();
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(0.2f * Time.deltaTime);
        }
    }

    IEnumerator WaitAndMakeFireInactive()
    {
        yield return new WaitForSeconds(5.0F);
        if (!isSomeoneStayOnTrigger)
        {
            Fire.SetActive(false);

            _audioSource.Stop();
            _timeSinceLastFireBurning = 0.0F;
        }
    }

    private void PlayFireBurningSound()
    {
        if (_timeSinceLastFireBurning == 0.0F)
        {
            _audioSource.PlayOneShot(FireBurningAudioClip);
        }

        _timeSinceLastFireBurning += Time.deltaTime;

        if (_timeSinceLastFireBurning >= 60.0F)
        {
            _timeSinceLastFireBurning = 0.0F;
        }
    }
}
