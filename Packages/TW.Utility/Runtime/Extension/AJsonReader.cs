using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TW.Utility.Extension
{
    public static class AJsonReader
    {
        public static T DeserializeObject<T>(string json)
        {
            // Check if the json string is not null or empty
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("JSON string is null or empty.");
                return default(T);
            }

            // Create an instance of the target type
            T obj = Activator.CreateInstance<T>();

            // Split the JSON string into key-value pairs
            Dictionary<string, string> keyValuePairs = ParseJson(json);

            // Get the properties of the target type
            PropertyInfo[] properties = typeof(T).GetProperties();

            // Iterate through properties and set their values based on JSON data
            foreach (PropertyInfo property in properties)
            {
                // Check if the property exists in the JSON data
                if (keyValuePairs.TryGetValue(property.Name, out string value))
                {
                    // Convert the string value to the property type and set it
                    object convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(obj, convertedValue);
                }
            }

            return obj;
        }

        // Simple JSON parser to extract key-value pairs
        private static Dictionary<string, string> ParseJson(string json)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            // Split JSON string by commas to get key-value pairs
            string[] pairs = json.Split(',');

            foreach (string pair in pairs)
            {
                // Split each pair by colon to separate key and value
                string[] keyValue = pair.Split(':');

                if (keyValue.Length == 2)
                {
                    // Remove leading and trailing whitespaces
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    // Remove quotes from the value
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    // Add key-value pair to the dictionary
                    keyValuePairs[key] = value;
                }
            }

            return keyValuePairs;
        }
    }
}
