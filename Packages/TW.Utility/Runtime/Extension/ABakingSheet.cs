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
    public static class ABakingSheet
    {
        public static async Task<string> GetCsv(string spreadsheetId, string tabName)
        {
            string json = await SendRequest(spreadsheetId, tabName);
            return JsonToCsv(json);
        }

        public static async Task<string> GetJson(string spreadsheetId, string tabName)
        {
            return await SendRequest(spreadsheetId, tabName);
        }

        private static async Task<string> SendRequest(string spreadsheetId, string tabName)
        {
            string url = $"https://opensheet.elk.sh/{spreadsheetId}/{tabName}";
            Debug.Log(url);
            UnityWebRequest www = UnityWebRequest.Get(url);

            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Success");
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

            // Write header
            csv.AppendLine(string.Join(",", dataList[0].Keys));

            // Write data
            foreach (var data in dataList)
            {
                List<string> values = new List<string>();
                foreach (string key in data.Keys)
                {
                    // Handle special characters and escaping
                    string value = EscapeCsvField(Convert.ToString(data[key]));
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