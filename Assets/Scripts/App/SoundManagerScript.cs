using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public List<AudioClip> playlist;
    private AudioSource audioSource;
    private int currentSongIndex = 0;
    public float switchInterval = 30f; // Time interval to switch songs in seconds
    public float fadeDuration = 3f; // Fade duration in seconds

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

    // Function to play the next song in the playlist
    private void PlayNextSong()
    {
        // Check if there are songs in the playlist
        if (playlist.Count > 0)
        {
            StartCoroutine(FadeOutAndIn());
        }
        else
        {
            Debug.LogError("Playlist is empty. Add audio clips to the playlist.");
        }
    }

    // Coroutine to fade out the current song and fade in the next one
    private IEnumerator FadeOutAndIn()
    {
        float startTime = Time.time;

        // Fade out the current song
        while (Time.time < startTime + fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0.7f, 0f, (Time.time - startTime) / fadeDuration);
            yield return null;
        }

        // Increment the index
        currentSongIndex = (currentSongIndex + 1) % playlist.Count;

        // Set the next song
        audioSource.clip = playlist[currentSongIndex];

        startTime = Time.time;

        // Play the next song
        audioSource.Play();
        // Fade in the next song
        while (Time.time < startTime + fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 0.7f, (Time.time - startTime) / fadeDuration);
            yield return null;
        }


    }

    // Function to play the "barMusic" audio clip
    public void PlayBarMusic()
    {
        if (audioSource != null && playlist.Count > 0)
        {
            // Set the initial song
            audioSource.clip = playlist[currentSongIndex];
            audioSource.Play();

            // Start a coroutine to switch songs at regular intervals
            StartCoroutine(SwitchSongs());
        }
        else
        {
            Debug.LogError("AudioSource or playlist is not set in the SoundManagerScript.");
        }
    }

    // Coroutine to switch songs at regular intervals
    private IEnumerator SwitchSongs()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);
            PlayNextSong();
        }
    }
}
