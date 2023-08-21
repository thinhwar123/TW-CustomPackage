using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW.DesignPattern
{
    public class PlayerPrefsDataAccess : IDataAccess
    {
        public void SetData<T>(string dataKey, T value)
        {
            if (value is int)
            {
                PlayerPrefs.SetInt(dataKey, (int)(value as object));
            }
            else if (value is float)
            {
                PlayerPrefs.SetFloat(dataKey, (float)(value as object));
            }
            else if (value is string)
            {
                PlayerPrefs.SetString(dataKey, (string)(value as object));
            }

        }
        public T GetData<T>(string dataKey, T defaultValue = default)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(dataKey, (int)(object)defaultValue);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)PlayerPrefs.GetFloat(dataKey, (float)(object)defaultValue);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(dataKey, (string)(object)defaultValue);
            }
            else
            {
                Debug.LogError("Unsupported data type.");
                return defaultValue;
            }
        }

        public void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
        }
    }

}