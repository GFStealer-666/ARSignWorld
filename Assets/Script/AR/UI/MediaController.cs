using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MediaController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;

    [Header("UI Controls")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Image soundIcon;
    [SerializeField] private Image videoIcon;
    [SerializeField] private Sprite soundOnSprite, soundOffSprite;
    [SerializeField] private Sprite playSprite, pauseSprite;

    private bool isManuallyMuted;

    private void Start()
    {
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        SetControlsInteractable(false);
    }

    public void SetVideo(string path)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;

        videoIcon.sprite = playSprite;
        soundIcon.sprite = soundOnSprite;
        isManuallyMuted = false;
        audioSource.mute = false;

        videoButton.interactable = true;
        soundButton.interactable = false;

        // Disabled the sound button before the video starts playing
        
        videoPlayer.Pause();
    }

    public void ToggleSound()
    {
        isManuallyMuted = !isManuallyMuted;
        audioSource.mute = isManuallyMuted;
        soundIcon.sprite = isManuallyMuted ? soundOffSprite : soundOnSprite;
    }

    public void ToggleVideo()
    {
        if (videoPlayer.isPlaying)
            PauseMedia();
        else
            PlayMedia();
    }

    private void PlayMedia()
    {
        videoPlayer.Play();
        audioSource.Play();
        videoIcon.sprite = pauseSprite;
        soundButton.interactable = true;
    }

    private void PauseMedia()
    {
        videoPlayer.Pause();
        audioSource.Pause();
        videoIcon.sprite = playSprite;
        soundButton.interactable = false;
    }

    public void StopMedia()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.targetTexture?.Release();
        }
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        videoIcon.sprite = playSprite;
        soundIcon.sprite = soundOnSprite;
        isManuallyMuted = false;
        
        SetControlsInteractable(false);
    }

    public void SetControlsInteractable(bool interactable)
    {
        if (soundButton != null)
            soundButton.interactable = interactable;
        if (videoButton != null)
            videoButton.interactable = interactable;
    }
}