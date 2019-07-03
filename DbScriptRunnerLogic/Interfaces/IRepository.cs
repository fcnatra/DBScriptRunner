namespace DbScriptRunnerLogic.Interfaces
{
    public interface IRepository : IRepositoryInformation
    {
        void Save(string content);
        string Load();
    }
}