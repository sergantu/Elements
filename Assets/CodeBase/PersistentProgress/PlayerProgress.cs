using System;

namespace CodeBase.PersistentProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerProgress()
        {
            CurrentGameData = new CurrentGameData();
        }

        public CurrentGameData CurrentGameData { get; set; }
    }
}