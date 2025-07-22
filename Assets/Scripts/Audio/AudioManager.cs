using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSourcePrefab;
    [SerializeField] private int seChannelCount = 5;

    private List<AudioSource> seChannels = new();

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
        InitializeSeChannels();
        LoadSavedVolume();
    }

    private void InitializeDictionaries()
    {
        bgmDict.Clear();
        seDict.Clear();

        foreach (var clip in bgmClips)
            if (clip != null && !string.IsNullOrEmpty(clip.audioId))
                bgmDict[clip.audioId] = clip;

        foreach (var clip in seClips)
            if (clip != null && !string.IsNullOrEmpty(clip.audioId))
                seDict[clip.audioId] = clip;
    }

    private void InitializeSeChannels()
    {
        for (int i = 0; i < seChannelCount; i++)
        {
            AudioSource newSource = Instantiate(seSourcePrefab, transform);
            newSource.playOnAwake = false;
            newSource.loop = false;
            seChannels.Add(newSource);
        }
    }

    #region 재생 기능

    public void PlayBGM(string id, bool loop = true)
    {
        if (bgmDict.TryGetValue(id, out var data))
        {
            if (bgmSource.clip == data.clip && bgmSource.isPlaying) return;

            bgmSource.clip = data.clip;
            bgmSource.volume = data.volume * BgmVolume;
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
        if (!seDict.TryGetValue(id, out var data))
        {
            Debug.LogWarning($"[AudioManager] SE ID '{id}' not found.");
            return;
        }

        foreach (var channel in seChannels)
        {
            if (!channel.isPlaying)
            {
                channel.volume = data.volume * SeVolume;
                channel.PlayOneShot(data.clip);
                return;
            }
        }

        Debug.LogWarning("[AudioManager] 모든 SE 채널이 사용 중입니다.");
    }

    #endregion

    #region  볼륨 제어 및 저장

    public float BgmVolume => PlayerPrefs.GetFloat("BGM_VOLUME", 0.8f);
    public float SeVolume => PlayerPrefs.GetFloat("SE_VOLUME", 1.0f);

    public void SetBgmVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("BGM_VOLUME", volume);
        bgmSource.volume = volume;
    }

    public void SetSeVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SE_VOLUME", volume);

        foreach (var channel in seChannels)
            channel.volume = volume;
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.Save();
    }

    private void LoadSavedVolume()
    {
        float savedBgm = BgmVolume;
        float savedSe = SeVolume;

        bgmSource.volume = savedBgm;
        foreach (var channel in seChannels)
            channel.volume = savedSe;
    }

    #endregion

    #region  수동 등록

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

    #endregion
}