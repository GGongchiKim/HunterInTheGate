using UnityEngine;

[System.Serializable]
public class AudioData
{
    [Tooltip("������� ������ ID (��: 'MainBGM', 'ButtonClick')")]
    public string audioId;

    [Tooltip("����� ����� Ŭ��")]
    public AudioClip clip;

    [Range(0f, 1f)]
    [Tooltip("��� ���� (0.0 ~ 1.0)")]
    public float volume = 1f;
}