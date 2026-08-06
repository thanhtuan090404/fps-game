using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Phát nhạc nền khi bắt đầu trò chơi
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic; // Gán clip nhạc nền vào AudioSource
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlaySFX(AudioClip clip , float volume = 1f , bool randomPitch = false)
    {
        if (clip == null) return;

        // đổi pitch ngẫu nhiên nếu randomPitch = true
        if (randomPitch)
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
        else
            sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(clip, volume);

    }
}
