using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void PlaySFXRequest(string name)
    {
        var audio = Resources.Load<AudioClip>("SFX/" + name);
        if (audio == null)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(audio, Camera.main.transform.position);
    }

    public void PlayMusicRequest(string name)
    {
        var audio = Resources.Load<AudioClip>("Music/" + name);
        if (audio == null)
        {
            return;
        }
        if (_audioSource != audio)
        {
            _audioSource.clip = audio;
            _audioSource.Play();
        }
    }

}
