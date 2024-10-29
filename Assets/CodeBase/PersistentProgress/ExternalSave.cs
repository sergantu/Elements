using System;

namespace CodeBase.PersistentProgress
{
    [Serializable]
    public class ExternalSave
    {
        public bool WorldData { get; set; }
        public bool CurrentGameData { get; set; }
        public bool PayableData { get; set; }

        public ExternalSave()
        {
            WorldData = false;
            CurrentGameData = false;
            PayableData = false;
        }
    }
}