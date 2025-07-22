using UnityEngine;
using UnityEngine.UI;

public class OptionPanelController : MonoBehaviour
{
    [Header("�����̴�")]
    public Slider bgmSlider;
    public Slider seSlider;

    [Header("UI Panel")]
    public GameObject optionPanel;

    private float originalBgmVolume;
    private float originalSeVolume;

    private bool isInitialized = false;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        InitializeUIFromAudioSettings();
    }

    private void InitializeUIFromAudioSettings()
    {
        // ���� �� ��� (Cancel ������)
        originalBgmVolume = AudioManager.Instance.BgmVolume;
        originalSeVolume = AudioManager.Instance.SeVolume;

        // �����̴� �ʱ�ȭ
        bgmSlider.SetValueWithoutNotify(originalBgmVolume);
        seSlider.SetValueWithoutNotify(originalSeVolume);

        // ������ �ߺ� ����
        bgmSlider.onValueChanged.RemoveAllListeners();
        seSlider.onValueChanged.RemoveAllListeners();

        // �̺�Ʈ ����
        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        seSlider.onValueChanged.AddListener(OnSeVolumeChanged);

        isInitialized = true;
    }

    private void OnBgmVolumeChanged(float value)
    {
        if (!isInitialized || AudioManager.Instance == null) return;
        AudioManager.Instance.SetBgmVolume(value);
    }

    private void OnSeVolumeChanged(float value)
    {
        if (!isInitialized || AudioManager.Instance == null) return;
        AudioManager.Instance.SetSeVolume(value);
        AudioManager.Instance.PlaySE("ui_hover"); // ���� �׽�Ʈ ȿ����
    }

    public void OnClickAccept()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SaveVolumeSettings();
        }

        originalBgmVolume = bgmSlider.value;
        originalSeVolume = seSlider.value;

        optionPanel.SetActive(false);
    }

    public void OnClickCancel()
    {
        if (AudioManager.Instance != null)
        {
            // �� ����
            AudioManager.Instance.SetBgmVolume(originalBgmVolume);
            AudioManager.Instance.SetSeVolume(originalSeVolume);
        }

        // UI ���� (�����̴� �̺�Ʈ�� �ٽ� ȣ����� �ʵ��� SetValueWithoutNotify)
        bgmSlider.SetValueWithoutNotify(originalBgmVolume);
        seSlider.SetValueWithoutNotify(originalSeVolume);

        optionPanel.SetActive(false);
    }
}