using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Model
{
    [CreateAssetMenu(fileName = "BubbleData", menuName = "StaticData/Bubble")]
    public class BubbleStaticData : ScriptableObject
    {
        public BubbleTypeId BubbleTypeId;
        public BubbleTypeId UpperBubbleTypeId;
        public AssetReferenceGameObject PrefabReference;
        public bool CanSpawn;
        public int SpawnWeight;
        public int MergePoints;
    }
}