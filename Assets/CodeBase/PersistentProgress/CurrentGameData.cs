using System;

namespace CodeBase.PersistentProgress
{
    [Serializable]
    public class CurrentGameData
    {
        public int CurrentLevel { get; set; }

        public CurrentGameData()
        {
            CurrentLevel = 1;
        }
    }
}