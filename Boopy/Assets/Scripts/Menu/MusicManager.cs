using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // 1. Clave para guardar el volumen en PlayerPrefs
    private const string VOLUME_KEY = "MusicVolume";
    
    public static MusicManager instance { get; private set; }
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.5f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            
            PlayerPrefs.SetFloat(VOLUME_KEY, volume);
            PlayerPrefs.Save(); 
        }
    }
}