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
    /// 데이터 설정
    /// </summary>
    public void SetData(string key, object value)
    {
        data[key] = value;
    }

    /// <summary>
    /// 꺼내면서 삭제
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
    /// 그대로 보기만 (삭제 X)
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
    /// 문자열용 단축 접근자 (삭제 포함)
    /// </summary>
    public string GetString(string key)
    {
        return ConsumeData<string>(key);
    }

    /// <summary>
    /// 키 존재 여부 확인
    /// </summary>
    public bool HasData(string key)
    {
        return data.ContainsKey(key);
    }

    /// <summary>
    /// 타입 확인 포함 반환 (삭제 X)
    /// </summary>
    public T GetData<T>(string key)
    {
        if (data.TryGetValue(key, out object value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    public void ClearAll()
    {
        data.Clear();
    }
}