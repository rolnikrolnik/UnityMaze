using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Managers;

public class FireballSoundScript : MonoBehaviour {

    public PlayerController Player { get { return SceneManager.Instance.MazeManager.Player; } }

    public AudioClip FireballAudioClip;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFireballSound()
    {
        _audioSource.PlayOneShot(FireballAudioClip);
    }
}
