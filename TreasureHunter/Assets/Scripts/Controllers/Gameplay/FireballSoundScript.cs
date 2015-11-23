using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;

public class FireballSoundScript : MonoBehaviour {

    public PlayerController Player;

    public AudioClip FireballAudioClip;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFireballSound()
    {
        var distance = Vector3.Distance(transform.position, Player.transform.position);
        var scale = 1.0F - 0.03F * distance;
        if (scale < 0.0F)
        {
            scale = 0.0F;
        }

        _audioSource.PlayOneShot(FireballAudioClip, scale);
    }
}
