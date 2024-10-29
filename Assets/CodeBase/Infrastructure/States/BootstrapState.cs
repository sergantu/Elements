using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly AssetProvider _assetProvider;
        private readonly GameStateMachine _stateMachine;

        public BootstrapState(AssetProvider assetProvider, GameStateMachine stateMachine)
        {
            _assetProvider = assetProvider;
            _stateMachine = stateMachine;
        }

        public async UniTaskVoid Enter()
        {
            EnterLoadLevel().Forget();
        }

        public async UniTaskVoid Exit()
        {
        }

        private async UniTaskVoid EnterLoadLevel()
        {
            _assetProvider.Construct();
            _stateMachine.Enter<LoadProgressState>().Forget();
        }
    }
}