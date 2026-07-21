using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---- Audio Sources ----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("---- Audio Clips ----")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip damageSound;
    public AudioClip coinSound;
    public AudioClip dieSound;

    [Header("---- Music Loop Settings ----")]
    [Tooltip("Custom loop length in seconds. Keep this in sync with your music clip's intended loop point.")]
    [SerializeField] private float loopDuration = 30f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    private void Update()
    {
        // Check if the music source is playing
        if (musicSource != null && musicSource.isPlaying)
        {
            // If the current playback time passes the loop point, start it from start
            if (musicSource.time >= loopDuration)
            {
                musicSource.time = 0f;
            }
        }
    }

    
    public void PlayMusic()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;

            musicSource.loop = false;

            musicSource.time = 0f;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}