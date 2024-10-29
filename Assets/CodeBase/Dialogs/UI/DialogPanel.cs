using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Dialogs.UI
{
    public class DialogPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private Button _closeButton;

        private Action _onCloseButton;

        public void Construct(string title, Action onCloseButton)
        {
            _title.text = title;
            _onCloseButton = onCloseButton;
            AddListeners();
        }

        private void AddListeners() => _closeButton.onClick.AddListener(OnCloseClick);

        private void OnCloseClick()
        {
            _onCloseButton?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy() => _closeButton.onClick.RemoveAllListeners();
    }
}