using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ��ȯ �� ������ Ű-�� �����͸� �����ϴ� �긮�� ������ �̱���.
/// ���� �ε�� �� �����͸� ���� ����, �ڵ����� �����ϴ� ���.
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
    /// ������ ����
    /// </summary>
    public void SetData(string key, object value)
    {
        data[key] = value;
    }

    /// <summary>
    /// �����鼭 ����
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
    /// �״�� ���⸸ (���� X)
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
    /// ���ڿ��� ���� ������ (���� ����)
    /// </summary>
    public string GetString(string key)
    {
        return ConsumeData<string>(key);
    }

    /// <summary>
    /// Ű ���� ���� Ȯ��
    /// </summary>
    public bool HasData(string key)
    {
        return data.ContainsKey(key);
    }

    /// <summary>
    /// Ÿ�� Ȯ�� ���� ��ȯ (���� X)
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