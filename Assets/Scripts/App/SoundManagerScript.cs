using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public AudioClip barMusic;
    static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        // Create an AudioSource component if it doesn't exist
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Play the audio clip
        PlayBarMusic();
    }

    // Function to play the "barMusic" audio clip
    public void PlayBarMusic()
    {
        if (audioSource != null && barMusic != null)
        {
            audioSource.clip = barMusic;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or barMusic clip is not set in the SoundManagerScript.");
        }
    }
}