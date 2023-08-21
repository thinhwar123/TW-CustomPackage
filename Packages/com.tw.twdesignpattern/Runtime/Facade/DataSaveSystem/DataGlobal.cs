using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW.DesignPattern
{
    public class DataGlobal : Singleton<DataGlobal>
    {
        private IDataAccess m_DataAccess;
        public IDataAccess DataAccess => m_DataAccess ??= new ObscuredPrefsDataAccess();

        public T GetData<T>(string dataKey, T defaultValue = default)
        {
            return DataAccess.GetData<T>(dataKey, defaultValue);
        }
        public void SetData<T>(string dataKey, T value)
        {
            DataAccess.SetData<T>(dataKey, value);
        }

        public void ClearAllData()
        {
            DataAccess.ClearAllData();
        }
    } 
}
