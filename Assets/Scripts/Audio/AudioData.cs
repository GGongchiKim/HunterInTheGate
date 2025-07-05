using UnityEngine;

[System.Serializable]
public class AudioData
{
    [Tooltip("오디오를 구분할 ID (예: 'MainBGM', 'ButtonClick')")]
    public string audioId;

    [Tooltip("재생할 오디오 클립")]
    public AudioClip clip;

    [Range(0f, 1f)]
    [Tooltip("재생 볼륨 (0.0 ~ 1.0)")]
    public float volume = 1f;
}