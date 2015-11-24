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
        var scale = GetVolumeScaleBasedOnDistance();

        _audioSource.PlayOneShot(SpikesInAudioClip, scale);
    }

    public void PlaySpikesOutSound()
    {
        var scale = GetVolumeScaleBasedOnDistance();

        _audioSource.PlayOneShot(SpikesOutAudioClip, scale);
    }

    private float GetVolumeScaleBasedOnDistance()
    {
        var distance = Vector3.Distance(transform.position, Player.transform.position);
        var scale = 1.0F - 0.03F * distance;
        if (scale < 0.0F)
        {
            scale = 0.0F;
        }

        return scale;
    }
}
