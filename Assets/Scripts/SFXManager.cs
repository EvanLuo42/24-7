using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [System.Serializable]
    public class SfxEntry
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
        public bool randomPitch;
    }

    public List<SfxEntry> sfxList = new();
    private readonly Dictionary<string, SfxEntry> _sfxDict = new();

    [Header("Audio Source Pool")]
    public int poolSize = 10;
    private readonly List<AudioSource> _audioSources = new();

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        foreach (var entry in sfxList)
        {
            _sfxDict[entry.name] = entry;
        }
        
        for (var i = 0; i < poolSize; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            _audioSources.Add(source);
        }
    }

    public void Play(string sfxName)
    {
        if (!_sfxDict.TryGetValue(sfxName, out var entry) || entry.clip == null)
        {
            Debug.LogWarning($"SFX '{sfxName}' not found!");
            return;
        }

        var source = GetAvailableSource();
        source.clip = entry.clip;
        source.volume = entry.volume;
        source.pitch = entry.randomPitch ? Random.Range(0.9f, 1.1f) : 1f;
        source.Play();
    }

    private AudioSource GetAvailableSource()
    {
        foreach (var s in _audioSources.Where(s => !s.isPlaying))
        {
            return s;
        }

        return _audioSources[0];
    }
}