using CodeBase.Infrastructure;

namespace CodeBase.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}