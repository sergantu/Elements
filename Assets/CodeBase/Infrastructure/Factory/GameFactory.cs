using System;
using CodeBase.Dialogs.UI;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.PersistentProgress;
using CodeBase.StaticData.Model;
using CodeBase.StaticData.Service;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IService
    {
        private readonly AssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        public GameFactory(AssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _staticData = staticData;
            _assets = assets;
        }

        public async UniTask WarmUp()
        {
            /*await _assets.Load<GameObject>(AssetAddress.UIRoot);
            await _assets.Load<GameObject>(AssetAddress.MenuUI);
            await _assets.Load<GameObject>(AssetAddress.GamePlayUI);
            await _assets.Load<GameObject>(AssetAddress.WinWindow);
            await _assets.Load<GameObject>(AssetAddress.DialogPanel);*/
        }

        public async UniTask WarmUpLevel(BubbleTheme bubbleTheme)
        {
            await _assets.Load<GameObject>(bubbleTheme + "Level");

            Array bubbleTypes = Enum.GetValues(typeof(BubbleTypeId));
            foreach (BubbleTypeId bubble in bubbleTypes) {
                BubbleStaticData bubbleStaticData = _staticData.ForBubble(bubble);
                await _assets.Load<GameObject>(bubbleStaticData.PrefabReference);
            }
        }
        
        public async UniTask<GameObject> CreateBoard()
        {
            return await _assets.Instantiate(AssetAddress.Board);
        }
        
        public async UniTask<GameObject> CreateElement(string name, Vector3 position)
        {
            return await _assets.Instantiate(name, position);
        }

        public async UniTask<GameObject> CreateFireElement(Vector3 position, Transform parent)
        {
            return await _assets.Instantiate(AssetAddress.Fire, position, parent);
        }
        public async UniTask<GameObject> CreateWaterElement(Vector3 position, Transform parent)
        {
            return await _assets.Instantiate(AssetAddress.Water, position, parent);
        }
        
        
        

        public async UniTask CreateMenu(Transform parent)
        {
            await _assets.Instantiate(AssetAddress.MenuUI, parent);
        }

        public async UniTask<DialogPanel> CreateDialog(Transform parent)
        {
            GameObject ui = await _assets.Instantiate(AssetAddress.DialogPanel, parent);
            return ui.GetComponent<DialogPanel>();
        }

        public void Cleanup() => _assets.Cleanup();
    }
}