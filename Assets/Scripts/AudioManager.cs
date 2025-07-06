using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip backgroundClip;
    public AudioClip LoseClip;
    public AudioClip WinClip;
    public AudioClip ClickClip;

    [Header("Audio Sources")]
    private AudioSource backgroundSource;
    private AudioSource effectSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

       
        backgroundSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        
        if (backgroundClip != null)
        {
            backgroundSource.clip = backgroundClip;
            backgroundSource.loop = true;
            backgroundSource.Play();
        }
    }

    public void Lose()
    {
        if (LoseClip != null && effectSource != null)
        {
            effectSource.PlayOneShot(LoseClip);
        }
    }

    public void Win()
    {
        if (WinClip != null && effectSource != null)
        {
            effectSource.PlayOneShot(WinClip);
        }
    }

    public void Click()
    {
        if (ClickClip != null)
        {
            effectSource.PlayOneShot(ClickClip);
        }
    }

}