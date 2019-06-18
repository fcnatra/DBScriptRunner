namespace DbScriptRunnerLogic.Interfaces
{
    public interface IRepository
    {
        string Name { get; set; }
        string Location { get; set; }
        void Save(string content);
        string Load();
    }
}