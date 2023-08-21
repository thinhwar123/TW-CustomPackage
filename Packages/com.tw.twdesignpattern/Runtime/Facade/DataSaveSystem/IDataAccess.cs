namespace TW.DesignPattern
{
    public interface IDataAccess
    {
        /// <summary>
        /// Set data by key
        /// </summary>
        /// <typeparam name="T">typeof data</typeparam>
        /// <param name="dataKey">key of data</param>
        /// <param name="value">value of data</param>
        public void SetData<T>(string dataKey, T value);
        /// <summary>
        /// Get data by key
        /// </summary>
        /// <typeparam name="T">typeof data</typeparam>
        /// <param name="dataKey">key of data</param>
        /// <param name="defaultValue">default value</param>
        /// <returns>Return defaultValue</returns>
        public T GetData<T>(string dataKey, T defaultValue = default(T));

        /// <summary>
        /// Clear all data
        /// </summary>
        public void ClearAllData();
    }

}