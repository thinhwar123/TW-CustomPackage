using UnityEngine;

public class AJsonReader : MonoBehaviour
{
    public static T Read<T>(TextAsset data)
    {
        return JsonUtility.FromJson<T>(data.text);
    }
    public static T Read<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }
    public static string Write<T>(T data, bool prettyPrint = false)
    {
        return JsonUtility.ToJson(data, prettyPrint);
    }
}
