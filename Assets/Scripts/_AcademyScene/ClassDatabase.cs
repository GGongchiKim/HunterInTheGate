using System.Collections.Generic;
using UnityEngine;

public class ClassDatabase : MonoBehaviour
{
    public static ClassDatabase Instance { get; private set; }

    [Header("��ϵ� ��� ����")]
    public List<ClassData> allClasses = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// ���� ������ ���� ����Ʈ�� ��ȯ�մϴ�. ���� ���� �߰� ����.
    /// </summary>
    public List<ClassData> GetAvailableClasses()
    {
        // TODO: ����, NPC ���� � ���� ���͸� ���� �߰� ����
        return allClasses;
    }
}
