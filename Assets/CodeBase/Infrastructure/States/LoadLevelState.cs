using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.PersistentProgress;
using CodeBase.SaveLoad.Service;
using CodeBase.StaticData.Service;
using CodeBase.UI.Service;
using CodeBase.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly GameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly UIService _uiService;
        private readonly SaveLoadService _saveLoadService;
        private readonly GameStateMachine _stateMachine;

        public LoadLevelState(SceneLoader sceneLoader,
                              GameFactory gameFactory,
                              IPersistentProgressService progressService,
                              IStaticDataService staticData,
                              UIService uiService,
                              SaveLoadService saveLoadService,
                              GameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiService = uiService;
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
        }

        public async UniTaskVoid Enter()
        {
            _gameFactory.Cleanup();
            try {
                await _gameFactory.WarmUp();
                OnLoaded().Forget();
            } catch (Exception e) {
                Debug.LogError("Enter LoadLevelState1 error " + e.Message);
                Enter().Forget();
            }
        }

        public async UniTaskVoid Exit()
        {
            /*_uiService.LoadingCurtain.Hide();*/
        }

        private async UniTaskVoid OnLoaded()
        {
            try {
                _uiService.Clear();
                /*_staticData.Load(_progressService.Progress.WorldData.CurrentBubbleTheme);
                await _gameFactory.WarmUpLevel(_progressService.Progress.WorldData.CurrentBubbleTheme);

                await InitGameBoard();
                await InitUIRoot();
                await InitMenuUI();*/
            } catch (Exception e) {
                Debug.LogError("LoadLevelState1 error " + e.Message);
                OnLoaded().Forget();
            }

            _stateMachine.Enter<GameLoopState>().Forget();
        }

        private async UniTask InitUIRoot() => await _uiService.CreateUIRoot();
        private async UniTask InitMenuUI() => await _gameFactory.CreateMenu(_uiService.MainUIContainer);

        private async UniTask InitGameBoard()
        {
            /*GameLogic.Component.GameLevel level = await _gameFactory.CreateGameLevel();
            _bubbleController.SetGameBoard(level);
            _gameLogicService.SetGameLevel(level);*/
        }
    }
}