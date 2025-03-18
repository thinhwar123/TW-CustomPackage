#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor; 
#endif

using UnityEngine;

namespace TW.Utility.DesignPattern
{
    public interface IStateMachineDebug
    {
        public bool IsOnRunning();
        public string GetCurrentState();
        public string[] GetPendingTransitionStateQueue();
    }

    public sealed class StateMachineDebugAttribute : System.Attribute
    {

    }

#if UNITY_EDITOR
    public sealed class StateMachineDebugAttributeDrawer<T> : OdinAttributeDrawer<StateMachineDebugAttribute, T>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            IStateMachineDebug value = (this.ValueEntry.SmartValue as IStateMachineDebug);

            bool isRunning = value != null && value.IsOnRunning();
            Rect rect = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), label);
            EditorGUI.Toggle(rect, isRunning);

            string currentState = value?.GetCurrentState();
            if (currentState == null) return;
            EditorGUILayout.LabelField(new GUIContent($"Current UniTaskState : {currentState}"));

            string[] pendingTransitionStateQueue = value?.GetPendingTransitionStateQueue();
            if (pendingTransitionStateQueue == null || pendingTransitionStateQueue.Length == 0) return;
            for (int i = 0; i < pendingTransitionStateQueue.Length; i++)
            {
                EditorGUILayout.LabelField(new GUIContent($"{i} Pending Queue UniTaskState : {pendingTransitionStateQueue[i]}"));
            }
        }
    } 
#endif

}
