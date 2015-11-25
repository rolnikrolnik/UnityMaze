using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Interfaces;
using Treasure_Hunter.Managers;

public class SpikeTrapScript : MonoBehaviour 
{
    public PlayerController Player { get { return SceneManager.Instance.MazeManager.Player; } }

    public AudioClip SpikesOutAudioClip;

    public AudioClip SpikesInAudioClip;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(0.05f);
            Debug.Log("Stepped into a spike trap!");
        }
    }

    public void PlaySpikesInSound()
    {
        _audioSource.PlayOneShot(SpikesInAudioClip);
    }

    public void PlaySpikesOutSound()
    {
        _audioSource.PlayOneShot(SpikesOutAudioClip);
    }
}
