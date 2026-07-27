using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void PlayerSFX(AudioClip clip , float volume = 1f , bool randomPitch = false)
    {
        if (clip == null) return;

        // đổi pitch ngẫu nhiên nếu randomPitch = true
        if (randomPitch)
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
        else
            sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(clip, volume);
        // trả pitch về 1 sau khi phát âm thanh
        sfxSource.pitch = 1f;

    }
}
