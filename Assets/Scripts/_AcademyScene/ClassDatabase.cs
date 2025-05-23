using System.Collections.Generic;
using UnityEngine;

public class ClassDatabase : MonoBehaviour
{
    public static ClassDatabase Instance { get; private set; }

    [Header("등록된 모든 수업")]
    public List<ClassData> allClasses = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 선택 가능한 수업 리스트를 반환합니다. 추후 필터 추가 가능.
    /// </summary>
    public List<ClassData> GetAvailableClasses()
    {
        // TODO: 요일, NPC 조건 등에 따른 필터링 로직 추가 예정
        return allClasses;
    }
}
