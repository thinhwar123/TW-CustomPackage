using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace TW.Utility.Extension
{
    /// <summary>
    /// A utility class for retrieving the content of a Google spreadsheet in CSV or Json format.
    /// For example, to retrieve the content of the "Sheet1" tab in https://docs.google.com/spreadsheets/d/16WoJtP7SzaI9F8B_6MV0vIF57I8M7W3-DhL4mKVAF0Y/edit#gid=0
    /// spreadsheetId = 16WoJtP7SzaI9F8B_6MV0vIF57I8M7W3-DhL4mKVAF0Y
    /// tabName = Sheet1
    /// </summary>
    public static class ABakingSheet
    {
        /// <summary>
        /// Retrieves the content of a spreadsheet specified by its ID and tab name in CSV format.
        /// </summary>
        /// <param name="spreadsheetId">The ID of the target spreadsheet.</param>
        /// <param name="tabName">The name of the tab (worksheet) within the spreadsheet.</param>
        /// <returns>A Task representing the asynchronous operation, returning the spreadsheet content in CSV format.</returns>
        public static async Task<string> GetCsv(string spreadsheetId, string tabName)
        {
            string json = await SendRequest(spreadsheetId, tabName);
            return JsonToCsv(json);
        }
        
        /// <summary>
        /// Retrieves the content of a spreadsheet specified by its ID and tab name in JSON format.
        /// </summary>
        /// <param name="spreadsheetId">The ID of the target spreadsheet.</param>
        /// <param name="tabName">The name of the tab (worksheet) within the spreadsheet.</param>
        /// <returns>A Task representing the asynchronous operation, returning the spreadsheet content in JSON format.</returns>
        public static async Task<string> GetJson(string spreadsheetId, string tabName)
        {
            return await SendRequest(spreadsheetId, tabName);
        }
        
        public static async Task<List<Dictionary<string, string>>> GetDataTable(string spreadsheetId, string tabName)
        {
            string json = await SendRequest(spreadsheetId, tabName);
            return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
        }

        private static async Task<string> SendRequest(string spreadsheetId, string tabName)
        {
            string url = $"https://opensheet.elk.sh/{spreadsheetId}/{tabName}";
            UnityWebRequest www = UnityWebRequest.Get(url);

            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Update Success {tabName}");
                return www.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Error: " + www.error);
                return "";
            }
        }

        private static string JsonToCsv(string json)
        {
            StringBuilder csv = new StringBuilder();

            // Deserialize JSON array
            List<Dictionary<string, object>> dataList =
                JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

            // Get all unique keys from all dictionaries
            HashSet<string> allKeys = new HashSet<string>();
            foreach (var data in dataList)
            {
                allKeys.UnionWith(data.Keys);
            }

            // Write header
            csv.AppendLine(string.Join(",", allKeys));

            // Write data
            foreach (var data in dataList)
            {
                List<string> values = new List<string>();
                foreach (string key in allKeys)
                {
                    // Handle special characters and escaping
                    string value = data.TryGetValue(key, out object value1) ? EscapeCsvField(Convert.ToString(value1)) : "";
                    values.Add(value);
                }

                csv.AppendLine(string.Join(",", values));
            }

            return csv.ToString();
        }

        private static string EscapeCsvField(string field)
        {
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                field = $"\"{field}\"";
            }

            return field;
        }
    }

    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation m_AsyncOp;
        private Action m_Continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.m_AsyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted => m_AsyncOp.isDone;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            this.m_Continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            m_Continuation();
        }
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}