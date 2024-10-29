using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.PersistentProgress;
using Cysharp.Threading.Tasks;
using TigerForge;

namespace CodeBase.SaveLoad.Service
{
    public class SaveLoadService : IService
    {
        private const string CurrentGameProgress = "CurrentGameProgress";
        private const string CurrentGameData = "CurrentGameData";
        private const string Default = "wUH*FW(OKIM";

        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SaveProgress()
        {
            var saveFile = new EasyFileSave(CurrentGameProgress);
            string data = DataExtensions.ToJson(_progressService.Progress.CurrentGameData);
            saveFile.AddSerialized(CurrentGameData, data);
            saveFile.Save(Default);
        }

        public async UniTask<PlayerProgress> LoadProgress()
        {
            var progress = new PlayerProgress();
            progress.CurrentGameData = await LoadCurrentGameProgress();

            return progress;
        }

        public async UniTask<CurrentGameData> LoadCurrentGameProgress()
        {
            var saveFile = new EasyFileSave(CurrentGameProgress);
            if (!saveFile.Load(Default)) {
                return new CurrentGameData();
            }

            var currentGameData = DataExtensions.FromJson<CurrentGameData>((string) saveFile.GetDeserialized(CurrentGameData, typeof(string)));
            saveFile.Dispose();
            return currentGameData;
        }
    }
}