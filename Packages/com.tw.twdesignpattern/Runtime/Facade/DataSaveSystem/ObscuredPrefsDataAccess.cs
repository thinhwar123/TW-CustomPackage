using CodeStage.AntiCheat.Storage;

namespace TW.DesignPattern
{
    public class ObscuredPrefsDataAccess : IDataAccess
    {
        public T GetData<T>(string dataKey, T defaultValue = default)
        {
            return ObscuredPrefs.Get<T>(dataKey, defaultValue);
        }

        public void ClearAllData()
        {
            ObscuredPrefs.DeleteAll();
        }

        public void SetData<T>(string dataKey, T value)
        {
            ObscuredPrefs.Set<T>(dataKey, value);
        }
    } 
}
