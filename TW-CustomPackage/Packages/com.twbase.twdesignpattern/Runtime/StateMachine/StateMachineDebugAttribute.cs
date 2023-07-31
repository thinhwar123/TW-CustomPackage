#region TW.DesignPattern

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor; 
#endif

using UnityEngine;

public interface IStateMachineDebug
{
    public bool IsRunning();
    public string GetCurrentState();
    public string[] GetPendingTransitionStateQueue();
}

public sealed class StateMachineDebugAttribute : System.Attribute
{

}

public sealed class StateMachineDebugAttributeDrawer<T> : OdinAttributeDrawer<StateMachineDebugAttribute, T>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        IStateMachineDebug value = (this.ValueEntry.SmartValue as IStateMachineDebug);

        bool isRunning = value != null && value.IsRunning();
        Rect rect = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), label);
        EditorGUI.Toggle(rect, isRunning);

        string currentState = value?.GetCurrentState();
        if (currentState == null) return;
        EditorGUILayout.LabelField(new GUIContent($"Current State : {currentState}"));

        string[] pendingTransitionStateQueue = value?.GetPendingTransitionStateQueue();
        if (pendingTransitionStateQueue == null || pendingTransitionStateQueue.Length == 0) return;
        for (int i = 0; i < pendingTransitionStateQueue.Length; i++)
        {
            EditorGUILayout.LabelField(new GUIContent($"{i} Pending Queue State : {pendingTransitionStateQueue[i]}"));
        }
    }
}

#endregion
