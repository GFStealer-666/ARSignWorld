using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class QuizVideoController : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public AudioSource audioSource; // Optional, if you're using external audio

    [Header("UI")]
    public Button pausePlayButton;
    public Image pausePlayIcon;
    public Sprite playSprite;
    public Sprite pauseSprite;

    private void Start()
    {
        videoPlayer.started += OnVideoStarted;
    }

    private void OnDestroy()
    {
        videoPlayer.started -= OnVideoStarted;
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
        UpdateIcon();
    }

    public void TogglePausePlay()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            audioSource?.Pause();
        }
        else
        {
            videoPlayer.Play();
            if (!audioSource?.mute ?? true) // play only if not muted
                audioSource?.Play();
        }

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        pausePlayIcon.sprite = videoPlayer.isPlaying ? pauseSprite : playSprite;
    }
}
