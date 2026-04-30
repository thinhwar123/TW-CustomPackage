namespace DataModule
{
    public interface IDataModule
    {
        string Key { get; }
        int Version { get; }

        void SetDefault();
        void FromJson(string json);
        string ToJson();

        void Migrate();
    }
}