using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstManager : MonoBehaviour
{
    public static OstManager Instance { get; private set; }

    [System.Serializable]
    public class OstEntry
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
    }

    public List<OstEntry> tracks = new();
    private readonly Dictionary<string, OstEntry> _trackDict = new();

    private AudioSource _audioSource;
    private Coroutine _currentCoroutine;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;

        foreach (var track in tracks)
        {
            _trackDict.TryAdd(track.name, track);
        }
    }

    public void Play(string ostName)
    {
        if (!_trackDict.TryGetValue(ostName, out var newTrack))
        {
            Debug.LogWarning($"OST '{ostName}' not found.");
            return;
        }

        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        _currentCoroutine = StartCoroutine(FadeAndPlay(newTrack));
    }

    public void Stop()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        _currentCoroutine = StartCoroutine(FadeOutAndStop());
    }

    private IEnumerator FadeAndPlay(OstEntry newTrack)
    {
        if (_audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut());
        }
        
        _audioSource.clip = newTrack.clip;
        _audioSource.volume = 0f;
        _audioSource.Play();
        
        yield return StartCoroutine(FadeIn(newTrack.volume));
    }

    private IEnumerator FadeIn(float targetVolume)
    {
        var timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            _audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            yield return null;
        }
        _audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut()
    {
        var startVolume = _audioSource.volume;
        var timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }
        _audioSource.volume = 0f;
        _audioSource.Stop();
    }

    private IEnumerator FadeOutAndStop()
    {
        yield return StartCoroutine(FadeOut());
        _audioSource.clip = null;
    }
}