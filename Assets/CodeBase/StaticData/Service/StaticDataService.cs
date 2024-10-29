using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Model;
using UnityEngine;

namespace CodeBase.StaticData.Service
{
    public class StaticDataService : IStaticDataService
    {
        private const string BubblesDataPath = "StaticData/Bubbles/";

        private Dictionary<BubbleTypeId, BubbleStaticData> _bubbles;

        public void Load(BubbleTheme theme) =>
                _bubbles = Resources.LoadAll<BubbleStaticData>(BubblesDataPath + theme).ToDictionary(x => x.BubbleTypeId, x => x);

        public BubbleStaticData ForBubble(BubbleTypeId typeId) => _bubbles.TryGetValue(typeId, out BubbleStaticData staticData) ? staticData : null;

        public List<BubbleStaticData> GetSpawned() => _bubbles.Values.Where(bubble => bubble.CanSpawn).ToList();
    }
}