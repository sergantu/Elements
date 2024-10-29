using System.Collections.Generic;
using CodeBase.StaticData.Model;

namespace CodeBase.StaticData.Service
{
    public interface IStaticDataService
    {
        void Load(BubbleTheme theme);
        BubbleStaticData ForBubble(BubbleTypeId typeId);
        List<BubbleStaticData> GetSpawned();
    }
}