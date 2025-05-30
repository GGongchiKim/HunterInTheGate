using System;
using System.Collections.Generic;

/// <summary>
/// 인벤토리에서 사용하는 덱 프리셋의 임시 데이터 구조.
/// 실제 저장은 DeckSaveData를 사용.
/// </summary>
[Serializable]
public class I_DeckPresetData
{
    public string deckName;
    public List<string> cardIds = new();

    public bool isSelected = false;      // 현재 탐험 덱 여부
    public bool isFavorite = false;      // 즐겨찾기
    public int slotIndex = -1;           // UI 상 위치
    public string note = "";             // 유저 메모

    public I_DeckPresetData() { }

    public I_DeckPresetData(string name)
    {
        deckName = name;
        cardIds = new List<string>();
    }

    /// <summary>
    /// 현재 상태의 깊은 복사본을 생성
    /// </summary>
    public I_DeckPresetData DeepCopy()
    {
        return new I_DeckPresetData
        {
            deckName = this.deckName,
            cardIds = new List<string>(this.cardIds),
            isSelected = this.isSelected,
            isFavorite = this.isFavorite,
            slotIndex = this.slotIndex,
            note = this.note
        };
    }
}