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

        public List<string> activeQuests = new();     // 진행 중인 퀘스트 ID
        public List<string> completedQuests = new();  // 완료한 퀘스트 ID
    }
}
