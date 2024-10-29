using System;
using CodeBase.Dialogs.UI;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Factory;
using CodeBase.UI.Service;
using Cysharp.Threading.Tasks;

namespace CodeBase.Dialogs.Service
{
    public class DialogService : IService
    {
        private readonly UIService _uiService;
        private readonly GameFactory _gameFactory;

        public DialogService(UIService uiService, GameFactory gameFactory)
        {
            _uiService = uiService;
            _gameFactory = gameFactory;
        }
        
        public async UniTaskVoid ShowDialog(string title, Action onCloseButton)
        {
            DialogPanel panel = await _gameFactory.CreateDialog(_uiService.OverallContainer);
            panel.Construct(title, onCloseButton);
        }
    }
}