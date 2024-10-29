using CodeBase.Extension;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.UI.Component;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.UI.Service
{
    public class UIService
    {
        private readonly AssetProvider _assets;

        private RectTransform _uiRoot;
        public RectTransform MainUIContainer { get; set; }
        public RectTransform OverallContainer { get; set; }
        public LoadingCurtain LoadingCurtain { get; set; }

        public UIService(AssetProvider assets)
        {
            _assets = assets;
        }

        public void Clear()
        {
            if (_uiRoot == null) {
                return;
            }
            Object.Destroy(_uiRoot.gameObject);
            MainUIContainer.RemoveAllChildren();
        }
        
        public void ClearMainUI()
        {
            MainUIContainer.RemoveAllChildren();
        }

        public async UniTask CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(AssetAddress.UIRoot);
            _uiRoot = root.GetComponent<RectTransform>();
            Canvas = root.GetComponent<Canvas>();
            InitContainers(root);
        }

        private void InitContainers(GameObject root)
        {
            var uiRoot = root.GetComponent<UIRoot>();
            if (uiRoot == null) {
                return;
            }

            /*MainUIContainer = uiRoot.MainUIContainer;
            OverallContainer = uiRoot.OverallContainer;*/
            Object.Destroy(uiRoot);
        }

        public void AttachToMainUI(Transform transform) => transform.SetParent(MainUIContainer);

        public Canvas Canvas { get; private set; }
    }
}