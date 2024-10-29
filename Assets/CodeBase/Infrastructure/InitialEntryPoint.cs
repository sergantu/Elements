using CodeBase.Infrastructure.States;
using VContainer.Unity;

namespace CodeBase.Infrastructure
{
    public class InitialEntryPoint : IStartable
    {
        private readonly GameStateMachine _stateMachine;

        public InitialEntryPoint(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Start()
        {
            /*Application.logMessageReceived += HandleException;
            UniTaskScheduler.UnobservedTaskException += HandleUniTaskException;*/

            _stateMachine.Init();
            _stateMachine.Enter<BootstrapState>().Forget();
        }
    }
}