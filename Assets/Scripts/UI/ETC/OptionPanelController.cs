using UnityEngine;
using UnityEngine.UI;

public class OptionPanelController : MonoBehaviour
{
    [Header("슬라이더")]
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
        // 원래 값 기억 (Cancel 복원용)
        originalBgmVolume = AudioManager.Instance.BgmVolume;
        originalSeVolume = AudioManager.Instance.SeVolume;

        // 슬라이더 초기화
        bgmSlider.SetValueWithoutNotify(originalBgmVolume);
        seSlider.SetValueWithoutNotify(originalSeVolume);

        // 리스너 중복 방지
        bgmSlider.onValueChanged.RemoveAllListeners();
        seSlider.onValueChanged.RemoveAllListeners();

        // 이벤트 연결
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
        AudioManager.Instance.PlaySE("ui_hover"); // 볼륨 테스트 효과음
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
            // 값 복원
            AudioManager.Instance.SetBgmVolume(originalBgmVolume);
            AudioManager.Instance.SetSeVolume(originalSeVolume);
        }

        // UI 복원 (슬라이더 이벤트가 다시 호출되지 않도록 SetValueWithoutNotify)
        bgmSlider.SetValueWithoutNotify(originalBgmVolume);
        seSlider.SetValueWithoutNotify(originalSeVolume);

        optionPanel.SetActive(false);
    }
}