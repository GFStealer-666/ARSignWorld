using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class PageAudioController : MonoBehaviour
{
    // This script manages audio playback for pages in the UI.
    // It allows setting up audio from a specified path, toggling playback, and updating UI elements accordingly.

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource bgsound;
    [SerializeField] private Button audioToggleButton;
    [SerializeField] private Image audioIcon;
    [SerializeField] private Sprite playSprite, pauseSprite;
    
    private bool isPaused = true;
    private bool isInitialized = false;

    private void InitializeAudioSource()
    {
        // if (!isInitialized && audioSource != null)
        // {

        //     audioSource.Stop();
        //     isInitialized = true;
        //     Debug.Log("Audio Source initialized");
        // }
    }

    public void SetupAudio(string audioPath, bool startPaused = true)
    {
        InitializeAudioSource();
        if (string.IsNullOrEmpty(audioPath)) return;

        // Remove file extension if present
        string resourcePath = "Description_Sound/" + Path.GetFileNameWithoutExtension(audioPath);
        
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);
        if (clip == null)
        {
            Debug.LogWarning($"Could not load audio clip from Resources: {resourcePath}");
            return;
        }

        audioSource.clip = clip;
        isPaused = startPaused;

        audioSource.Stop();
    
        if (!startPaused)
        {
            audioSource.Play();
        }

        UpdateAudioUI();
    }

    public void ToggleAudio()
    {
        isPaused = !isPaused;
        Debug.Log($"Toggle Audio - isPaused: {isPaused}"); // Debug log

        if (isPaused)
        {
            bgsound.Play();
            audioSource.Pause();
        }
        else
        {
            if (!audioSource.isPlaying) // If not playing at all, start from beginning
            {
                bgsound.Pause();
                audioSource.Play();
            }
            else // Otherwise unpause
            {
                audioSource.UnPause();
            }
            Debug.Log($"Playing audio: {audioSource.clip?.name}, isPlaying: {audioSource.isPlaying}"); // Debug log
        }
        UpdateAudioUI();
    }

    private void UpdateAudioUI()
    {
        if (audioIcon != null)
        {
            audioIcon.sprite = isPaused ? playSprite : pauseSprite;
        }
    }
    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isPaused = true;
            UpdateAudioUI();
        }
        bgsound.UnPause();
    }
}
