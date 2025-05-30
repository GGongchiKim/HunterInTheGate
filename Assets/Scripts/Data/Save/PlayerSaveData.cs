using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class PlayerSaveData
    {
        public PlayerStatsSaveData stats = new();
        public PlayerInventorySaveData inventory = new();
        public GameProgressSaveData progress = new();
    }
}