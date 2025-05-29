using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class GameProgressSaveData
    {
        public string playerName;
        public int inGameYear;
        public int inGameWeek;
        public HunterGrade hunterRank;

        public List<string> activeQuests = new();     // ���� ���� ����Ʈ ID
        public List<string> completedQuests = new();  // �Ϸ��� ����Ʈ ID
    }
}
