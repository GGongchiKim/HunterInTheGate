using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("Registered BGM Clips")]
    [SerializeField] private List<AudioData> bgmClips;

    [Header("Registered SE Clips")]
    [SerializeField] private List<AudioData> seClips;

    private Dictionary<string, AudioData> bgmDict = new();
    private Dictionary<string, AudioData> seDict = new();

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        bgmDict.Clear();
        seDict.Clear();

        foreach (var clip in bgmClips)
        {
            if (clip != null && !string.IsNullOrEmpty(clip.audioId))
                bgmDict[clip.audioId] = clip;
        }

        foreach (var clip in seClips)
        {
            if (clip != null && !string.IsNullOrEmpty(clip.audioId))
                seDict[clip.audioId] = clip;
        }
    }

   
    public void PlayBGM(string id, bool loop = true)
    {
        if (bgmDict.TryGetValue(id, out var data))
        {
            if (bgmSource.clip == data.clip && bgmSource.isPlaying) return;

            bgmSource.clip = data.clip;
            bgmSource.volume = data.volume;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"[AudioManager] BGM ID '{id}' not found.");
        }
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    
    public void PlaySE(string id)
    {
        if (seDict.TryGetValue(id, out var data))
        {
            seSource.PlayOneShot(data.clip, data.volume);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] SE ID '{id}' not found.");
        }
    }

   
    public void RegisterBGM(AudioData data)
    {
        if (data != null && !string.IsNullOrEmpty(data.audioId))
            bgmDict[data.audioId] = data;
    }

    public void RegisterSE(AudioData data)
    {
        if (data != null && !string.IsNullOrEmpty(data.audioId))
            seDict[data.audioId] = data;
    }
}