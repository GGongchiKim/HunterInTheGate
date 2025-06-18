using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 전환 시 간단한 키-값 데이터를 전달하는 브리지 역할의 싱글톤.
/// 씬이 로드된 후 데이터를 꺼내 쓰고, 자동으로 삭제하는 방식.
/// </summary>
public class SceneDataBridge : MonoBehaviour
{
    public static SceneDataBridge Instance { get; private set; }

    private Dictionary<string, object> data = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 씬 전환 전에 데이터를 저장합니다.
    /// </summary>
    public void SetData(string key, object value)
    {
        data[key] = value;
    }

    /// <summary>
    /// 데이터를 가져오고, 가져온 후 해당 항목은 삭제합니다.
    /// </summary>
    public T ConsumeData<T>(string key)
    {
        if (data.TryGetValue(key, out object value) && value is T typedValue)
        {
            data.Remove(key);
            return typedValue;
        }

        return default;
    }

    /// <summary>
    /// 데이터를 존재한 채로 확인만 합니다 (삭제하지 않음).
    /// </summary>
    public T PeekData<T>(string key)
    {
        if (data.TryGetValue(key, out object value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    /// <summary>
    /// 전체 데이터 초기화
    /// </summary>
    public void ClearAll()
    {
        data.Clear();
    }
}